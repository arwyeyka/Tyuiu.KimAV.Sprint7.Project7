using System;
using System.Windows.Forms;

namespace Tyuiu.KAV.Sprint7.Project.V7
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain_KAV());
        }
    }
}
