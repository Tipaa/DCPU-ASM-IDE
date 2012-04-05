using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace DCPU_16
{
    public partial class Window : Form
    {
        public string project;

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
            saveFileDialog.Filter = Standard.getCombined(Standards.SourceFiles,Standards.AllFiles);
            saveFileDialog.ShowDialog();
        }

        private void SaveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            File.Create(saveFileDialog.FileName);
            new OpenFileDisplay(saveFileDialog.FileName,true).ShowDialog();
        }

        private void compiledProgram0x10cToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = Standard.getCombined(Standards.ProjectFiles,Standards.SourceFiles, Standards.CompiledFiles, Standards.AllFiles);
            saveFileDialog.ShowDialog();
        }

        private void sourceFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = Standard.getCombined(Standards.ProjectFiles, Standards.SourceFiles, Standards.CompiledFiles, Standards.AllFiles);
            openFileDialog.ShowDialog();
        }

        private void compiledProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = Standard.getCombined(Standards.ProjectFiles,Standards.SourceFiles,Standards.CompiledFiles,Standards.AllFiles);
            openFileDialog.ShowDialog();
        }

        private void OpenFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            if (Path.GetExtension(openFileDialog.FileName).Equals(Standards.ProjectFiles.raw))
            {
                openProject(openFileDialog.FileName);
            }
            else
            {
                new OpenFileDisplay(openFileDialog.FileName, false).ShowDialog();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().ShowDialog();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openProject(string s)
        {

        }

        private void dCPU16ProjectdprojToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = Standard.getCombined(Standards.ProjectFiles);
            saveFileDialog.ShowDialog();
        }

        private void projectFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = Standard.getCombined(Standards.ProjectFiles);
            openFileDialog.ShowDialog();
        }
    }
}
