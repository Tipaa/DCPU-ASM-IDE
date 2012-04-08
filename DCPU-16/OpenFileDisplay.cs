using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using FastColoredTextBoxNS;
using System.Text.RegularExpressions;

namespace DCPU_16
{
    public partial class OpenFileDisplay : Form
    {
        private CodeStyles styles = new CodeStyles();
        public readonly string fileOpen;
        public bool hasModified = false;

        public OpenFileDisplay(String s,bool isNew)
        {
            fileOpen = s;
            InitializeComponent();
            this.Text += s;         
            if (!isNew)
            {
                LoadFile(s);
            }
            this.hasModified = false;
            setToolStrip();
            sourceCodeBox.SelectionChanged += new EventHandler(this.sourceCodeBox_updateText);
            openFileDialog.Filter = Standard.getCombined(Standards.SourceFiles);
        }

        private void OpenFileDisplay_Load(object sender, EventArgs e)
        {
        }

        private void LoadFile(String filename)
        {
            sourceCodeBox.Text = File.ReadAllText(filename);
            Range all = new Range(sourceCodeBox);
            all.Start = new Place(0, 0);
            all.End = new Place(sourceCodeBox.Lines.Last().Length, sourceCodeBox.LinesCount);
            changeStyles(new Range(sourceCodeBox));
        }

        private void highlight()
        {
            sourceCodeBox.BeginUpdate();
        }

        private void sourceCodeBox_textChanged(object sender, TextChangedEventArgs e)
        {
            changeStyles(e.ChangedRange);
            setToolStrip();
            hasModified = true;
        }

        public void changeStyles(Range changedRange)
        {
            changedRange.ClearStyle();
            CodeStyles.catchlabels(sourceCodeBox.Text);
            changedRange.SetStyle(styles.CommentStyle, CodeStyles.regexComments);
            changedRange.SetStyle(styles.KeywordStyle, CodeStyles.regexKeywords);
            changedRange.SetStyle(styles.RegisterStyle, CodeStyles.regexRegisters);
            changedRange.SetStyle(styles.HexPrefix, CodeStyles.regexHexPrefixes);
            changedRange.SetStyle(styles.LiteralStyle, CodeStyles.regexLiterals);
            changedRange.SetStyle(styles.DeclaredLabelStyle, CodeStyles.regexDeclareLabels);
            changedRange.SetStyle(styles.LabelStyle, CodeStyles.regexLabels);                       
            changedRange.SetStyle(styles.MacroStyle, CodeStyles.regexMacros);
            changedRange.SetStyle(styles.PointerStyle, CodeStyles.regexPointers);
        }

        public void setToolStrip()
        {
            if (hasModified)
            {
                this.Text = "Edit File: " + fileOpen + " *";
            }
            else
            {
                this.Text = "Edit File: " + fileOpen;
            }
            this.toolStripFileType.Text = "Source file";
            this.toolStripFileLength.Text = sourceCodeBox.LinesCount + " Lines of code";
            this.toolStripCurserPosition.Text = "Ln: " + sourceCodeBox.Selection.Start.iLine + " Col: " + sourceCodeBox.Selection.Start.iChar;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog();
        }

        private void saveFile()
        {
            this.hasModified = false;
            setToolStrip();
            File.WriteAllText(fileOpen, sourceCodeBox.Text);
        }

        private void saveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            saveFile();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sourceCodeBox.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sourceCodeBox.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sourceCodeBox.Paste();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sourceCodeBox.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sourceCodeBox.Redo();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sourceCodeBox.Refresh();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = Standard.getCombined(Standards.SourceFiles,Standards.CompiledFiles,Standards.AllFiles);
        }

        private void sourceCodeBox_updateText(object sender, EventArgs e)
        {
            setToolStrip();
        }

        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ClearState(sourceCodeBox.Text, this.fileOpen).pass().pass().pass().pass();
        }
    }
}
