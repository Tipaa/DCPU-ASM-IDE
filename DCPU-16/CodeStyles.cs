using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FastColoredTextBoxNS;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace DCPU_16
{
    partial class CodeStyles
    {
        public Style KeywordStyle = new TextStyle(Brushes.Blue,null,FontStyle.Bold);
        public Style RegisterStyle = new TextStyle(Brushes.DarkTurquoise, null, FontStyle.Bold);
        public Style LiteralStyle = new TextStyle(Brushes.LimeGreen, null, FontStyle.Italic);
        public Style HexPrefix = new TextStyle(Brushes.Lime, null, FontStyle.Strikeout);
        public Style LabelStyle = new TextStyle(Brushes.Coral, null, FontStyle.Underline);
        public Style CommentStyle = new TextStyle(Brushes.LimeGreen, null, FontStyle.Regular);
        public Style MacroStyle = new TextStyle(Brushes.Navy, null, FontStyle.Italic|FontStyle.Bold);

        public string regexKeywords = "(SET|ADD|SUB|MUL|DIV|MOD|SHL|SHR|AND|BOR|XOR|IFE|IFN|IFG|IFB|DAT) ";
        public string regexRegisters = " (A|B|C|I|J|X|Y|Z|SP|PC|O|POP|PUSH|PEEK), ?";
        public string regexLiterals = "(0x|0d|0o|0b)[0-9a-fA-F]{0,4},?";
        public string regexHexPrefixes = "0x";
        public string regexLabels = "^:[^ .]+,?|\\[[a-zA-Z0-9]\\],?";
        public string regexComments = ";.*";
        public string regexMacros = "\\[(vram|start|crash|end|exit)\\]";

   }

}
