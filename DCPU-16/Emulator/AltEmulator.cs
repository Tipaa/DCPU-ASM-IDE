using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCPU_16.Emulator
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    class AltCPU : IEmulator
    {
        public ushort[] Mem = new ushort[0x10000];
        public ushort A, B, C, X, Y, Z, I, J;
        public ushort[] Registers;
        public ushort PC = 0x0, SP = 0x0; //Word of god says SP is initialised to 0, not 0xffff: https://twitter.com/notch/status/187636538870468608
        public ushort O;

        public bool SkipNextInstruction = false;
        public uint Tick;

        public delegate void AdvancedOpcode(AltCPU cpu, ref ushort b);
        public Dictionary<int, AdvancedOpcode> AdvancedOpcodes = new Dictionary<int, AdvancedOpcode>();

        public AltCPU()
        {
            AdvancedOpcodes.Add(0x01, (AltCPU cpu, ref ushort b) =>
            {
                cpu.Mem[--cpu.SP] = cpu.PC;
                cpu.PC = b;
            });
        }

        #region IEmulator Hooks

        public void LoadProgram(string binary)
        {
            Console.WriteLine(binary);
            int i = 0;
            foreach (char c in binary)
            {
                Mem[i] = (ushort)c;
                i++;
            }
        }

        public void LoadProgram(ushort[] binary)
        {
            uint i = 0;
            foreach (ushort u in binary)
            {
                Mem[i++] = u;
            }
        }

        public void StepProgram(uint steps)
        {
            for (int i = 0; i < steps; i++)
            {
                Console.WriteLine(Disassemble(PC));
                Step();
                PrintState();
            }
        }

        public void Reset()
        {
            Mem = new ushort[0x10000];
            A = 0;
            B = 0;
            C = 0;
            I = 0;
            J = 0;
            X = 0;
            Y = 0;
            Z = 0;
            SP = 0;
            PC = 0;
            O = 0;
        }

        public EmulatorState ProvideState()
        {
            Registers = new ushort[] { A, B, C, X, Y, Z, I, J };
            return new EmulatorState(ref Mem, ref Registers, ref Mem, ref SP, ref PC, ref O);
        }

        public void ReceiveState(EmulatorState e)
        {
            Mem = e.Memory;
            Registers = e.Registers;
            A = e.Registers[0];
            B = e.Registers[1];
            C = e.Registers[2];
            X = e.Registers[3];
            Y = e.Registers[4];
            Z = e.Registers[5];
            I = e.Registers[6];
            J = e.Registers[7];
            SP = e.SP;
            PC = e.PC;
            O = e.O;
        }

        #endregion

        public void Step()
        {
            ushort instruction = Mem[PC++];
            int opcode = instruction & 0xF;
            int aLoc = (instruction >> 4) & 0x3F;
            int bLoc = (instruction >> 10) & 0x3F;

            if (opcode != 0)
            {
                RefLoc(aLoc, (ref ushort a) =>
                {
                    ushort b = ReadLoc(bLoc);
                    BasicOpcode(opcode, ref a, b);
                });
            }
            else
            {
                try
                {
                    RefLoc(bLoc, (ref ushort b) => AdvancedOpcodes[aLoc](this, ref b));
                }
                catch (KeyNotFoundException e)
                {
                }
            }

            Tick += (uint)CyclesRequiredForCurrentInstruction(instruction);

            if (SkipNextInstruction)
            {
                Skip();
                SkipNextInstruction = false;
            }
        }

        public void Skip()
        {
            ushort instruction = Mem[PC++];
            int opcode = instruction & 0xF;
            int aLoc = (instruction >> 4) & 0x3F;
            int bLoc = (instruction >> 10) & 0x3F;

            if (opcode != 0)
            {
                ReadLoc(aLoc);
                ReadLoc(bLoc);
            }
            else
            {
                ReadLoc(bLoc);
            }
        }

        private void BasicOpcode(int opcode, ref ushort a, ushort b)
        {
            switch (opcode)
            {
                case 0x1: a = b; return;
                case 0x2: Overflow(ref a, (uint)a + b); return;
                case 0x3: Overflow(ref a, (uint)a - b); return;
                case 0x4: Overflow(ref a, (uint)a * b); return;
                case 0x5:
                    if (b == 0)
                    {
                        O = a = 0;
                    }
                    else
                    {
                        a /= b;
                        O = (ushort)(((uint)a << 16) / b);
                    }
                    return;
                case 0x6:
                    a = (ushort)(b == 0 ? 0 : a % b);
                    return;
                case 0x7:
                    a <<= b;
                    O = (ushort)(((uint)a << b) >> 16);
                    return;
                case 0x8:
                    a >>= b;
                    O = (ushort)(((uint)a << 16) >> b);
                    return;
                case 0x9: a &= b; return;
                case 0xa: a |= b; return;
                case 0xb: a ^= b; return;
                case 0xc: SkipNextInstruction = !(a == b); return;
                case 0xd: SkipNextInstruction = !(a != b); return;
                case 0xe: SkipNextInstruction = !(a > b); return;
                case 0xf: SkipNextInstruction = !((a & b) != 0); return;
                default: throw new Exception("Opcode invalid");
            }
        }
        private void Overflow(ref ushort a, uint result)
        {
            a = (ushort)result;
            O = (ushort)(result >> 16);
        }

        //Watch out for side-effects with the below function.
        private delegate void RefLocDelegate(ref ushort @ref);
        private void RefLoc(int loc, RefLocDelegate action)
        {
            switch (loc)
            {
                case 0x00: action(ref A); return;
                case 0x01: action(ref B); return;
                case 0x02: action(ref C); return;
                case 0x03: action(ref X); return;
                case 0x04: action(ref Y); return;
                case 0x05: action(ref Z); return;
                case 0x06: action(ref I); return;
                case 0x07: action(ref J); return;
                case 0x08: action(ref Mem[A]); return;
                case 0x09: action(ref Mem[B]); return;
                case 0x0a: action(ref Mem[C]); return;
                case 0x0b: action(ref Mem[X]); return;
                case 0x0c: action(ref Mem[Y]); return;
                case 0x0d: action(ref Mem[Z]); return;
                case 0x0e: action(ref Mem[I]); return;
                case 0x0f: action(ref Mem[J]); return;
                case 0x10: action(ref Mem[A + Mem[PC++]]); return;
                case 0x11: action(ref Mem[B + Mem[PC++]]); return;
                case 0x12: action(ref Mem[C + Mem[PC++]]); return;
                case 0x13: action(ref Mem[X + Mem[PC++]]); return;
                case 0x14: action(ref Mem[Y + Mem[PC++]]); return;
                case 0x15: action(ref Mem[Z + Mem[PC++]]); return;
                case 0x16: action(ref Mem[I + Mem[PC++]]); return;
                case 0x17: action(ref Mem[J + Mem[PC++]]); return;
                case 0x18: action(ref Mem[SP++]); return;
                case 0x19: action(ref Mem[SP]); return;
                case 0x1a: action(ref Mem[--SP]); return;
                case 0x1b: action(ref SP); return;
                case 0x1c: action(ref PC); return;
                case 0x1d: action(ref O); return;
                case 0x1e: action(ref Mem[Mem[PC++]]); return;
                case 0x1f: action(ref Mem[PC++]); return;
                default:
                    {
                        var tmp = (ushort)(loc & 0x1F);
                        action(ref tmp);
                        return;
                    }
            }
        }
        private ushort ReadLoc(int loc)
        {
            ushort value = 0;
            RefLoc(loc, (ref ushort @ref) => value = @ref);
            return value;
        }

        private int CyclesRequiredForCurrentInstruction(ushort instruction)
        {
            int opcode = instruction & 0xF;
            int aLoc = (instruction >> 4) & 0x3F;
            int bLoc = (instruction >> 10) & 0x3F;

            int cycleCount = 0;

            //Cycles due to memory access
            if ((0x10 <= aLoc && aLoc <= 0x17) || aLoc == 0x1e || aLoc == 0x1f)
                cycleCount += 1;
            if ((0x10 <= bLoc && bLoc <= 0x17) || bLoc == 0x1e || bLoc == 0x1f)
                cycleCount += 1;

            //Cycles due to processing
            if (opcode == 0x1 || (0x9 <= opcode && opcode <= 0xb))
                cycleCount += 1;
            if ((0x2 <= opcode && opcode <= 0x4) || (0x7 <= opcode && opcode <= 0x8))
                cycleCount += 2;
            if (0x5 <= opcode && opcode <= 0x6)
                cycleCount += 3;
            if (opcode == 0)
                cycleCount += 2;

            //Cycles for IFx statements
            if (0xc <= opcode)
                cycleCount += SkipNextInstruction ? 3 : 2;

            return cycleCount;
        }

        public void PrintState()
        {
            Console.WriteLine("A {0:X4} B {1:X4} C {2:X4}", A, B, C);
            Console.WriteLine("X {0:X4} Y {1:X4} Z {2:X4}", X, Y, Z);
            Console.WriteLine("I {0:X4} J {1:X4} O {2:X4}", I, J, O);
            Console.WriteLine("PC {0:X4} SP {1:X4}", PC, SP);
        }

        public string Disassemble(ushort address)
        {
            Func<int, string> disassembleLoc = (loc) =>
            {
                const string registerNames = "ABCXYZIJ";
                if (loc <= 0x7)
                    return registerNames[loc & 0x7].ToString();
                if (loc <= 0xf)
                    return "[" + registerNames[loc & 0x7] + "]";
                if (loc <= 0x17)
                    return "[" + registerNames[loc & 0x7] + " + " + string.Format("0x{0:X}", Mem[address++]) + "]";
                if (loc <= 0x1d)
                    return new[] { "POP", "PEEK", "PUSH", "SP", "PC", "O" }[loc - 0x18];
                if (loc == 0x1e)
                    return string.Format("[0x{0:X4}]", Mem[address++]);
                if (loc == 0x1f)
                    return string.Format("0x{0:X}", Mem[address++]);
                return string.Format("0x{0:X}", loc & 0x1f);
            };

            ushort instruction = Mem[address++];
            int opcode = instruction & 0xF;
            int aLoc = (instruction >> 4) & 0x3F;
            int bLoc = (instruction >> 10) & 0x3F;

            if (opcode != 0)
            {
                var opName = new[] { null, "SET", "ADD", "SUB", "MUL", "DIV", "MOD", "SHL", "SHR", "AND", "BOR", "XOR", "IFE", "IFN", "IFG", "IFB" }[opcode];
                return opName + " " + disassembleLoc(aLoc) + ", " + disassembleLoc(bLoc);
            }
            if (aLoc == 0x01)
            {
                return "JSR " + disassembleLoc(bLoc);
            }
            return "ERR " + string.Format("0x{0:X}", aLoc) + ", " + disassembleLoc(bLoc);
        }
    }

    class AltEmulatorProxy
    {
        public static void Test()
        {
            var program = new ushort[]
        {
            /*0x7c01, 0x003b, 0x7de1, 0x1000, 0x0020, 0x7803, 0x1000, 0xc00d,
            0x7dc1, 0x001a, 0xa861, 0x7c01, 0x2000, 0x2161, 0x2000, 0x8463,
            0x806d, 0x7dc1, 0x000e, 0x9031, 0x7c10, 0x0018, 0x7dc1, 0x001a,
            0x9037, 0x61c1, 0x7dc1, 0x001a, 0x0000, 0x0000, 0x0000, 0x0000,*/

            0x7DC1, 0x0013, 0x01A1, 0x05A1,
            0x09A1, 0x19A1, 0x1DA1, 0x0DA1,
            0x11A1, 0x7DA1, 0x8000, 0x8001,
            0x8011, 0x8021, 0x8061, 0x8071,
            0x8031, 0x8041, 0x8051, 0x7DC1,
            0x0002, 0x7DC1, 0x0017, 0x6051,
            0x6041, 0x6031, 0x6071, 0x6061,
            0x6021, 0x6011, 0x6001, 0x7DC1,
            0x001F,
        };

            var cpu = new AltCPU();

            for (int i = 0; i < program.Length; i++)
                cpu.Mem[i] = program[i];

            //Run(cpu, 1000);
            RunWhilePrintingState(cpu, 50);
            //StepThroughProgram(cpu);

            Console.ReadLine();
        }

        public static void Run(AltCPU cpu, int steps)
        {
            for (int i = 0; i < steps; i++)
                cpu.Step();

            cpu.PrintState();
        }

        public static void RunWhilePrintingState(AltCPU cpu, int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                Console.WriteLine(cpu.Disassemble(cpu.PC));
                cpu.Step();
                cpu.PrintState();
            }
        }

        public static void StepThroughProgram(AltCPU cpu)
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Step");
                Console.WriteLine(cpu.Disassemble(cpu.PC));
                cpu.Step();
                cpu.PrintState();
                Console.WriteLine();
                while (true)
                {
                    Console.WriteLine("[I]:Inspect [ENTER]:Step");
                    Console.Write("> ");
                    switch (Console.ReadLine())
                    {
                        case "":
                            goto exitinputloop;
                        case "i":
                            Console.Write("Location? 0x");
                            Console.WriteLine("{0:X4}", cpu.Mem[int.Parse(Console.ReadLine(), NumberStyles.HexNumber)]);
                            break;
                        default:
                            Console.WriteLine("Invalid Input");
                            break;
                    }
                }
            exitinputloop:
                ;
            }
        }
    }
}
