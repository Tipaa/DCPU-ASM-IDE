using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DCPU_16.Emulator
{
    public partial class EmulatorWindow : Form
    {
        
        public IEmulator cpu;
        public string file = "";
        public string binary = "";

        public ushort[] mem;
        public ushort[] reg;
        public ushort sp;
        public ushort pc;
        public ushort o;

        public EmulatorWindow()
        {
            InitializeComponent();
            //cpu = new Cpu(new ushort[0x1000]);
            cpu = new AltCPU();
            AltEmulatorProxy.Test();           
            this.memDump.ReadOnly = false;
            mem = new ushort[0x10000];
        }

        public EmulatorState ProvideState()
        {
            mem = getValues(memDump.Text);
            reg = getRegisters();
            return new EmulatorState(ref mem, ref reg, ref mem, ref sp, ref pc, ref o);
        }

        public ushort[] getValues(string s)
        {
            sp = (ushort)Convert.ToUInt32(numericRegisterSP.Value.ToString(), 16);
            pc = (ushort)Convert.ToUInt32(numericRegisterPC.Value.ToString(), 16);
            o = (ushort)Convert.ToUInt32(numericRegisterO.Value.ToString(), 16);

            string[] s1 = s.Split(' ', '\n');
            for (int i = 0; i < mem.Length; i++)
            {
                if (i < s1.Length)
                {
                    if (s1[i].Trim().Length < 1)
                    {
                        continue;
                    }
                    mem[i] = ((ushort)Convert.ToUInt32(s1[i].ToLower().Trim(), 16));
                }
            }
            
            /*List<ushort> result = new List<ushort>();
            foreach (string s1 in s.Split(' ', '\n'))
            {
                Console.WriteLine(s1);
                if (s1.Trim().Length < 1)
                {
                    continue;
                }
                result.Add((ushort)Convert.ToUInt32(s1.ToLower().Trim(), 16));
            }
            return result.ToArray();*/

            return mem;
        }

        public ushort[] getRegisters()
        {
            List<ushort> result = new List<ushort>();

            result.Add((ushort)Convert.ToUInt32(numericRegisterA.Value.ToString(), 10));
            result.Add((ushort)Convert.ToUInt32(numericRegisterB.Value.ToString(), 10));
            result.Add((ushort)Convert.ToUInt32(numericRegisterC.Value.ToString(), 10));
            result.Add((ushort)Convert.ToUInt32(numericRegisterX.Value.ToString(), 10));
            result.Add((ushort)Convert.ToUInt32(numericRegisterY.Value.ToString(), 10));
            result.Add((ushort)Convert.ToUInt32(numericRegisterZ.Value.ToString(), 10));
            result.Add((ushort)Convert.ToUInt32(numericRegisterI.Value.ToString(), 10));
            result.Add((ushort)Convert.ToUInt32(numericRegisterJ.Value.ToString(), 10));
            
            return result.ToArray();
        }

        private void Handle(EmulatorState state)
        {
            dumpMemory(state);
            //dumpStack(state);

            numericRegisterA.Value = state.Registers[0];
            numericRegisterB.Value = state.Registers[1];
            numericRegisterC.Value = state.Registers[2];
            numericRegisterI.Value = state.Registers[6];
            numericRegisterJ.Value = state.Registers[7];
            numericRegisterX.Value = state.Registers[3];
            numericRegisterY.Value = state.Registers[4];
            numericRegisterZ.Value = state.Registers[5];

            numericRegisterSP.Value = state.SP;
            numericRegisterPC.Value = state.PC;
            numericRegisterO.Value = state.O;

            setToolTip();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            cpu.Reset();
            if (file == "")
            {
                btnLoad_Click(null, null);
            }
            cpu.LoadProgram(File.ReadAllText(file));
            Handle(cpu.ProvideState());
            /*Console.WriteLine(Convert.ToUInt32(cpuMemSize.Text, 16));
            cpu = new Cpu(new ushort[Convert.ToUInt32(cpuMemSize.Text,16)]);
            numericRegisterA.Value = 0;
            numericRegisterB.Value = 0;
            numericRegisterC.Value = 0;
            numericRegisterI.Value = 0;
            numericRegisterJ.Value = 0;
            numericRegisterX.Value = 0;
            numericRegisterY.Value = 0;
            numericRegisterZ.Value = 0;
            numericRegisterSP.Value = 0;
            numericRegisterPC.Value = 0;
            numericRegisterO.Value = 0;
            loadBinary(file);
            //cpu.Memory = loadMemory();
            dumpMemory(cpu.ProvideState());
            dumpStack(cpu.ProvideState());*/
        }

        private void dumpMemory(EmulatorState state)
        {
            memDump.Text = "";
            uint i = 0;
            foreach (ushort u in state.Memory)
            {
                if (i++ > Convert.ToUInt32(cpuMemSize.Text, 16))
                {
                    break;
                }
                memDump.Text += String.Format("{0:X4} ", u);
            }
        }

        private void dumpStack(EmulatorState state)
        {
            stackTextBox.Text = "";
            for (uint u = state.SP; u < state.Memory.Length; u++)
            {
                stackTextBox.Text += state.Memory[u] + '\n';
            }
        }

        /*private ushort[] loadMemory()
        {
            ushort[] result = new ushort[cpu.Memory.Length];
            for (int i = 0; i < cpu.Memory.Length; i++)
            {
                result[i] = Convert.ToUInt16(memDump.Text.Substring((i*5), (i*5)).Trim(), 16);
                memDump.Text.Remove(0, 5);
            }
            return result;
        }*/

        private void tick(uint i)
        {
            if (cpu == null)
            {
                btnReset_Click(null, null);
                return;
            }

            cpu.ReceiveState(ProvideState());

            cpu.StepProgram(i);

            Handle(cpu.ProvideState());
        }

        private void loadBinary(string fileName)
        {            
            cpu.LoadProgram(File.ReadAllText(fileName));
        }

        private void btnStep_Click(object sender, EventArgs e)
        {
            if (cpu == null)
            {
                btnReset_Click(null, null);
            }
            tick(1);
        }

        private void btnRunSteps_Click(object sender, EventArgs e)
        {
            tick(Convert.ToUInt32(stepsToRunBox.Text, 16));
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = Standard.getCombined(Standards.CompiledFiles,Standards.AllFiles);
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                file = openFileDialog.FileName;
                loadBinary(openFileDialog.FileName);
            }
        }

        private void updateView(EmulatorState state)
        {
            dumpMemory(state);
            dumpStack(state);

        }

        private void setToolTip()
        {
            int i = (Convert.ToInt32(numericRegisterPC.Value));
            string s = memDump.Text.Substring(i*5, 4);
            disassemblerTip.SetToolTip(this.memDump,
                "PC: " + String.Format("{0:X4}", Convert.ToUInt16(numericRegisterPC.Value)) + '\n' +
                "Hex: " + s + '\n' +
                i + "\n" +
                "Disassembled: " + cpu.Disassemble((ushort)i));
        }

        public static ushort[] binFromFile(string filename)
        {
            string bin = File.ReadAllText(filename);
            ushort[] array = new ushort[bin.Length];
            int i = 0;
            foreach (char c in bin)
            {
                array[i] = (ushort)c;
                i++;
            }
            return array;
        }

        private void disassemblerTip_Popup(object sender, PopupEventArgs e)
        {

        }

        private void numericRegisterPC_ValueChanged(object sender, EventArgs e)
        {
            setToolTip();
        }
    }
}
