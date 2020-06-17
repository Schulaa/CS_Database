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
            switch (typus)
            {
                case WindowType.Init:
                    initSecurity();
                    break;
                case WindowType.Blank:
                    break;
                case WindowType.Table:
                    break;
                default:
                    break;
            }
        }
        private void initSecurity()
        {
            DBCon.unlocked = false;
            const string INITTEXT = "Willkommen in der Vereinsdatenbank 2020\nBitte geben Sie Ihr Passwort ein:";
            Label lbl = new Label();
            lbl.Text = INITTEXT;
            lbl.Size = lbl.PreferredSize;
            lbl.Left = (this.ClientSize.Width - lbl.Width) / 2;
            lbl.Top = (this.ClientSize.Height - lbl.Height) / 2;
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.Visible = true;
            this.Controls.Add(lbl);

            tbox = new TextBox();
            tbox.AcceptsReturn = false;
            tbox.Multiline = false;
            tbox.PasswordChar = '#';
            tbox.Height = tbox.PreferredHeight;
            tbox.Width = lbl.Width;
            tbox.Left = lbl.Left;
            tbox.Top = lbl.Top + lbl.Height + 10;
            tbox.Visible = true;
            tbox.TextAlign = HorizontalAlignment.Center;
            this.Controls.Add(tbox);

            Button btn = new Button();
            btn.Text = "Bestätigen";
            btn.TextAlign = ContentAlignment.MiddleCenter;
            btn.Size = btn.PreferredSize;
            btn.Left = tbox.Left;
            btn.Top = tbox.Top + tbox.Height + 10;
            btn.Visible = true;
            btn.Click += new EventHandler(this.enterPassword);

            this.AcceptButton = btn;
            this.Controls.Add(btn);
            // this.CenterToParent();
        }
        private void enterPassword(Object sender, EventArgs e)
        {
            const string SETPWD_QST = "Es ist noch kein Passwort gesetzt.\nMöchten Sie Ihre Eingabe als Passwort setzen?";
            const string REPEATPWD_LBL = "Bitte wiederholen Sie das Passwort, um es zu setzen.";

            try
            {
                if (DBCon.checkPassword(tbox.Text))
                {
                    //next Form
                }
                else
                {
                    tbox.Text = "";
                }
            }
            catch (IndexOutOfRangeException)
            {
                DialogResult dr = MessageBox.Show(SETPWD_QST, "Passwort nicht gesetzt", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification, false);
                if (dr == DialogResult.Yes)
                {
                    string res = popupTextboxForm("Passwort bestätigen", REPEATPWD_LBL);
                    Console.WriteLine(res);

                }
                else
                {
                    MessageBox.Show("no");
                }
            }
        }
        public string popupTextboxForm(string caption, string text)
        {
            string response = "";
            using (Form1 f = new Form1(WindowType.Blank))
            {

                f.Text = caption;

                Label lbl = new Label();
                lbl.Text = text;
                lbl.Size = lbl.PreferredSize;
                lbl.Top = (f.ClientSize.Height - lbl.Height) / 2;
                lbl.Left = (f.ClientSize.Width - lbl.Width) / 2;

                TextBox tbox = new TextBox();
                tbox.Height = tbox.PreferredHeight;
                tbox.Width = lbl.Width;
                tbox.PasswordChar = '#';
                tbox.Top = lbl.Top + lbl.Height + 10;
                tbox.Left = lbl.Left;
                tbox.AcceptsReturn = false;
                tbox.Multiline = false;
                tbox.TextAlign = HorizontalAlignment.Center;

                Button btn = new Button();
                btn.Text = "Bestätigen";
                btn.TextAlign = ContentAlignment.MiddleCenter;
                btn.Size = btn.PreferredSize;
                btn.Top = tbox.Top + tbox.Height + 10;
                btn.Left = tbox.Left;
                btn.Click += new EventHandler(acceptBtn);

                void acceptBtn(Object sender, EventArgs e)
                {
                    response = tbox.Text;
                    f.Dispose();
                    this.TopMost = true;
                }

                Button btnDeny = new Button();
                btnDeny.Text = "Abbrechen";
                btnDeny.TextAlign = ContentAlignment.MiddleCenter;
                btnDeny.Size = btn.PreferredSize;
                btnDeny.Top = btn.Top;
                btnDeny.Left = tbox.Left + tbox.Width - btnDeny.Width;
                btnDeny.Click += new EventHandler(denyBtn);

                void denyBtn(Object sender, EventArgs e)
                {
                    response = "";
                    this.Dispose();
                }



                f.Controls.Add(lbl);
                f.Controls.Add(tbox);
                f.Controls.Add(btn);
                f.Controls.Add(btnDeny);
                f.AcceptButton = btn;
                f.CancelButton = btnDeny;

                f.Shown += new EventHandler(formShown);

                void formShown(object sender, EventArgs e)
                {
                    this.Enabled = false;
                    f.Enabled = true;
                    f.Activate();
                    f.BringToFront();
                    tbox.Focus();
                    
                    f.TopMost = true;
                    f.WindowState = FormWindowState.Normal;
                }
                f.WindowState = FormWindowState.Minimized;
                f.ShowDialog(this);



            }
            return response;
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
