using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Datenbank
{
    public partial class Form1 : Form
    {
        private TextBox tbox;
        public Form1() : this(WindowType.Init)
        {

            // InitializeComponent();
            // CenterToScreen();
            // initSecurity();
            // createGridView();
            // createMenu();
        }
        public Form1(WindowType typus)
        {
            InitializeComponent();
            CenterToScreen();
            this.WindowState = FormWindowState.Maximized;
            }

        private void createGridView()
        {
            DataGridView dgv = new DataGridView();
            dgv.DataSource = DBCon.GetDataSet(new Person());
            dgv.DataMember = Person.CollectionName;
            dgv.SetBounds(10, 30, this.Width, this.Height);
            this.Controls.Add(dgv);
        }

        private void createMenu()
        {
            MenuStrip menuStrip = new MenuStrip();
            ToolStripMenuItem fileItem = new ToolStripMenuItem("File");
            menuStrip.Items.Add(fileItem);
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);
        }
    }

    public enum WindowType
    {
        Init,
        Blank,
        Table
    }
}
