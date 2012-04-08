using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Windows.Forms;

namespace DCPU_16
{
    class Assembler
    {
    }

    class Line
    {
        public string text;
        public ushort pointer;
        public Operation compiled;
        public List<ushort> literals=new List<ushort>();

        public static List<Line> lines = new List<Line>();
        public static Dictionary<ushort, string> labels = new Dictionary<ushort,string>();
        public static List<Line> deferredLines = new List<Line>();

        public static void setLabels(Dictionary<ushort, string> dict)
        {
            labels = dict;
        }

        public static string compileAll()
        {
            string bin = "";
            foreach (Line line in lines)
            {
                line.compile();
            }
            foreach (Line line in deferredLines)
            {
                line.compile();
            }
            foreach (Line line in lines)
            {
                bin+=line.compile().binary;
            }
            return bin;
        }

        public Line(string s, ushort p)
        {
            text = s.Trim();
            pointer = p;
        }

        public void register()
        {
            lines.Add(this);
        }

        public int getLength()
        {
            return 2 + literals.Capacity*2;
        }

        public ushort[] getValues()
        {
            MessageBox.Show(text+"_");
            ushort[] result = new ushort[2];
            if (Regex.Split(text, "[^a-z^A-Z^0-9]").Length < 3)
            {
                return result;
            }
            result[0] = getValue(Regex.Split(text, "[^a-z^A-Z^0-9]")[1]);
            result[1] = getValue(Regex.Split(text, "[^a-z^A-Z^0-9]")[2]);
            return result;
        }

        public ushort getValue(string input)
        {
            if (input.StartsWith("0x"))
            {
                literals.Add(Convert.ToUInt16(input,16));
                return (0x1f);
            }
            if (input.StartsWith("0d"))
            {
                literals.Add(Convert.ToUInt16(input, 10));
                return (0x1f);
            }
            if (input.StartsWith("0b"))
            {
                literals.Add(Convert.ToUInt16(input, 2));
                return (0x1f);
            }
            if (Regex.IsMatch(input, "\\[[.*+]\\]"))
            {
                return (0x1e);
            }
            #region switch
            switch (input)
            {
                    //Values
                case "POP":
                    return (0x18);
                case "PEEK":
                    return (0x19);
                case "PUSH":
                    return(0x1a);
                case "SP":
                    return(0x1b);
                case "PC":
                    return(0x1c);
                case "O":
                    return(0x1d);
                    //Opcodes
                case "SET":
                    return(0x1);
                case "ADD":
                    return(0x2);
                case "SUB":
                    return(0x3);
                case "MUL":
                    return(0x4);
                case "DIV":
                    return(0x5);
                case "MOD":
                    return(0x6);
                case "SHL":
                    return(0x7);
                case "SHR":
                    return(0x8);
                case "AND":
                    return(0x9);
                case "BOR":
                    return(0xa);
                case "XOR":
                    return(0xb);
                case "IFE":
                    return(0xc);
                case "IFN":
                    return(0xd);
                case "IFG":
                    return(0xe);
                case "IFB":
                    return(0xf);
            }
            #endregion
            foreach (ushort key in labels.Keys)
            {
                string label;
                if (labels.TryGetValue(key,out label))
                {
                    if (input == label)
                    {
                        literals.Add(key);
                        return key;
                    }
                }
            }
            //If nothing found
            deferredLines.Add(this);
            return 0x0;
        }

        public ushort getOp()
        {
            return getValue(Regex.Split(text, "[^a-z^A-Z^0-9]")[0]);
        }

        public CompiledLine compile()
        {
            ushort[] values = getValues();
            if (text.StartsWith(":"))
            {
                labels.Add((ushort)(getLength() + pointer), text.Substring(1, Math.Max(text.IndexOf(' '),1)));
                text = Regex.Replace(text,CodeStyles.regexLabels,"");
                MessageBox.Show(text);
                return compile();
            }
            if (deferredLines.Contains(this))
            {
                return new CompiledLine(0, 0, 0, literals);
            }
            return new CompiledLine(getOp(),values[0],values[1],literals);
        }
    }

    class CompiledLine
    {
        public string binary = "";

        public CompiledLine(ushort op, ushort valA, ushort valB, List<ushort> lit)
        {
            BitField opcode = new BitField(0, 4);
            opcode.Set(op);
            BitField valueA = new BitField(5, 10);
            valueA.Set(valA);
            BitField valueB = new BitField(11, 16);
            valueB.Set(valB);

            binary += (char)(opcode.Value() | valueA.Value() | valueB.Value());

            foreach (ushort u in lit)
            {
                binary += u;
            }
        }

