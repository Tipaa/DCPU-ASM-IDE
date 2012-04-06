using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace DCPU_16
{

    #region Emulator
    public partial class Emulator
    {
        public static Cpu cpu1;
        public static Cpu cpu2;
        public static Cpu cpu3;

        public static ushort[] extraMemory;

        public static List<ushort> emulateFromBinary(string input)
        {
            List<ushort> mem = new List<ushort>();
            foreach (var line in input.Split('\n', '\r'))
            {
                foreach (var b in line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    MessageBox.Show(b);
                    mem.Add(ushort.Parse(b, NumberStyles.HexNumber));
                }
            }
            return mem;
        }

        public static void emulateFromFile(string filepath)
        {
            string binary = File.ReadAllText(filepath);
            emulateFromBinary(binary);
        }
    }

    public partial class Cpu
    {
        public Cpu(ushort[] initalMemory)
        {
            for (int i = 0; i < initalMemory.Length; i++)
            {
                Memory[i] = initalMemory[i];
            }
        }

        private readonly ushort[] Memory = new ushort[0x10000];
        private readonly ushort[] Registers = new ushort[8];
        private static readonly string[] RegisterNames = new string[] { "A", "B", "C", "X", "Y", "Z", "I", "J" };
        private ushort PC = 0;
        private ushort SP = 0;
        private ushort Overflow = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>true if basic, false otherwise</returns>
        private bool GetInstr(out int op, out Value a, out Value b)
        {
            ushort instr = Memory[PC++];
            if ((instr & 0xf) == 0)
            {
                //non-basic
                op = (instr >> 4) & 0x3f;
                a = CreateValue((ushort)(instr >> 10));
                b = default(Value);
                return false;
            }
            else
            {
                //basic
                op = instr & 0xf;
                a = CreateValue((ushort)((instr >> 4) & 0x3f));
                b = CreateValue((ushort)(instr >> 10));
                return true;
            }
        }

        private void Skip()
        {
            //Use GetInstr to get the side effect of advancing the PC,
            //but don't let the SP change.
            Value a, b;
            int op;
            ushort temp = SP;
            GetInstr(out op, out a, out b);
            SP = (ushort)temp;
        }

        public void Tick()
        {
            int op;
            Value a, b;
            if (!GetInstr(out op, out a, out b))
            {
                //non-basic
                switch (op)
                {
                    case 0x1: //JSR
                        Memory[--SP] = PC;
                        PC = a.Get();
                        break;
                    default:
                        throw new UnsupportedInstructionException(op);
                }
            }
            else
            {
                //basic
                uint temp;
                switch (op)
                {
                    case 0x1: //SET
                        a.Set(b.Get());
                        break;
                    case 0x2: //ADD
                        temp = (uint)(a.Get() + b.Get());
                        a.Set((ushort)(temp & 0xffff));
                        Overflow = temp > 0xffff ? (ushort)1 : (ushort)0;
                        break;
                    case 0x3: //SUB
                        temp = (uint)(a.Get() - b.Get());
                        a.Set((ushort)(temp & 0xffff));
                        Overflow = temp > 0xffff ? (ushort)0xffff : (ushort)0;
                        break;
                    case 0x4: //MUL
                        temp = (uint)(a.Get() * b.Get());
                        a.Set((ushort)temp);
                        Overflow = (ushort)((temp >> 16) & 0xffff);
                        break;
                    case 0x5: //DIV
                        if (b.Get() == 0)
                        {
                            a.Set(0);
                            Overflow = 0;
                        }
                        else
                        {
                            a.Set((ushort)(a.Get() / b.Get()));
                            Overflow = (ushort)(((a.Get() << 16) / b.Get()) & 0xffff);
                        }
                        break;
                    case 0x6: //MOD
                        if (b.Get() == 0)
                            a.Set(0);
                        else
                            a.Set((ushort)(a.Get() % b.Get()));
                        break;
                    case 0x7: //SHL
                        temp = (uint)(a.Get() << b.Get());
                        Overflow = (ushort)((temp >> 16) & 0xffff);
                        a.Set((ushort)temp);
                        break;
                    case 0x8: //SHR
                        a.Set((ushort)(a.Get() >> b.Get()));
                        Overflow = (ushort)(((a.Get() << 16) >> b.Get()) & 0xffff);
                        break;
                    case 0x9: //AND
                        a.Set((ushort)(a.Get() & b.Get()));
                        break;
                    case 0xa: //BOR
                        a.Set((ushort)(a.Get() | b.Get()));
                        break;
                    case 0xb: //XOR
                        a.Set((ushort)(a.Get() ^ b.Get()));
                        break;
                    case 0xc: //IFE
                        if (!(a.Get() == b.Get()))
                            Skip();
                        break;
                    case 0xd: //IFN
                        if (!(a.Get() != b.Get()))
                            Skip();
                        break;
                    case 0xe: //IFG
                        if (!(a.Get() > b.Get()))
                            Skip();
                        break;
                    case 0xf: //IFB
                        if (!((a.Get() & b.Get()) != 0))
                            Skip();
                        break;
                    default:
                        throw new Exception("Should not get to here.");
                }
            }
        }

        public string Status()
        {
            var sb = new StringBuilder();

            foreach (var name in RegisterNames.Concat(new[] { "PC", "SP", "O" }))
            {
                sb.AppendFormat("{0,5}", name);
            }
            sb.AppendLine();

            const string numberFormat = "{0,5:x}";
            for (int i = 0; i < Registers.Length; i++)
            {
                sb.AppendFormat(numberFormat, Registers[i]);
            }
            sb.AppendFormat(numberFormat, PC);
            sb.AppendFormat(numberFormat, SP);
            sb.AppendFormat(numberFormat, Overflow);

            return sb.ToString();
        }
    

        private Value CreateValue(ushort ndx)
        {
            if (ndx < 0x08)
                return new Value(Registers, ndx);
            if (ndx < 0x10)
                return new Value(Memory, Registers[ndx - 0x8]);
            if (ndx < 0x18)
                return new Value(Memory, (ushort)(Memory[PC++] + Registers[ndx - 0x10]));
            switch (ndx)
            {
                case 0x18:
                    return new Value(Memory, SP++); //POP
                case 0x19:
                    return new Value(Memory, SP); //PEEK
                case 0x1a:
                    return new Value(Memory, --SP); //PUSH
                case 0x1b:
                    return new Value(SaveLocation.SP, this);
                case 0x1c:
                    return new Value(SaveLocation.PC, this);
                case 0x1d:
                    return new Value(SaveLocation.Overflow, this);
                case 0x1e:
                    return new Value(Memory, Memory[PC++]);
                case 0x1f:
                    return new Value(Memory[PC++]);
                default:
                    break;
            }
            if (ndx < 0x40)
                return new Value((ushort)(ndx - 0x20));
            throw new ArgumentOutOfRangeException();
        }

        enum SaveLocation
        {
            None = 0,
            Literal,
            Memory,
            SP,
            PC,
            Overflow,
        }

        struct Value
        {
            public Value(ushort[] buffer, ushort index)
            {
                this.Buffer = buffer;
                this.Index = index;
                this.SaveLoc = SaveLocation.Memory;
                this.Literal = 0;
                this.MyCpu = null;
            }

            public Value(SaveLocation loc, Cpu cpu)
            {
                this.Buffer = null;
                this.Index = 0;
                this.SaveLoc = loc;
                this.Literal = 0;
                this.MyCpu = cpu;
            }

            public Value(ushort lit)
            {
                this.Buffer = null;
                this.Index = 0;
                this.SaveLoc = SaveLocation.Literal;
                this.Literal = lit;
                this.MyCpu = null;
            }

            private SaveLocation SaveLoc;
            private ushort[] Buffer;
            private ushort Index;
            private ushort Literal;
            private Cpu MyCpu;

            public ushort Get()
            {
                if (SaveLoc == SaveLocation.Memory)
                    return Buffer[Index];
                else if (SaveLoc == SaveLocation.Literal)
                    return Literal;
                else if (SaveLoc == SaveLocation.SP)
                    return MyCpu.SP;
                else if (SaveLoc == SaveLocation.PC)
                    return MyCpu.PC;
                else if (SaveLoc == SaveLocation.Overflow)
                    return MyCpu.Overflow;
                throw new NotSupportedException(SaveLoc.ToString());
            }

            public void Set(ushort val)
            {
                if (SaveLoc == SaveLocation.Memory)
                    Buffer[Index] = val;
                else if (SaveLoc == SaveLocation.Literal)
                    return; //ignore attempts to set literals
                else if (SaveLoc == SaveLocation.SP)
                    MyCpu.SP = val;
                else if (SaveLoc == SaveLocation.PC)
                    MyCpu.PC = val;
                else if (SaveLoc == SaveLocation.Overflow)
                    MyCpu.Overflow = val;
                else
                    throw new NotSupportedException(SaveLoc.ToString());
            }

            public override string ToString()
            {
                var sb = new StringBuilder();
                if (SaveLoc == SaveLocation.Memory)
                {
                    if (Buffer.Length == 8)
                    {
                        sb.Append("Register: ");
                        sb.Append(RegisterNames[Index]);
                    }
                    else
                    {
                        sb.AppendFormat("Memory: 0x{0:x}", Index);
                    }
                }
                else
                {
                    sb.Append(SaveLoc.ToString());
                }

                if (SaveLoc != SaveLocation.None)
                    sb.AppendFormat(" Value: 0x{0:x}", Get());

                return sb.ToString();
            }
        }
    }

    public class UnsupportedInstructionException : NotSupportedException
    {
        public UnsupportedInstructionException(int instr)
            : base("This instruction in is not supported: 0x" + instr.ToString("x"))
        {
        }
    }

#endregion
}
