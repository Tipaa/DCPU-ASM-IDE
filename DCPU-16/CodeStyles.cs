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
        public Style DefaultStyle = new TextStyle(Brushes.Black, null, FontStyle.Regular);
        public Style KeywordStyle = new TextStyle(Brushes.Blue, null, FontStyle.Bold);
        public Style RegisterStyle = new TextStyle(Brushes.DarkTurquoise, null, FontStyle.Bold);
        public Style LiteralStyle = new TextStyle(Brushes.Blue, null, FontStyle.Italic);
        public Style HexPrefix = new TextStyle(Brushes.LightBlue, null, FontStyle.Strikeout | FontStyle.Italic);
        public Style DeclaredLabelStyle = new TextStyle(Brushes.Coral, null, FontStyle.Underline);
        public Style CommentStyle = new TextStyle(Brushes.LimeGreen, null, FontStyle.Regular);
        public Style MacroStyle = new TextStyle(Brushes.Navy, null, FontStyle.Italic | FontStyle.Bold);
        public Style PointerStyle = new TextStyle(Brushes.Plum, null, FontStyle.Bold);
        public Style LabelStyle = new TextStyle(Brushes.Orange, null, FontStyle.Regular);
        public Style CommaStyle = new TextStyle(Brushes.Navy, null, FontStyle.Regular);
        public Style ErrorStyle = new TextStyle(Brushes.Red, null, FontStyle.Bold | FontStyle.Underline);

        #region regexPunctuation
        public static string regexCommaSeparator = "([\\s]*,[\\s]*)";
        public static string regexSpace = "([\\s]+)";
        public static string regexSpaceOptional = "([\\s]*)";
        public static string regexAnySeparator = "(\\W+)";
        public static string regexSpaceSeparator = "([^,\\s]+)";
        public static string regexEndLine = "($|\\n|\\r|;)";
        public static string regexStartLine = "^()";
        public static string regexMaths = "(\\+|\\-|\\*|\\/)";
        public static string regexValidName = "(([^\\d]\\w)\\w*)";

        #endregion

        #region regexLang
        public static string regexBasicKeywords = "(SET|ADD|SUB|MUL|DIV|MOD|SHL|SHR|AND|BOR|XOR|IFE|IFN|IFG|IFB)";
        public static string regexNonBasicKeywords = "(JSR|DAT|BRK) ";
        public static string regexAllRegisters = "(A|B|C|I|J|X|Y|Z|SP|PC|O|POP|PUSH|PEEK)";
        public static string regexHexPrefixes = "(0x|0d|0o|0b)";
        public static string regexComments = ";(.*)?";                      //Anything behind a ;
        public static string regexMacros = "\\.(vram|crash|end|exit|include)";  //All the current macros
        public static string regexError = ".*";                             //All other code

        public static string regexLiterals = "((" + regexHexPrefixes + "[0-9a-fA-F]{1,8},?)|DAT " + regexValidName + ",? ((" + regexHexPrefixes + "[0-9a-fA-F]{1,8})|(\"(.*)\"))|(\"\\w+\"))";
        public static string regexCanPoint = "((" + regexAllRegisters + ")|(" + regexLiterals + ")|(" + regexMacros + ")|(" + regexMaths + "))";
        public static string regexDeclareLabels = "(^|\\r|\\n):" + regexValidName;    //:<name>
        public static string regexLabels = regexCanPoint + ",?";              //A label name
        public static string regexPointers = "\\[" + regexCanPoint + "\\]";     //Any valid pointers
        #endregion

        public static string regexCompoundValue = "((" + regexAllRegisters + ")|(" + regexPointers + ")|(" + regexLiterals + ")|(" + regexMacros + ")|(" + regexLabels + "))";
        public static string regexModifiable = "((" + regexAllRegisters + ")|(" + regexPointers + "))";
        public static string regexValuePrefix = "((" + regexCommaSeparator + regexSpaceOptional + ")|((" + regexBasicKeywords + ")|(" + regexNonBasicKeywords + "))(" + regexSpaceOptional + "))";

        public static string syntaxRegisters = regexAnySeparator + regexAllRegisters + regexAnySeparator;//"(,| )+(A|B|C|I|J|X|Y|Z|SP|PC|O|POP|PUSH|PEEK)(\\s+)?(,|\\n|;)";
        public static string syntaxBasicKeywords = regexAnySeparator + regexSpaceOptional + regexBasicKeywords + regexSpaceSeparator + regexCompoundValue + regexCommaSeparator + regexCompoundValue + regexEndLine;
        public static string syntaxNonBasicKeywords = regexAnySeparator + regexSpaceOptional + regexNonBasicKeywords + regexSpaceSeparator + regexCompoundValue + regexEndLine;
        public static string syntaxDat = "(DAT)" + regexSpace + regexValidName + regexCommaSeparator + regexLiterals + regexEndLine;
        public static string syntaxLabel = regexLabels;

        public static void catchlabels(String input)
        {
            regexLabels = regexAnySeparator + "(";
            foreach (Match m in Regex.Matches(input, regexDeclareLabels))
            {
                regexLabels += m.Value.Trim().Substring(1) + "|";
            }
            regexLabels.Remove(regexLabels.Length - 2);
            regexLabels += ")" + regexAnySeparator;
        }

    }

    class InsertSpaceSnippet : AutocompleteItem
    {
        string pattern;

        public InsertSpaceSnippet(string pattern)
            : base("")
        {
            this.pattern = pattern;
        }

        public InsertSpaceSnippet()
            : this("(" + CodeStyles.regexBasicKeywords + ")(\\w+,)(\\w+)")
        {
        }

        public override CompareResult Compare(string fragmentText)
        {
            if (Regex.IsMatch(fragmentText, pattern))
            {
                Text = InsertSpaces(fragmentText);
                if (Text != fragmentText)
                    return CompareResult.Visible;
            }
            return CompareResult.Hidden;
        }

        public string InsertSpaces(string fragment)
        {
            var m = Regex.Match(fragment, pattern);
            if (m == null)
                return fragment;
            if (m.Groups[1].Value == "" && m.Groups[3].Value == "")
                return fragment;
            return (m.Groups[1].Value + " " + m.Groups[2].Value + " " + m.Groups[3].Value).Trim();
        }

        public override string ToolTipTitle
        {
            get
            {
                return Text;
            }
        }
    }
}
