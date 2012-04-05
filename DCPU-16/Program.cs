using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DCPU_16
{
    partial class Window
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Window());
        }
    }
}
