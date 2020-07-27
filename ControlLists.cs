using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Linq;

namespace Datenbank
{
    class ControlLists
    {
        
        #region MainPage
        public static Control[] indexPage(Form form1)
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
        #endregion

        #region MemberPages
        public static Control[] memberListPage(Form form1)
        {
            Person pers = new Person();
            List<Control> controls = new List<Control>();
            Person dmyPers = new Person();
            #region SearchBox

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

            #endregion

            #region Table

            DataGridView dgv = new DataGridView();
            dgv.DataSource = DBOperations.GetDataSet<Person>(new Person());
            dgv.DataMember = dmyPers.CollectionName;
            dgv.Height = form1.ClientSize.Height - form1.MainMenuStrip.Height - 20;
            dgv.Width = form1.ClientSize.Width - 100;
            dgv.Top = form1.MainMenuStrip.Height + 50;
            dgv.Left = 10;
            dgv.ReadOnly = true;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToAddRows = false;
            // dgv.CellClick += new DataGridViewCellEventHandler(clickTableCont);
            dgv.CellDoubleClick += new DataGridViewCellEventHandler(clickTableCont);


            #endregion

            #region Buttons

            string[] buttonNames = { "Neu..", "Bearbeiten..", "Löschen.." };
            Image[] buttonIcons = { Image.FromFile("./img/new_data_smol.png"), Image.FromFile("./img/edit_data_smol.png"), Image.FromFile("./img/delete_data_smol.png") };
            EventHandler[] buttonEvents = { new EventHandler(addMember), new EventHandler(editMember), new EventHandler(deleteMember) };

            searchBox.Leave += new EventHandler(searchBoxLeft);
            Button accept = new Button();
            accept.Click += new EventHandler(searchBoxLeft);
            form1.AcceptButton = accept;

            Size prefSize = new Size();
            for (int i = 0; i < buttonNames.Length; i++)
            {
                Button btn = new Button();
                btn.Text = buttonNames[i];
                if (prefSize.IsEmpty)
                {
                    prefSize = btn.PreferredSize;
                }
                btn.Image = buttonIcons[i];

                btn.Click += buttonEvents[i];

                btn.Size = prefSize;
                btn.Width = prefSize.Width * 2;
                btn.Top = form1.MainMenuStrip.Height + 10;
                btn.Left = 10 + (i * (btn.Width + 5));
                controls.Add(btn);
            }

            #endregion

            #region DropDown

            Button defFilterDropDown = new Button();
            defFilterDropDown.Text = "Standardfilter...";
            defFilterDropDown.Size = prefSize;
            defFilterDropDown.Width = prefSize.Width * 2;
            defFilterDropDown.Top = form1.MainMenuStrip.Height + 10;
            defFilterDropDown.Left = 10 + (buttonNames.Length * (defFilterDropDown.Width + 5));

            string[] dropDownNames = { "Alle", "Barzahler", "SEPA", "Aktiv", "Passiv", "Ehrenmitglied" };
            ContextMenuStrip dropDown = new ContextMenuStrip();

            for (int i = 0; i < dropDownNames.Length; i++)
            {
                ToolStripItem dmy = new ToolStripButton();
                dmy.Text = dropDownNames[i];
                dmy.Size = defFilterDropDown.Size;
                dmy.Click += new EventHandler(clickSetDefFilter);
                dropDown.Items.Add(dmy);
            }
            defFilterDropDown.Click += new EventHandler(clickDropdown);
            controls.Add(defFilterDropDown);

            #endregion

            controls.Add(dgv);

            #region EventHandler

            void addMember(Object sender, EventArgs e)
            {
                Person p = new Person();
                p.id = p.GetNextId();
                p.Upsert();
                Control ctrl = (Control)sender;
                MemberCard mc = new MemberCard(p);
                mc.Deactivate += new EventHandler(onDisposed);
                mc.ShowDialog();

            }
            void editMember(Object sender, EventArgs e)
            {
                int rowId = dgv.SelectedCells[0].RowIndex;
                Control ctrl = (Control)sender;
                if (rowId >= 0)
                {
                    var query =
                        from flds in dmyPers.dataColumns
                        where (flds.Unique == true)
                        select flds.ColumnName;

                    var id = dgv.Rows[rowId].Cells[query.First()].Value;

                    MemberCard mc = new MemberCard(DBCon.GetPersonById((int)id));
                    mc.Deactivate += new EventHandler(onDisposed);
                    mc.ShowDialog();

                }
            }
            void deleteMember(Object sender, EventArgs e)
            {
                pers = new Person();
                int rowId = dgv.SelectedCells[0].RowIndex;
                if (rowId >= 0)
                {
                    var query = pers.dataColumns.Where(x=>x.Unique==true).Select(x=>x.ColumnName);


                    var id = dgv.Rows[rowId].Cells[query.First()].Value;
                    if (MessageBox.Show("Sind Sie sicher, dass Sie das Mitglied löschen möchten?", "Löschen", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly, false) == DialogResult.Yes)
                    {
                        DBCon.DeletePerson(DBCon.GetPersonById((int)id));
                        onDisposed(sender, e);
                    }
                }

            }
            void searchBoxLeft(object sender, EventArgs e)
            {
                dgv.DataSource = DBCon.GetDataSet(new Person(), searchBox.Text);
            }
            void clickDropdown(object sender, EventArgs e)
            {
                Button clicked = (Button)sender;
                dropDown.Show(clicked.Location);
            }
            void clickSetDefFilter(object sender, EventArgs e)
            {
                ToolStripItem clicked = (ToolStripItem)sender;
                switch (clicked.Text)
                {
                    case "Barzahler":
                        dgv.DataSource = DBCon.PersonFilterPmtType(Person.paymentType.Cash);
                        break;
                    case "SEPA":
                        dgv.DataSource = DBCon.PersonFilterPmtType(Person.paymentType.SEPA);
                        break;
                    case "Aktiv":
                        dgv.DataSource = DBCon.PersonFilterMemberType(Person.type.Active);
                        break;
                    case "Passiv":
                        dgv.DataSource = DBCon.PersonFilterMemberType(Person.type.Passive);
                        break;
                    case "Ehrenmitglied":
                        dgv.DataSource = DBCon.PersonFilterMemberType(Person.type.Honor);
                        break;
                    case "Alle":
                        dgv.DataSource = DBCon.GetDataSet(new Person());
                        break;
                }
            }
            void clickTableCont(object sender, DataGridViewCellEventArgs e)
            {
                pers = new Person();
                if (e.RowIndex >= 0)
                {
                    DataGridView dmyDgv = (DataGridView)sender;

                    var query =
                        from flds in pers.dataColumns
                        where (flds.Unique == true)
                        select flds.ColumnName;

                    var id = dmyDgv.Rows[e.RowIndex].Cells[query.First()].Value;

                    MemberCard mc = new MemberCard(DBCon.GetPersonById((int)id));
                    mc.Deactivate += new EventHandler(onDisposed);
                    mc.ShowDialog();

                }
            }
            void onDisposed(Object sender, EventArgs e)
            {
                dgv.DataSource = DBCon.GetDataSet(new Person());
            }

            #endregion

            return controls.ToArray();
        }

