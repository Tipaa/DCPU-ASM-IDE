using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace DCPU_16
{

    class Parser
    {
        public static void parse(string text, string pathToWriteTo)
        {

#region RemoveComments
            foreach (Match match in Regex.Matches(text, new CodeStyles().regexComments, RegexOptions.None))
            {
                text = text.Replace(match.Value,"");
            }
#endregion

#region MapLabels
            foreach (Match match in Regex.Matches(text, new CodeStyles().regexDeclareLabels, RegexOptions.None))
            {
                text = text.Replace(match.Value, "");
                new Label(0, match.Value);
            }
#endregion

#region ReplaceLabels
            foreach (Match match in Regex.Matches(text, new CodeStyles().regexLabels, RegexOptions.None))
            {
                if (match.Value.Length < 1)
                {
                    continue;
                }
                MessageBox.Show(match.Value);
                text = text.Replace(match.Value, Label.searchByName(match.Value).pointer+"");
            }
#endregion
            string[] lines = text.Split(new string[] { "\n" }, 8096, StringSplitOptions.None);
            foreach (string line in lines)
            {
            }
        }
    }

    struct Label
    {
        public readonly ushort pointer;
        public readonly string name;
        
        public static List<Label> labels = new List<Label>();

        public Label(ushort pointer, string name)
        {
            this.pointer = pointer;
            this.name = name;
            labels.Add(this);
        }

        private Label(string s)
        {
            MessageBox.Show("Unfound Label: "+s);
            pointer = 0xffff;
            name = "";
        }

        private Label(int i)
        {
            this = new Label("Pointer of" + i);
        }

        public static Label searchByName(string name)
        {
            foreach (Label label in labels)
            {
                if (label.name == name)
                {
                    return label;
                }
            }
            return new Label(name);
        }

        public static Label searchByPointer(ushort pointer)
        {
            foreach (Label label in labels)
            {
                if (label.pointer == pointer)
                {
                    return label;
                }
            }
            return new Label(pointer);
        }

    }

    struct CompiledByte
    {
        public CompiledByte(OpCode op, Value a, Value b)
        {
            ushort[] output = compile(op,a,b);
            for (int i = 0; i < output.Length; i++)
            {
                ;
            }
        }

        ushort[] compile(OpCode op, Value a, Value b)
        {
            ushort result = 0;
            result |= op.binary;
            bool litA = false;
            bool litB = false;
            if (a.binary < 0x20)
            {
                result |= (ushort)(a.binary << 4);
            }
            else
            {
                result |= (ushort)Values.RD_LITERAL << 4;
                litA = true;
            }
            if (b.binary < 0x20)
            {
                result |= (ushort)(b.binary << 10);
            }
            else
            {
                result |= (ushort)Values.RD_LITERAL << 10;
                litB = true;
            }
            ushort[] output = new ushort[1 + (litA ? 1 : 0) + (litB ? 1 : 0)];
            output[0] = result;
            if (litA)
            {
                output[1] = a.binary;
            } else if (!litA && litB)
            {
                output[1] = b.binary;
            }
            else if (litA && litB)
            {
                output[2] = b.binary;
            }

            return output;
        }
    }

    struct OpCode
    {
        public OpCode(ushort u)
        {
            binary = u;
        }

        public ushort binary;
    }

    struct Value
    {
        public Value(ushort u)
        {
            binary = u;
        }

        public ushort binary;
    }

    struct Literal
    {
        public Literal(ushort u)
        {
            binary = 0x1f;
            literal = u;
        }

        public ushort binary;
        public ushort literal;
    }

    enum OpCodes : ushort
    {
        NUL,
        SET,
        ADD, SUB, MUL, DIV,
        MOD,
        SHL, SHR,
        AND, BOR, XOR,
        IFE, IFN, IFG, IFB
    }

    enum Values : ushort
    {
        //Read Register
        A,B,C,X,Y,Z,I,J,

        //Read [Register]
        RDA,RDB,RDC,RDX,RDY,RDZ,RDI,RDJ,

        //Read [Register+literal]
        OFF_RDA,OFF_RDB,OFF_RDC,OFF_RDX,
        OFF_RDY,OFF_RDZ,OFF_RDI,OFF_RDJ,

        //Read from stack
        POP,PEEK,PUSH,

        //Read [Stack Pointer]
        SP,

        //Read [Program Counter]
        PC,

        //Read Overflow
        O,

        //Read from [Literal Pointer]
        RD_LITERAL_POINTER,

        //Read Literal
        RD_LITERAL,

        //Read Literal < 0x20
        LIT

    }

}
