using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Datenbank
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool tryAgain = true;

            while (tryAgain)
            {
                if (new SecurityForm().ShowDialog() == DialogResult.OK)
                {
                    tryAgain = false;
                    Application.Run(new Form1());
                }
                else
                {
                    if (System.Windows.Forms.MessageBox.Show("Passwort falsch!\nErneut versuchen?", "Fehler", MessageBoxButtons.RetryCancel,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly, false) == DialogResult.Retry)
                    {
                        tryAgain = true;
                    }
                    else
                    {
                        tryAgain = false;
                        Application.Exit();
                    }
                }
            }
        }
    }
}
