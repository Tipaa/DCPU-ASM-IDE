using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DCPU_16
{
    class Macros
    {
        public static Dictionary<string, string> macros = new Dictionary<string, string>();
        static object _hook = hook();

        public static object hook()
        {
            macros.Add(".vram", "0x8000");
            macros.Add(".crash", "0xff80");
            macros.Add("BRK", "SET PC, 0x0000");
            macros.Add(".keyboard", "0x9000");

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
                    string str;
                    if (macros.TryGetValue(key, out str))
                    {
                        foreach (Match m in Regex.Matches(str, key))
                        {
                            aline = aline.Substring(0, m.Index) + key + aline.Substring(m.Index + m.Length);
                        }
                    }
                }
                output += aline;
                output += '\n';
            }
            return output;
        }
    }
}