        public void append(string input, out string output)
        {
            output = input + binary;
        }
    }

    public interface State
    {
        State pass();
    }

    struct Bittable
    {
        public void put(int bits, byte offset) { }
    }

    struct BitField
    {
        ushort mask;
        ushort value;
        char low;
        char high;

        public BitField(int low, int high)
        {
            this = new BitField((char)low, (char)high);
        }

        public BitField(char low, char high)
        {
            mask = (ushort)0xffff;
            mask &= (ushort)(0xffff << low + 16);
            mask &= (ushort)(0xffff <<high);
            value = (ushort)0;
            this.low = low;
            this.high = high;
        }

        public void Set(ushort c)
        {
            value = (ushort)(c << low);
        }

        public ushort Value()
        {
            return (ushort)(value & mask);
        }
    }

    struct OpCode
    {
        BitField value;

        public OpCode(int code)
        {
            value = new BitField((char)0, (char)4);
            value.Set((char)code);
        }

        public OpCode(char code)
        {
            value = new BitField((char)0, (char)4);
            value.Set(code);
        }

        public ushort Value()
        {
            return value.Value();
        }
    }

    struct ValueA
    {
        public BitField value;

        public ushort Value()
        {
            return value.Value();
        }

        public ValueA(ushort u)
        {
            value = new BitField((char)5, (char)10);
            value.Set(u);
        }

        public ValueA(int i)
        {
            value = new BitField((char)5, (char)10);
            value.Set((char)i);
        }
    }

    struct ValueB
    {
        public BitField value;

        public ushort Value()
        {
            return value.Value();
        }

        public ValueB(ushort u)
        {
            value = new BitField((char)11, (char)16);
            value.Set(u);
        }

        public ValueB(int i)
        {
            value = new BitField((char)11, (char)16);
            value.Set((char)i);
        }
    }

    struct Operation
    {
        public BitField op;
        public BitField valueA;
        public BitField valueB;
        public ushort[] literals;

        public Operation(ushort opcode, params ushort[] values)
        {
            opcode &= 0xf;
            op = new BitField(0,4);
            valueA = new BitField(5,10);
            valueB = new BitField(11,16);
            if (opcode == 0)
            {
                valueA.Set(opcode);
                valueB.Set(values[0]);
            }
            else
            {
                op.Set(opcode);
                if(values[0]>0x1f)
                {
                    valueA.Set(0x1f);//Literal
                } else {
                    valueA.Set((ushort)(0x20+values[0]));//Constants
                }
                if(values[1]>0x1f)
                {
                    valueB.Set(0x1f);//Literal
                } else {
                    valueB.Set((ushort)(0x20+values[1]));
                }
            }
            if (values.Length > 2)
            {
                literals = new ushort[values.Length - 2];
                for (int i = 0; i < literals.Length; i++)
                {
                    literals[i] = values[i - 2];
                }
            }
            else
            {
                literals = new ushort[0];
            }
        }

        public string write()
        {
            string result = "";
            ushort operation = (ushort)(valueB.Value() | valueA.Value() | op.Value());
            result += operation;
            foreach (ushort u in literals)
            {
                result += u;
            }
            return result;
        }
    }

    public class ClearState : State
    {
        string cleanText;
        string fileLoc;

        public ClearState(string text, string fileLocation)
        {
            fileLoc = fileLocation;
            text.ToUpper();
            foreach (string line in text.Split('\n', '\r'))
            {
                if (line.Contains(';'))
                {
                    line.Remove(line.IndexOf(';'),line.IndexOf(' '));
                }
                cleanText += line + '\n';
            }
        }

        public State pass()
        {
            return new MacroPass(cleanText,fileLoc);
        }
    }

    public class MacroPass : State
    {
        string fileLoc;
        string output;

        public MacroPass(string input, string fileLocation)
        {
            fileLoc = fileLocation;
            /*input.Replace("vram", "0x8000");
            input.Replace("start", "0x0");
            input.Replace("crash", "0x980");
            input.Replace("end", "exit");
            input.Replace("exit","0x0");*/

            MessageBox.Show(input);
            input = Macros.replaceAllMacros(input,fileLoc);

            MessageBox.Show(input);
            foreach (string line in input.Split('\n','\r'))
            {
                if (line.StartsWith(".include"))
                {
                    output += File.ReadAllText(fileLoc + line.Substring(9));
                }
                else
                {
                    output += line + '\n';
                }

                MessageBox.Show(line);
            }
        }

