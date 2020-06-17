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
        public Form1()
        {

            InitializeComponent();
            AddMenu();
            this.WindowState = FormWindowState.Maximized;
        }
        private void AddMenu()
        {
            ToolStripButton homeBtn = new ToolStripButton("Home", Image.FromFile("./img/home.png"));
            MenuStrip menu = new MenuStrip();

            // Image home = Image.FromFile("./img/home.png");
            // homeBtn.Image = home;
            // homeBtn.AutoSize = true;
            homeBtn.ImageAlign = ContentAlignment.MiddleCenter;
            homeBtn.TextAlign = ContentAlignment.MiddleCenter;
            homeBtn.Click += new EventHandler(ClickHomeBtn);

            void ClickHomeBtn(Object sender, EventArgs e)
            {
                //TODO
                System.Windows.Forms.MessageBox.Show("Home clicked");
            }

            // ToolStripDropDownButton memberDropdown = new ToolStripDropDownButton("Mitglieder",);
            ToolStripButton memberListBtn = new ToolStripButton("Mitgliederliste", Image.FromFile("./img/member.png"));
            memberListBtn.Click += new EventHandler(ClickMemberBtn);

            void ClickMemberBtn(Object sender, EventArgs e)
            {
                //TODO
                System.Windows.Forms.MessageBox.Show("Member list clicked");
            }

            // memberDropdown.DropDown.Items.Add(memberListBtn);
            // memberDropdown.DropDown.Size = memberDropdown.DropDown.PreferredSize;

            ToolStripButton ledgerBtn = new ToolStripButton("Kassenbuch", Image.FromFile("./img/ledger.png"));
            ledgerBtn.Click += new EventHandler(ClickLedgerBtn);

            void ClickLedgerBtn(Object sender, EventArgs e)
            {
                //TODO
                System.Windows.Forms.MessageBox.Show("ledger clicked");
            }

            menu.Items.Add(homeBtn);
            menu.Items.Add(memberListBtn);
            menu.Items.Add(ledgerBtn);
            menu.AutoSize = true;


            this.Controls.Add(menu);
            this.MainMenuStrip = menu;


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