        // public static Control[] memberCardPage(Form form1, Person person)
        // {
        //     List<Control> controls = new List<Control>();
        //     DataColumn[] fields = person.dataColumns;

        //     object[] data = person.getAsObjArr();
        //     int startHeight = 50;
        //     int startLeft = 20;
        //     Size defSize = new Size(250, 80);
        //     int fieldsPerCol = fields.Count() / 2;
        //     for (int i = 0; i < fields.Count(); i++)
        //     {
        //         Label lbl = new Label();
        //         lbl.Text = fields[i].ColumnName;
        //         lbl.Size = defSize;

        //         lbl.Top = startHeight + (i % fieldsPerCol) * (lbl.Size.Height) + 20;
        //         lbl.Left = (i < fieldsPerCol) ? startLeft : (startLeft + defSize.Width * 2 + 50);

        //         if (fields[i].DataType == typeof(Person.type))
        //         {
        //             ComboBox cbox = new ComboBox();
        //             cbox.DropDownStyle = ComboBoxStyle.DropDownList;
        //             foreach (var item in DBObject.GetEnumList<Person.type>())
        //             {
        //                 cbox.Items.Add(item);
        //             }
        //             if (i < data.Length && data[i] != null)
        //             {
        //                 cbox.SelectedItem = data[i];
        //             }
        //             else
        //             {
        //                 cbox.Text = Person.type.Left.ToString();
        //             }
        //             setControlBounds(cbox);
        //             controls.Add(cbox);
        //         }
        //         else if (fields[i].DataType == typeof(Person.paymentType))
        //         {
        //             ComboBox cbox = new ComboBox();
        //             cbox.DropDownStyle = ComboBoxStyle.DropDownList;

        //             foreach (var item in DBObject.GetEnumList<Person.paymentType>())
        //             {
        //                 cbox.Items.Add(item);
        //             }
        //             if (i < data.Length && data[i] != null)
        //             {
        //                 cbox.SelectedItem = data[i];
        //             }
        //             else
        //             {
        //                 cbox.Text = Person.paymentType.None.ToString();
        //             }
        //             setControlBounds(cbox);
        //             controls.Add(cbox);

