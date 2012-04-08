using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace DCPU_16
{
    class AssemblerWrapper
    {


        [DllImport("assembler.dll", EntryPoint = "assemble")]

        static extern void assemble(string filename, string fileout);

        public static void Assemble(string filename)
        {
            string fileout = Path.GetFileNameWithoutExtension(filename) + ".dcpu";
            assemble(filename, fileout);
        }


    }
}
