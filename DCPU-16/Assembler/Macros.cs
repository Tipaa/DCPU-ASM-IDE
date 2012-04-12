using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace DCPU_16
{
    interface IMacro
    {
        string getReplace(string line, string key, string fileLocation);
    }

    class SimpleReplace : IMacro
    {
        string repl;

        public SimpleReplace(string replace)
        {
            repl = replace;
        }

        public string getReplace(string line, string key, string notUsed)
        {
            return line.Replace(key, repl);
        }
    }

    class IncludeMacro : IMacro
    {       
        public IncludeMacro()
        {
        }

        public string getReplace(string line, string key, string f)
        {
            string result = "";
            foreach (string l in line.Split('\n', '\r'))
            {
                string al = l.Trim();
                if (al.StartsWith(".include"))
                {
                    string filename = al.Substring(al.IndexOf('"')).Replace("\"","");
                    al = File.ReadAllText(Path.GetDirectoryName(f)+"\\"+filename+Standards.SourceFiles.raw);
                }
                result += al+'\n';
            }
            return result;
        }
    }

    class Macros
    {
        public static Dictionary<string, IMacro> macros = new Dictionary<string, IMacro>();
        static object _hook = hook();

        public static object hook()
        {
            macros.Add(".vram", new SimpleReplace("0x8000"));
            macros.Add(".crash", new SimpleReplace("0xff80"));
            macros.Add("BRK", new SimpleReplace(":def3a53 SET PC, def3a53"));
            macros.Add(".keyboard", new SimpleReplace("0x9000"));
            macros.Add(".include", new IncludeMacro());
            return null;
        }

        public static string replaceAllMacros(string input, string fileLoc)
        {
            string output = "";
            foreach (string line in input.Split('\n', '\r'))
            {
                string aline = line;
                foreach (string key in macros.Keys)
                {
                    IMacro str;
                    if (macros.TryGetValue(key, out str))
                    {
                        /*foreach (Match m in Regex.Matches(str, key))
                        {
                            aline = aline.Substring(0, m.Index) + key + aline.Substring(m.Index + m.Length);
                        }*/
                    }
                }
                output += aline;
                output += '\n';
            }
            return output;
        }
    }
}
