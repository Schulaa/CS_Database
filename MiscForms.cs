using System;
using System.Windows.Forms;
using System.Data;

namespace Datenbank
{
    class MiscForms : Form
    {
        public static void invokeMemberCard(Person p)
        {
            MiscForms memberCard = new MiscForms();
            memberCard.Text = "Mitgliederkarte " + p.firstName + " " + p.lastName;
            memberCard.WindowState = FormWindowState.Maximized;
            memberCard.ShowDialog();
        }
    }
}