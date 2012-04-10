using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DCPU_16
{
    public partial class EmulatorWindow : Form
    {
        public Cpu cpu;
        public string file = "";
        public string binary = "";

        public EmulatorWindow()
        {
            InitializeComponent();
            cpu = new Cpu(new ushort[1000]);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Console.WriteLine(Convert.ToUInt32(cpuMemSize.Text, 16));
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
            dumpMemory();
            dumpStack();
        }

        private void dumpMemory()
        {
            memDump.Text = "";
            uint i = 0;
            foreach (ushort u in cpu.Memory)
            {
                i++;
                memDump.Text += String.Format("{0:X4} ", u);
                Console.WriteLine(String.Format("{0:X4} ", i));
            }
        }

        private void dumpStack()
        {
            stackTextBox.Text = "";
            for (uint u = cpu.SP; u < cpu.Memory.Length; u++)
            {
                stackTextBox.Text += cpu.Memory[u] + '\n';
            }
        }

        private ushort[] loadMemory()
        {
            ushort[] result = new ushort[cpu.Memory.Length];
            for (int i = 0; i < cpu.Memory.Length; i++)
            {
                result[i] = Convert.ToUInt16(memDump.Text.Substring((i*5), (i*5)).Trim(), 16);
                memDump.Text.Remove(0, 5);
            }
            return result;
        }

        private void tick()
        {
            if (cpu == null)
            {
                btnReset_Click(null, null);
                return;
            }
            //dumpMemory();

            cpu.SP = (ushort)Convert.ToUInt32(numericRegisterSP.Value.ToString(), 16);
            cpu.Overflow = (ushort)Convert.ToUInt32(numericRegisterO.Value.ToString(), 16);
            cpu.PC = (ushort)Convert.ToUInt32(numericRegisterPC.Value.ToString(), 16);
            cpu.Registers[0] = (ushort)Convert.ToUInt32(numericRegisterA.Value.ToString(), 16);
            cpu.Registers[1] = (ushort)Convert.ToUInt32(numericRegisterB.Value.ToString(), 16);
            cpu.Registers[2] = (ushort)Convert.ToUInt32(numericRegisterC.Value.ToString(), 16);
            cpu.Registers[3] = (ushort)Convert.ToUInt32(numericRegisterX.Value.ToString(), 16);
            cpu.Registers[4] = (ushort)Convert.ToUInt32(numericRegisterY.Value.ToString(), 16);
            cpu.Registers[5] = (ushort)Convert.ToUInt32(numericRegisterZ.Value.ToString(), 16);
            cpu.Registers[6] = (ushort)Convert.ToUInt32(numericRegisterI.Value.ToString(), 16);
            cpu.Registers[7] = (ushort)Convert.ToUInt32(numericRegisterJ.Value.ToString(), 16);

            cpu.Tick();

            numericRegisterA.Value = cpu.Registers[0];
            numericRegisterB.Value = cpu.Registers[1];
            numericRegisterC.Value = cpu.Registers[2];
            numericRegisterX.Value = cpu.Registers[3];
            numericRegisterY.Value = cpu.Registers[4];
            numericRegisterZ.Value = cpu.Registers[5];
            numericRegisterI.Value = cpu.Registers[6];
            numericRegisterJ.Value = cpu.Registers[7];
            numericRegisterSP.Value = cpu.SP;
            numericRegisterO.Value = cpu.Overflow;
            numericRegisterPC.Value = cpu.PC;
        }

        private void loadBinary(string fileName)
        {
            binary = File.ReadAllText(fileName);
            for (int i = 0; i < binary.Length/2; i++)
            {
                cpu.Memory[i] = (ushort)binary[i*2+1];
            }
        }

        private void btnStep_Click(object sender, EventArgs e)
        {
            if (cpu == null || cpu.Memory.Length != (ushort)Convert.ToUInt32(cpuMemSize.Text, 16))
            {
                btnReset_Click(null, null);
            }
            tick();
            updateView();
        }

        private void btnRunSteps_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Convert.ToUInt32(stepsToRunBox.Text, 16); i++)
            {
                tick();
            }
            updateView();
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

        private void updateView()
        {

            dumpMemory();
            dumpStack();
        }
    }
}
