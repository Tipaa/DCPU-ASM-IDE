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
using dcpu16_ASM;

namespace DCPU_16
{
    public partial class OpenFileDisplay : Form
    {
        private CodeStyles styles = new CodeStyles();
        public readonly string fileOpen;
        public bool hasModified = false;
        public Dictionary<uint, Error> lineErrors;

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
            changeStyles();
        }

        private void highlight()
        {
            sourceCodeBox.BeginUpdate();
        }

        private void sourceCodeBox_textChanged(object sender, TextChangedEventArgs e)
        {
            changeStyles();
            setToolStrip();
            hasModified = true;
        }

        public void changeStyles()
        {
            CodeStyles.catchlabels(sourceCodeBox.Text);
            Range changedRange = sourceCodeBox.Range;
            changedRange.ClearStyle();
            changedRange.SetStyle(styles.CommentStyle, CodeStyles.regexComments);
            changedRange.SetStyle(styles.CommaStyle, CodeStyles.regexCommaSeparator);
            changedRange.SetStyle(styles.KeywordStyle, CodeStyles.regexBasicKeywords);
            changedRange.SetStyle(styles.KeywordStyle, CodeStyles.regexNonBasicKeywords);
            changedRange.SetStyle(styles.RegisterStyle, CodeStyles.regexAllRegisters);
            changedRange.SetStyle(styles.DeclaredLabelStyle, CodeStyles.regexDeclareLabels);
            changedRange.SetStyle(styles.HexPrefix, CodeStyles.regexHexPrefixes);
            changedRange.SetStyle(styles.LiteralStyle, CodeStyles.regexLiterals);            
            changedRange.SetStyle(styles.MacroStyle, CodeStyles.regexMacros);
            changedRange.SetStyle(styles.PointerStyle, CodeStyles.regexPointers);
            changedRange.SetStyle(styles.LabelStyle, CodeStyles.regexLabels);
            changedRange.SetStyle(styles.DefaultStyle, CodeStyles.regexSpace);
            changedRange.SetStyle(styles.ErrorStyle, CodeStyles.regexError);
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
            if (saveFileDialog.ShowDialog().Equals(DialogResult.OK))
            {
                saveFile();
            }
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

        #region EditMenu
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
        #endregion

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            if (keyData == (Keys.F5))
            {
                sourceCodeBox.Refresh();
                return true;
            }

            if (keyData == (Keys.F9))
            {
                compile();
                return true;
            }
            if (keyData == (Keys.Control | Keys.F))
            {
                sourceCodeBox.ShowFindDialog();
                return true;
            }
            if (keyData == (Keys.Control | Keys.Z))
            {
                sourceCodeBox.Undo();
                return true;
            }
            if (keyData == (Keys.Control | Keys.Y))
            {
                sourceCodeBox.Redo();
                return true;
            }
            if (keyData == (Keys.Control | Keys.X))
            {
                sourceCodeBox.Cut();
                return true;
            }
            if (keyData == (Keys.Control | Keys.C))
            {
                sourceCodeBox.Copy();
                return true;
            }
            if (keyData == (Keys.Control | Keys.V))
            {
                sourceCodeBox.Paste();
                return true;
            }
            if (keyData == (Keys.Control | Keys.S))
            {
                saveFile();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = Standard.getCombined(Standards.SourceFiles,Standards.CompiledFiles,Standards.AllFiles);
        }

        private void sourceCodeBox_updateText(object sender, EventArgs e)
        {
            setToolStrip();
        }

        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            compile();
        }

        private void findReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sourceCodeBox.ShowReplaceDialog();
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sourceCodeBox.ShowFindDialog();
        }

        private void compile()
        {
            string source = sourceCodeBox.Text;
            string repl = "";
            foreach (string key in Macros.macros.Keys)
            {
                if (Macros.macros.TryGetValue(key, out repl))
                {
                    source = source.Replace(key, repl);
                }
            }
            if (hasModified && MessageBox.Show("Code must be saved before compiling.", "Save code", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }
            File.WriteAllText(fileOpen, source);
            int errorline = 0;
            string errortext = "";
            if (new CDCPU16Assemble().Assemble(fileOpen, out errorline, out errortext))
            {
                MessageBox.Show("Build succeeded!");
            }
            else
            {
                MessageBox.Show("Error at line "+errorline+'\n'+errortext);
            }
            File.WriteAllText(fileOpen, sourceCodeBox.Text);
        }
    }

    public class Error
    {
        string description;

        public Error(string s)
        {
            description = s;
        }

        public string getError()
        {
            return description;
        }
    }

}
