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
    public partial class Window : Form
    {
        public Window()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void sourceFiledcpuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "DCPU Assembler files (*.dcpu)|*.dcpu|Compiled DCPU files (*.0x10c)|*.0x10c|All Files (*.*)|*.*";
            saveFileDialog.ShowDialog();
        }

        private void SaveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            File.Create(saveFileDialog.FileName);
            new OpenFileDisplay(saveFileDialog.FileName,true).ShowDialog();
        }

        private void compiledProgram0x10cToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Compiled DCPU files (*.0x10c)|*.0x10c|DCPU Assembler files (*.dcpu)|*.dcpu|All Files (*.*)|*.*";
            saveFileDialog.ShowDialog();
        }

        private void sourceFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "DCPU Assembler files (*.dcpu)|*.dcpu|All Files (*.*)|*.*";
            openFileDialog.ShowDialog();
        }

        private void compiledProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "Compiled DCPU files (*.0x10c)|*.0x10c|All Files (*.*)|*.*";
            openFileDialog.ShowDialog();
        }

        private void OpenFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            new OpenFileDisplay(openFileDialog.FileName,false).ShowDialog();
        }
    }
}
