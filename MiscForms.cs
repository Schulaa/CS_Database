using System;
using System.Windows.Forms;
using System.Data;

namespace Datenbank
{
    class MiscForms : Form
    {
        public static void invokeMemberCard(Person p, Form parent)
        {
            MiscForms memberCard = new MiscForms();
            memberCard.Text = "Mitgliederkarte " + p.firstName + " " + p.lastName;
            // memberCard.Size = new System.Drawing.Size(800,600);
            memberCard.WindowState = FormWindowState.Maximized;
            // memberCard.Parent = parent;
            memberCard.Controls.AddRange(ControlLists.memberCardPage(memberCard, p));
            memberCard.Disposed += new EventHandler(memberCardDisposed);
            memberCard.ShowDialog();

            void memberCardDisposed(object sender, EventArgs e)
            {
                parent.Update();
            }
        }
        
    }
    class MemberCard : Form
    {
        public MemberCard(Person p)
        {
            this.Text = "Mitgliederkarte " + p.firstName + " " + p.lastName;
            this.WindowState = FormWindowState.Maximized;
            this.Controls.AddRange(ControlLists.memberCardPage(this,p));
            
        }

    }
}