        //             Button btn = new Button();
        //             btn.Image = Image.FromFile("./img/folder_contacts_smol.png");
        //             btn.Height = cbox.Size.Height + 5;
        //             btn.Width = btn.Height + 5;
        //             btn.Top = cbox.Top;
        //             btn.Left = cbox.Left + cbox.Size.Width + 10;
        //             btn.Click += new EventHandler(paiBtnClicked);
        //             void paiBtnClicked(object sender, EventArgs e)
        //             {
        //                 PersonAccountInfo pai = DBCon.getPAIforPerson(person);
        //                 if (pai == null)
        //                 {
        //                     pai = new PersonAccountInfo();
        //                     pai.personId = person.id;
        //                     pai.personName = person.lastName + ", " + person.firstName;
        //                     pai.mandateId = person.id;
        //                     DBCon.UpsertPersonAccountInfo(pai);
        //                     AccountInfoCard aic = new AccountInfoCard(pai);
        //                     aic.ShowDialog();
        //                 }
        //                 else
        //                 {
        //                     AccountInfoCard aic = new AccountInfoCard(pai);
        //                     aic.ShowDialog();
        //                 }
        //             }
        //             controls.Add(btn);

        //         }
        //         else if (fields[i].DataType == typeof(DateTime))
        //         {
        //             DateTimePicker dbox = new DateTimePicker();
        //             dbox.Format = DateTimePickerFormat.Short;

        //             try
        //             {
        //                 dbox.Value = (DateTime)data[i];
        //             }
        //             catch (System.ArgumentOutOfRangeException)
        //             {
        //                 // dbox.Enabled = false;
        //                 dbox.Format = DateTimePickerFormat.Custom;
        //                 dbox.CustomFormat = " ";
        //                 dbox.Validated += new EventHandler(dBoxValidated);
        //                 dbox.MouseDown += new MouseEventHandler(dBoxValidated);

        //                 void dBoxValidated(object sender, EventArgs e)
        //                 {
        //                     dbox.Format = DateTimePickerFormat.Short;
        //                 }
        //             }

        //             setControlBounds(dbox);
        //             controls.Add(dbox);
        //         }
        //         else
        //         {
        //             TextBox tbox = new TextBox();
        //             if (i < data.Length && data[i] != null)
        //             {
        //                 tbox.Text = data[i].ToString();
        //             }
        //             setControlBounds(tbox);
        //             controls.Add(tbox);

        //         }


        //         void setControlBounds(Control control)
        //         {
        //             control.Top = lbl.Top;
        //             control.Left = lbl.Left + lbl.Size.Width + 10;
        //             control.Size = defSize;
        //             control.Name = lbl.Text;

        //             if (control is TextBox)
        //             {
        //                 control.TextChanged += new EventHandler(ctrlTextChanged);
        //             }
        //             else if ((control is ComboBox))
        //             {
        //                 ComboBox cb = (ComboBox)control;
        //                 cb.SelectedValueChanged += new EventHandler(ctrlTextChanged);
        //             }
        //             else if (control is DateTimePicker)
        //             {
        //                 DateTimePicker db = (DateTimePicker)control;
        //                 db.ValueChanged += new EventHandler(ctrlValidated);
        //             }
        //             void ctrlTextChanged(object sender, EventArgs e)
        //             {
        //                 Control ctrl = (Control)sender;
        //                 ctrl.Validated += new EventHandler(ctrlValidated);
        //             }

        //             void ctrlValidated(object sender, EventArgs e)
        //             {
        //                 Control ctrl = (Control)sender;
        //                 int fieldId = fields.ToList().IndexOf(fields.Where(x => x.ColumnName == ctrl.Name).First());
        //                 if (ctrl is ComboBox)
        //                 {
        //                     Enum.TryParse(fields[fieldId].DataType, ctrl.Text, out data[fieldId]);
        //                 }
        //                 else if (ctrl is DateTimePicker)
        //                 {
        //                     DateTime date;
        //                     if (DateTime.TryParse(ctrl.Text, out date))
        //                     {
        //                         data[fieldId] = date;
        //                     }
        //                 }
        //                 else
        //                 {

        //                     if (fields[fieldId].DataType == typeof(int))
        //                     {
        //                         int num;
        //                         if (int.TryParse(ctrl.Text, out num))
        //                         {
        //                             if (fields[fieldId].Unique && person != null)
        //                             {
        //                                 System.Windows.Forms.MessageBox.Show("Nach einer Primärschlüsseländerung muss die aktuelle Seite geschlossen werden!", "Meldung");
        //                                 person = DBCon.UpdateIdPerson(person, num);
        //                                 data[fieldId] = num;
        //                                 control.Parent.Dispose();
        //                             }
        //                             else
        //                             {
        //                                 data[fieldId] = num;
        //                             }
        //                         }
        //                     }
        //                     else
        //                     {
        //                         data[fieldId] = control.Text;
        //                     }
        //                 }
        //                 person.setAsObjArr(data);
        //                 DBCon.UpsertPerson(person);
        //             }
        //         }

