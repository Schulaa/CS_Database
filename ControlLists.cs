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

            //search box:
            Label searchLbl = new Label();
            searchLbl.Text = "Namen suchen: ";
            searchLbl.Size = searchLbl.PreferredSize;
            searchLbl.TextAlign = ContentAlignment.BottomLeft;

            TextBox searchBox = new TextBox();
            searchBox.Height = searchLbl.Size.Height;
            searchBox.Width = searchLbl.Size.Width * 2;
            searchBox.Top = form1.MainMenuStrip.Height + 10;
            searchBox.Left = form1.ClientSize.Width - searchBox.Size.Width - 10;
            searchBox.BackColor = Color.White;

            searchLbl.Top = searchBox.Top;
            searchLbl.Left = searchBox.Left - searchLbl.Size.Width - 10;
            controls.AddRange(new Control[] { searchLbl, searchBox });

            //table
            DataGridView dgv = new DataGridView();
            dgv.DataSource = DBCon.GetDataSet(new Person());
            dgv.DataMember = Person.CollectionName;
            dgv.Height = form1.ClientSize.Height - form1.MainMenuStrip.Height - 20;
            dgv.Width = form1.ClientSize.Width - 100;
            dgv.Top = form1.MainMenuStrip.Height + 50;
            dgv.Left = 10;
            dgv.ReadOnly = true;
            dgv.AllowUserToDeleteRows = false;

            //buttons
            string[] buttonNames = { "Neu..", "Bearbeiten..", "Löschen.." };
            Image[] buttonIcons = { Image.FromFile("./img/new_data_smol.png"), Image.FromFile("./img/edit_data_smol.png"), Image.FromFile("./img/delete_data_smol.png") };
            EventHandler[] buttonEvents = { new EventHandler(addMember), new EventHandler(editMember), new EventHandler(deleteMember) };

            void addMember(Object sender, EventArgs e)
            {
                //TODO
                System.Windows.Forms.MessageBox.Show("add member");
            }
            void editMember(Object sender, EventArgs e)
            {
                //TODO
                System.Windows.Forms.MessageBox.Show("edit member");
            }
            void deleteMember(Object sender, EventArgs e)
            {
                //TODO
                System.Windows.Forms.MessageBox.Show("delete member" + dgv.SelectedRows.Count);
            }
            void searchBoxLeft(object sender, EventArgs e)
            {
                    dgv.DataSource = DBCon.GetDataSet(new Person(), searchBox.Text);
            }
            searchBox.Leave += new EventHandler(searchBoxLeft);
            Button accept = new Button();
            accept.Click += new EventHandler(searchBoxLeft);
            form1.AcceptButton = accept;

            Size prefSize = new Size();
            for (int i = 0; i < buttonNames.Length; i++)
            {
                Button btn = new Button();
                btn.Text = buttonNames[i];
                // btn.TextAlign = ContentAlignment.MiddleLeft;
                if (prefSize.IsEmpty)
                {
                    prefSize = btn.PreferredSize;
                }
                btn.Image = buttonIcons[i];
                // btn.ImageAlign = ContentAlignment.MiddleLeft;
                // btn.BackgroundImage = buttonIcons[i];
                // btn.BackgroundImageLayout = ImageLayout.Zoom;

                btn.Click += buttonEvents[i];

                btn.Size = prefSize;
                btn.Width = prefSize.Width * 2;
                btn.Top = form1.MainMenuStrip.Height + 10;
                btn.Left = 10 + (i * (btn.Width + 5));
                controls.Add(btn);
            }

            controls.Add(dgv);

            return controls.ToArray();
        }
    }
}