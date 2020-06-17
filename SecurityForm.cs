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
    public class SecurityForm : Form
    {
        TextBox tbox;
        public SecurityForm()
        {
            this.CenterToParent();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 500);
            // this.ClientSize = System.Drawing.Size()
            this.Text = "Anmeldung";
            initSecurity();
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

            Button btnCancel = new Button();
            btnCancel.Text = "Abbrechen";
            btnCancel.TextAlign = ContentAlignment.MiddleCenter;
            btnCancel.Size = btnCancel.PreferredSize;
            btnCancel.Left = tbox.Left + tbox.Width - btnCancel.Width;
            btnCancel.Top = btn.Top;
            btnCancel.Visible = true;
            btnCancel.Click += new EventHandler(clickBtnCancel);

            void clickBtnCancel(object sender, EventArgs e)
            {
                this.Dispose();
            }

            this.AcceptButton = btn;
            this.CancelButton = btnCancel;
            this.Controls.Add(btn);
            this.Controls.Add(btnCancel);
            // this.CenterToParent();
        }
        private void enterPassword(Object sender, EventArgs e)
        {
            const string SETPWD_QST = "Es ist noch kein Passwort gesetzt.\nMöchten Sie Ihre Eingabe als Passwort setzen?";
            const string REPEATPWD_LBL = "Bitte wiederholen Sie das Passwort, um es zu setzen.";
            string firstEntry = tbox.Text;

            try
            {
                if (DBCon.checkPassword(firstEntry))
                {
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    this.DialogResult = DialogResult.Cancel;
                    tbox.Text = "";
                }
            }
            catch (IndexOutOfRangeException)
            {
                DialogResult dr = MessageBox.Show(SETPWD_QST, "Passwort nicht gesetzt", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification, false);
                if (dr == DialogResult.Yes)
                {
                    string res = popupTextboxForm("Passwort bestätigen", REPEATPWD_LBL);

                    if (res == firstEntry)
                    {
                        DBCon.setPassword(res);
                    }

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
            using (Form f = new Form())
            {
                f.Text = caption;


                Label lbl = new Label();
                lbl.Text = text;
                lbl.Size = lbl.PreferredSize;
                lbl.Top = (f.ClientSize.Height - lbl.Height) / 2;
                lbl.Left = (f.ClientSize.Width - lbl.Width) / 2;

                TextBox tebox = new TextBox();
                tebox.Height = tebox.PreferredHeight;
                tebox.Width = lbl.Width;
                tebox.PasswordChar = '#';
                tebox.Top = lbl.Top + lbl.Height + 10;
                tebox.Left = lbl.Left;
                tebox.AcceptsReturn = false;
                tebox.Multiline = false;
                tebox.TextAlign = HorizontalAlignment.Center;

                Button btn = new Button();
                btn.Text = "Bestätigen";
                btn.TextAlign = ContentAlignment.MiddleCenter;
                btn.Size = btn.PreferredSize;
                btn.Top = tebox.Top + tebox.Height + 10;
                btn.Left = tebox.Left;
                btn.Click += new EventHandler(acceptBtn);

                void acceptBtn(Object sender, EventArgs e)
                {
                    response = tebox.Text;
                    f.Dispose();
                }

                Button btnDeny = new Button();
                btnDeny.Text = "Abbrechen";
                btnDeny.TextAlign = ContentAlignment.MiddleCenter;
                btnDeny.Size = btn.PreferredSize;
                btnDeny.Top = btn.Top;
                btnDeny.Left = tebox.Left + tebox.Width - btnDeny.Width;
                btnDeny.Click += new EventHandler(denyBtn);

                void denyBtn(Object sender, EventArgs e)
                {
                    response = "";
                    this.Dispose();
                }



                f.Controls.Add(lbl);
                f.Controls.Add(tebox);
                f.Controls.Add(btn);
                f.Controls.Add(btnDeny);
                f.AcceptButton = btn;
                f.CancelButton = btnDeny;

                f.Shown += new EventHandler(formShown);

                void formShown(object sender, EventArgs e)
                {
                    // this.Enabled = false;
                    f.Enabled = true;
                    f.Activate();

                    f.BringToFront();

                    f.WindowState = FormWindowState.Normal;
                }
                f.WindowState = FormWindowState.Minimized;
                f.Location = this.Location;
                f.ShowDialog(this);

            }
            return response;
        }

    }

}