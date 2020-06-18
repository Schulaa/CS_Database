using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Datenbank
{
    class ControlLists
    {
        public static Control[] indexPage(Form1 form1)
        {
            List<Control> ctrlList = new List<Control>();
            Label label = new Label();
            label.Text = "Herzlich Willkommen in der Vereinsdatenbank 2020!";
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Size = label.PreferredSize;
            label.Top = (form1.ClientSize.Height - (label.Size.Height)) / 4;
            label.Left = (form1.ClientSize.Width - label.Width) / 2;
            ctrlList.Add(label);

            PictureBox img = new PictureBox();
            Image src = Image.FromFile("./img/logo_trans.png");

            img.Image = src;
            // img.Scale(new SizeF((float)0.3,(float) 0.3));
            img.Height = src.Height / 3;
            img.Width = src.Width / 3;
            img.SizeMode = PictureBoxSizeMode.Zoom;

            img.Top = label.Top + label.Height + 10;
            img.Left = (form1.ClientSize.Width - img.Width) / 2;
            ctrlList.Add(img);

            Button button = new Button();
            button.Text = "OK";
            button.Size = button.PreferredSize;
            button.Top = img.Top + img.Height + 10;
            button.Left = (form1.ClientSize.Width - button.Width) / 2;
            ctrlList.Add(button);
            return ctrlList.ToArray();
        }

        public static Control[] memberPage(Form1 form1)
        {
            List<Control> controls = new List<Control>();
            DataGridView dgv = new DataGridView();
            dgv.DataSource = DBCon.GetDataSet(new Person());
            dgv.DataMember = Person.CollectionName;
            dgv.Height = form1.ClientSize.Height - form1.MainMenuStrip.Height - 20;
            dgv.Width = form1.ClientSize.Width - 100;
            dgv.Top = form1.MainMenuStrip.Height + 20;
            dgv.Left = 10;

            controls.Add(dgv);

            return controls.ToArray();
        }
    }
}