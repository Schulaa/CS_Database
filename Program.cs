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

            // DBCon.writeToDb(new Person{firstName="Simon",lastName="Schuler", id=1},mode.Insert);
            // DBCon.writeToDb(new Person{firstName="Test",lastName="Test2", id=2},mode.Insert);
            // List<DBObject> persons = new List<DBObject>();
            // DBCon.readDb(new Person(), persons);
            // foreach (Person item in persons)
            // {
                // Console.WriteLine(item.id + " " + item.firstName + " " + item.lastName);
            // }


            Application.Run(new Form1());

          
        }
    }
}
