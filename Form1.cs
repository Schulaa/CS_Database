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
        public WindowType state;
        public Form1()
        {
            InitializeComponent();
            this.state = WindowType.Home;
            AddMenu();
            this.WindowState = FormWindowState.Maximized;
            this.ClientSizeChanged += new EventHandler(onClientSizeChangedEvent);

            void onClientSizeChangedEvent(object sender, EventArgs e)
            {
                switch (state)
                {
                    case WindowType.Home:
                        resetView(ControlLists.indexPage(this));
                        break;
                    
                    case WindowType.Ledger:
                        resetView();
                        break;
                    
                    case WindowType.Member:
                        resetView(ControlLists.memberPage(this));
                        break;
                }
                
            }
        }
        private void AddMenu()
        {
            ToolStripButton homeBtn = new ToolStripButton("Home", Image.FromFile("./img/home.png"));
            MenuStrip menu = new MenuStrip();
            menu.LayoutStyle = ToolStripLayoutStyle.Flow;

            // Image home = Image.FromFile("./img/home.png");
            // homeBtn.Image = home;
            // homeBtn.AutoSize = true;
            homeBtn.ImageAlign = ContentAlignment.MiddleCenter;
            homeBtn.TextAlign = ContentAlignment.MiddleCenter;
            homeBtn.Click += new EventHandler(ClickHomeBtn);

            void ClickHomeBtn(Object sender, EventArgs e)
            {
                //TODO
                // System.Windows.Forms.MessageBox.Show("Home clicked");
                this.state = WindowType.Home;
                this.resetView(ControlLists.indexPage(this));
            }

            // ToolStripDropDownButton memberDropdown = new ToolStripDropDownButton("Mitglieder",);
            ToolStripButton memberListBtn = new ToolStripButton("Mitgliederliste", Image.FromFile("./img/member.png"));
            memberListBtn.Click += new EventHandler(ClickMemberBtn);

            void ClickMemberBtn(Object sender, EventArgs e)
            {
                //TODO
                this.state = WindowType.Member;
                this.resetView(ControlLists.memberPage(this));
            }

            // memberDropdown.DropDown.Items.Add(memberListBtn);
            // memberDropdown.DropDown.Size = memberDropdown.DropDown.PreferredSize;

            ToolStripButton ledgerBtn = new ToolStripButton("Kassenbuch", Image.FromFile("./img/ledger.png"));
            ledgerBtn.Click += new EventHandler(ClickLedgerBtn);

            void ClickLedgerBtn(Object sender, EventArgs e)
            {
                //TODO
                System.Windows.Forms.MessageBox.Show("ledger clicked");
                this.state = WindowType.Ledger;
            }

            menu.Items.Add(homeBtn);
            menu.Items.Add(memberListBtn);
            menu.Items.Add(ledgerBtn);
            menu.AutoSize = true;


            this.Controls.Add(menu);
            this.MainMenuStrip = menu;


        }
        private void resetView()
        {
            this.Controls.Clear();
            AddMenu();
        }
        private void resetView(Control[] newCtrls)
        {
            resetView();
            this.Controls.AddRange(newCtrls);
        }
    }

    public enum WindowType
    {
        Home,
        Member,
        Ledger
    }
}