        //         controls.Add(lbl);
        //     }

        //     return controls.ToArray();
        // }


        #endregion

        // #region AccountInfo
        // public static Control[] accountInfoCardPage(Form form, PersonAccountInfo ai)
        // {
        //     List<Control> controls = new List<Control>();
        //     DataColumn[] fields = PersonAccountInfo.dataColumns;
        //     object[] data = ai.getAsObjArr();
        //     int startHeight = 50;
        //     int startLeft = 20;
        //     Size defSize = new Size(250, 80);
        //     for (int i = 0; i < fields.Length; i++)
        //     {
        //         Label lbl = new Label();
        //         lbl.Text = fields[i].ColumnName;
        //         lbl.Size = defSize;
        //         lbl.Top = startHeight + (i * lbl.Size.Height) + 10;
        //         lbl.Left = startLeft;

        //         if (fields[i].DataType == typeof(DateTime))
        //         {
        //             DateTimePicker dbox = new DateTimePicker();
        //             dbox.Format = DateTimePickerFormat.Short;

        //             try
        //             {
        //                 dbox.Value = (DateTime)data[i];
        //             }
        //             catch (System.ArgumentOutOfRangeException)
        //             {
        //                 dbox.Format = DateTimePickerFormat.Custom;
        //                 dbox.CustomFormat = " ";
        //                 dbox.Validated += new EventHandler(dBoxValidated);
        //                 dbox.MouseDown += new MouseEventHandler(dBoxValidated);
        //                  void dBoxValidated(object sender, EventArgs e)
        //                 {
        //                     dbox.Format = DateTimePickerFormat.Short;
        //                 }
        //             }
        //             setControlBounds(dbox);
        //             controls.Add(dbox);
        //         }
        //         else
        //         {
        //             TextBox tbox = new TextBox();
        //             if (i < data.Length && data[i] != null)
        //             {
        //                 tbox.Text = data[i].ToString();
        //             }
        //             setControlBounds(tbox);
        //             controls.Add(tbox);


        //         }
        //         void setControlBounds(Control control)
        //         {
        //             control.Top = lbl.Top;
        //             control.Left = lbl.Left + lbl.Size.Width + 10;
        //             control.Size = defSize;
        //             control.Name = lbl.Text;

        //             if (control is TextBox)
        //             {
        //                 control.TextChanged += new EventHandler(ctrlTextChanged);
        //             }
        //             else if (control is ComboBox)
        //             {
        //                 ComboBox cb = (ComboBox)control;
        //                 cb.SelectedValueChanged += new EventHandler(ctrlTextChanged);
        //             }
        //             else if (control is DateTimePicker)
        //             {
        //                 DateTimePicker db = (DateTimePicker)control;
        //                 db.ValueChanged += new EventHandler(ctrlValidated);
        //             }

        //             void ctrlTextChanged(object sender, EventArgs e)
        //             {
        //                 Control ctrl = (Control)sender;
        //                 ctrl.Validated += new EventHandler(ctrlValidated);
        //             }

        //             void ctrlValidated(object sender, EventArgs e)
        //             {
        //                 Control ctrl = (Control)sender;

        //                 int fieldId = fields.ToList().IndexOf(fields.Where(x => x.ColumnName == ctrl.Name).First());
        //                 if (ctrl is DateTimePicker)
        //                 {
        //                     DateTime date;
        //                     if (DateTime.TryParse(ctrl.Text, out date))
        //                     {
        //                         data[fieldId] = date;
        //                     }
        //                 }
        //                 else
        //                 {

        //                     if (fields[fieldId].DataType == typeof(int))
        //                     {
        //                         int num;
        //                         if (int.TryParse(ctrl.Text, out num))
        //                         {
        //                             if (fields[fieldId].Unique)
        //                             {
        //                                 System.Windows.Forms.MessageBox.Show("Der Primärschlüssel kann nicht geändert werden!", "Meldung");
        //                                 // person = DBCon.UpdateIdPerson(person, num);
        //                                 // data[fieldId] = num;
        //                                 // control.Parent.Dispose();
        //                             }
        //                             else
        //                             {
        //                                 data[fieldId] = num;
        //                             }
        //                         }
        //                     }
        //                     else
        //                     {
        //                         data[fieldId] = control.Text;
        //                     }
        //                 }
        //                 ai.setAsObjArr(data);
        //                 DBCon.UpsertPersonAccountInfo(ai);
        //             }
        //         }
        //         controls.Add(lbl);
        //     }
        //     return controls.ToArray();
        // }
        // #endregion

    }
}