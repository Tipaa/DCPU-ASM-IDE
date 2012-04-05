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

        public OpenFileDisplay(String s,bool isNew)
        {
            InitializeComponent();
            this.Text += s;
            if (!isNew)
            {
                LoadFile(s);
            }
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
        }

        public void changeStyles(Range changedRange)
        {
            changedRange.ClearStyle();
            changedRange.SetStyle(styles.KeywordStyle, styles.regexKeywords);
            changedRange.SetStyle(styles.RegisterStyle, styles.regexRegisters);
            changedRange.SetStyle(styles.LiteralStyle, styles.regexLiterals);
            changedRange.SetStyle(styles.LabelStyle, styles.regexLabels);
            changedRange.SetStyle(styles.HexPrefix, styles.regexHexPrefixes);
            changedRange.SetStyle(styles.CommentStyle, styles.regexComments);
            changedRange.SetStyle(styles.MacroStyle, styles.regexMacros);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            File.WriteAllText(this.Text.Substring(11), sourceCodeBox.Text);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog();
        }

        private void saveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            File.WriteAllText(this.Text.Substring(11), sourceCodeBox.Text);
        }
    }
}
