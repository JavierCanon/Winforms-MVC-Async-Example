using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinformsMVCAsyncExampleNetStandard
{
    static class Program
    {

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppDomain.CurrentDomain.FirstChanceException += (sender, eventArgs) => {
                Debug.WriteLine(eventArgs.ToString());
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, exception) => {
                Debug.WriteLine(exception.ToString());
            };

            var view = new Views.Main.FormMain();
            Application.Run(view);

        }



    }
}