        public State pass()
        {

            MessageBox.Show(output);
            //return new GatherLabelPass(output,fileLoc);
            return new LineHandler(output, fileLoc);
        }
    }

    public class GatherLabelPass : State
    {
        string fileLoc;
        string output;
        Dictionary<ushort, string> labels;

        public GatherLabelPass(string input, string fileLocation)
        {
            fileLoc = fileLocation;
            ushort i = 0;
            foreach (string line in input.Split('\n', '\r'))
            {
                if (line.StartsWith(":"))
                {
                    labels.Add(i, line.Substring(1, line.IndexOf(' ')).Trim());
                }
                if (Regex.IsMatch(line, CodeStyles.regexKeywords, RegexOptions.None))
                {
                    i++;
                }
                i += (ushort)(Regex.Matches(line, CodeStyles.regexLiterals, RegexOptions.None).Count & 3);
            }
        }

        public State pass()
        {
            return new TranslatePass(output, fileLoc, labels);
        }
    }

    public class TranslatePass : State
    {
        Dictionary<ushort, string> labels;
        string fileLoc;
        string output;

        public TranslatePass(string input, string file, Dictionary<ushort, string> lab)
        {
            labels = lab;
            fileLoc = file;
            foreach (string line in input.Split('\n', '\r'))
            {
                OpCode op = new OpCode(-1);
                Operation operation = new Operation();
                line.Trim();
                int i = 0;
                foreach (string word in line.Split(' ', ','))
                {
                    if (i == 0)
                    {
                        #region SelectOpcode
                        switch (word)
                        {
                            case "SET":
                                op = new OpCode(0x1);
                                break;
                            case "ADD":
                                op = new OpCode(0x2);
                                break;
                            case "SUB":
                                op = new OpCode(0x3);
                                break;
                            case "MUL":
                                op = new OpCode(0x4);
                                break;
                            case "DIV":
                                op = new OpCode(0x5);
                                break;
                            case "MOD":
                                op = new OpCode(0x6);
                                break;
                            case "SHL":
                                op = new OpCode(0x7);
                                break;
                            case "SHR":
                                op = new OpCode(0x8);
                                break;
                            case "AND":
                                op = new OpCode(0x9);
                                break;
                            case "BOR":
                                op = new OpCode(0xa);
                                break;
                            case "XOR":
                                op = new OpCode(0xb);
                                break;
                            case "IFE":
                                op = new OpCode(0xc);
                                break;
                            case "IFN":
                                op = new OpCode(0xd);
                                break;
                            case "IFG":
                                op = new OpCode(0xe);
                                break;
                            case "IFB":
                                op = new OpCode(0xf);
                                break;
                            default:
                                continue;
                                break;
                        }
                        #endregion
                    }
                    object value;
                    if (i == 1)
                    {
                        value = new ValueA();

                        if (word.StartsWith("0x"))
                        {
                            value = new ValueA(0x1f);
                        }
                        else
                        {
                            switch (word)
                            {
                                case "POP":
                                    value = new ValueA(0x18);
                                    break;
                                case "PEEK":
                                    value = new ValueA(0x19);
                                    break;
                                case "PUSH":
                                    value = new ValueA(0x1a);
                                    break;
                                case "SP":
                                    value = new ValueA(0x1b);
                                    break;
                                case "PC":
                                    value = new ValueA(0x1c);
                                    break;
                                case "O":
                                    value = new ValueA(0x1d);
                                    break;
                            }
                            if (word.StartsWith("[") && word.EndsWith("]"))
                            {
                                //Pointer
                                value = new ValueA(0x1e);
                            }
                        }
                        i++;
                    }
                    operation.op.Set(op.Value());
                }
            }
        }

        public State pass()
        {
            return null;
        }

    }

    public class LineHandler : State
    {
        public LineHandler(string input, string fileLocation)
        {
            Line.deferredLines.Clear();
            Line.labels.Clear();
            Line.lines.Clear();
            ushort pointer = 0;
            foreach(string line in input.Split('\n','\r'))
            {
                if (line.Trim().Length < 1)
                {
                    continue;
                }
                Line l = new Line(line,pointer);
                l.register();
                pointer += (ushort)(l.getLength());
            }
            string output = Line.compileAll();
            File.WriteAllText(Path.GetFileNameWithoutExtension(fileLocation)+Standards.CompiledFiles.raw, output);
        }

        public State pass()
        {
            return this;
        }
    }

}
