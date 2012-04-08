using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FastColoredTextBoxNS;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace DCPU_16
{
    partial class CodeStyles
    {
        public Style KeywordStyle = new TextStyle(Brushes.Blue,null,FontStyle.Bold);
        public Style RegisterStyle = new TextStyle(Brushes.DarkTurquoise, null, FontStyle.Bold);
        public Style LiteralStyle = new TextStyle(Brushes.RoyalBlue, null, FontStyle.Italic);
        public Style HexPrefix = new TextStyle(Brushes.Blue, null, FontStyle.Strikeout|FontStyle.Italic);
        public Style DeclaredLabelStyle = new TextStyle(Brushes.Coral, null, FontStyle.Underline);
        public Style CommentStyle = new TextStyle(Brushes.LimeGreen, null, FontStyle.Regular);
        public Style MacroStyle = new TextStyle(Brushes.Navy, null, FontStyle.Italic|FontStyle.Bold);
        public Style PointerStyle = new TextStyle(Brushes.Plum, null, FontStyle.Bold);
        public Style LabelStyle = new TextStyle(Brushes.Orange, null, FontStyle.Regular);

        public static string regexKeywords = "(SET|ADD|SUB|MUL|DIV|MOD|SHL|SHR|AND|BOR|XOR|IFE|IFN|IFG|IFB|DAT|JSR) ";
        public static string regexRegisters = " (A|B|C|I|J|X|Y|Z|SP|PC|O|POP|PUSH|PEEK),? ?";
        public static string regexLiterals = "(((0x|0d|0o|0b)[0-9a-fA-F]{0,8},?)|DAT (([^\\d]\\w)\\w+),? (((0x|0d|0o|0b)[0-9a-fA-F]{1,8})|(\"(.*)\")))";
        public static string regexHexPrefixes = "(0x|0d|0o|0b)";
        public static string regexDeclareLabels = "(^|\\r|\\n):((\\S)*)";
        public static string regexLabels = "\\[.+\\],?";
        public static string regexComments = ";(.*)?";
        public static string regexMacros = "(vram|crash|end|exit)";
        public static string regexPointers = "\\[.*\\]";

        public static void catchlabels(String input)
        {
            regexLabels = "(";
            foreach (Match m in Regex.Matches(input, regexDeclareLabels))
            {
                regexLabels += m.Value.Trim().Substring(1) + "|";
            }
            regexLabels.Remove(regexLabels.Length - 2);
            regexLabels += ")";
        }

   }

}
