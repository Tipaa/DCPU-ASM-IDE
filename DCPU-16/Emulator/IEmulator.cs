using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCPU_16.Emulator
{
    public interface IEmulator
    {
        void LoadProgram(string binary);

        void LoadProgram(ushort[] binary);

        void StepProgram(uint steps);

        void Reset();

        EmulatorState ProvideState();

        void ReceiveState(EmulatorState e);

        string Disassemble(ushort address);
    }

    public class EmulatorState
    {
        public ushort[] Memory;
        public ushort[] Registers;
        public ushort[] Stack;
        public ushort SP;
        public ushort PC;
        public ushort O;

        public EmulatorState(ref ushort[] Mem, ref ushort[] Reg, ref ushort[] Stck, ref ushort StackPointer, ref ushort ProgramCounter, ref ushort Overflow)
        {
            Memory = Mem;
            Registers = Reg;
            Stack = Stck;
            SP = StackPointer;
            PC = ProgramCounter;
            O = Overflow;
        }

        public enum registers : uint
        {
            A = 0x0,
            B = 0x1,
            C = 0x2,
            X = 0x3,
            Y = 0x4,
            Z = 0x5,
            I = 0x6,
            J = 0x7
        }

    }
}
