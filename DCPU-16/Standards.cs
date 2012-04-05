using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCPU_16
{

    class Standard
    {
        public Standard(string ext, string desc)
        {
            extension = ext;
            description = desc;
        }

        public string extension;
        public string description;

        public string getFormatted()
        {
            return description+"|"+extension;
        }

        public static string getCombined(params Standard[] standards)
        {
            string output = "";
            for(int i = 0; i < standards.Length-1; i++)
            {
                output += standards[i].getFormatted() + "|";
            }
            output += standards[standards.Length - 1].getFormatted();
            return output;
        }
    }

    class Standards
    {
        public static readonly Standard SourceFiles = new Standard("*.dasm","DCPU Assembler Source");
        public static readonly Standard CompiledFiles = new Standard("*.dcpu", "DCPU Assembled Program");
        public static readonly Standard AllFiles = new Standard("*.*", "All Files");
    }
}
