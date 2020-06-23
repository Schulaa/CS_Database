using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

namespace Datenbank
{
    class GUIElements
    {
        #region Card Controls
        public static Control[] GetCardPageControls(DBObject src, int defWdith = 250, int defHeight = 80, int startTopDif = 50, int startLeftDif = 20)
        {
            DataColumn[] fields = src.dataColumns;
            object[] data = src.getAsObjArr();
            List<Control> controls = new List<Control>();
            int fieldsPerCol = fields.Length / 2;
            Size defSize = new Size(defWdith, defHeight);
            for (int i = 0; i < fields.Length; i++)
            {
                Label lbl = new Label();
                lbl.Text = fields[i].ColumnName;
                lbl.Size = defSize;

                lbl.Top = startTopDif + (i % fieldsPerCol) * lbl.Size.Height + 20;
                lbl.Left = (i < fieldsPerCol) ? startLeftDif : (startLeftDif + defWdith * 2 + 50);

                if (fields[i].DataType == typeof(Person.type))
                {
                    ComboBox cbox = new ComboBox();
                    cbox.DropDownStyle = ComboBoxStyle.DropDownList;
                    foreach (var item in DBObject.GetEnumList<Person.type>())
                    {
                        cbox.Items.Add(item);
                    }
                    if (i < data.Length && data[i] != null)
                    {
                        cbox.SelectedItem = data[i];
                    }
                    else
                    {
                        cbox.Text = Person.type.Left.ToString();
                    }
                    // setControlBounds(cbox);
                    controls.Add(cbox);
                }
                else if (fields[i].DataType == typeof(Person.paymentType))
                {
                    ComboBox cbox = new ComboBox();
                    cbox.DropDownStyle = ComboBoxStyle.DropDownList;

                    foreach (var item in DBObject.GetEnumList<Person.paymentType>())
                    {
                        cbox.Items.Add(item);
                    }
                    if (i < data.Length && data[i] != null)
                    {
                        cbox.SelectedItem = data[i];
                    }
                    else
                    {
                        cbox.Text = Person.paymentType.None.ToString();
                    }
                    // setControlBounds(cbox);
                    controls.Add(cbox);

                }
                else if (fields[i].DataType == typeof(DateTime))
                {
                    DateTimePicker dbox = new DateTimePicker();
                    dbox.Format = DateTimePickerFormat.Short;

                    try
                    {
                        dbox.Value = (DateTime)data[i];
                    }
                    catch (System.ArgumentOutOfRangeException)
                    {
                        // dbox.Enabled = false;
                        dbox.Format = DateTimePickerFormat.Custom;
                        dbox.CustomFormat = " ";
                        dbox.Validated += new EventHandler(dBoxValidated);
                        dbox.MouseDown += new MouseEventHandler(dBoxValidated);

                        void dBoxValidated(object sender, EventArgs e)
                        {
                            dbox.Format = DateTimePickerFormat.Short;
                        }
                    }

                    setControlBounds(dbox);
                    controls.Add(dbox);
                }
                else
                {
                    TextBox tbox = new TextBox();
                    if (i < data.Length && data[i] != null)
                    {
                        tbox.Text = data[i].ToString();
                    }
                    setControlBounds(tbox);
                    controls.Add(tbox);

                }


                void setControlBounds(Control ctrl)
                {
                    ctrl.Top = lbl.Top;
                    ctrl.Left = lbl.Left + lbl.Size.Width + 10;
                    ctrl.Size = lbl.Size;
                    ctrl.Name = lbl.Text;

                    if (ctrl is TextBox)
                    {
                        ctrl.TextChanged += new EventHandler(ctrlTextChanged);
                    }
                    else if ((ctrl is ComboBox))
                    {
                        ComboBox cb = (ComboBox)ctrl;
                        cb.SelectedValueChanged += new EventHandler(ctrlTextChanged);
                    }
                    else if (ctrl is DateTimePicker)
                    {
                        DateTimePicker db = (DateTimePicker)ctrl;
                        db.ValueChanged += new EventHandler(ctrlValidated);
                    }
                    void ctrlTextChanged(object sender, EventArgs e)
                    {
                        Control ctrl = (Control)sender;
                        ctrl.Validated += new EventHandler(ctrlValidated);
                    }

                    void ctrlValidated(object sender, EventArgs e)
                    {
                        Control ctrl = (Control)sender;
                        int fieldId = fields.ToList().IndexOf(fields.Where(x => x.ColumnName == ctrl.Name).First());
                        if (ctrl is ComboBox)
                        {
                            Enum.TryParse(fields[fieldId].DataType, ctrl.Text, out data[fieldId]);
                        }
                        else if (ctrl is DateTimePicker)
                        {
                            DateTime date;
                            if (DateTime.TryParse(ctrl.Text, out date))
                            {
                                data[fieldId] = date;
                            }
                        }
                        else
                        {

                            if (fields[fieldId].DataType == typeof(int))
                            {
                                int num;
                                if (int.TryParse(ctrl.Text, out num))
                                {
                                    if (fields[fieldId].Unique && src != null)
                                    {
                                        //TODO:
                                        // System.Windows.Forms.MessageBox.Show("Nach einer Primärschlüsseländerung muss die aktuelle Seite geschlossen werden!", "Meldung");
                                        // person = DBCon.UpdateIdPerson(person, num);
                                        // data[fieldId] = num;
                                        // control.Parent.Dispose();
                                    }
                                    else
                                    {
                                        data[fieldId] = num;
                                    }
                                }
                            }
                            else
                            {
                                data[fieldId] = ctrl.Text;
                            }
                        }
                        src.setAsObjArr(data);
                        //TODO:
                        // DBCon.UpsertPerson(person);
                    }
                }
                controls.Add(lbl);
            }
            return controls.ToArray();

        }
        #endregion

        #region Buttons

        public static Control[] GetCardPageButtons(DBObject src, Form target)
        {
            List<Control> controls = new List<Control>();
            List<string> captions = new List<string>();
            List<EventHandler> eventHandlers = new List<EventHandler>();
            Size prefSize = new Size();

            if ((src is Person))
            {
                //Delete:
                captions.Add("Löschen..");
                eventHandlers.Add(delegate (object sender, EventArgs e) { deleteMember(sender, e, src); });

                //Account Information:
                captions.Add("Kontoinformationen..");
                eventHandlers.Add(delegate (object sender, EventArgs e) { openAccountInfo(sender, e, src); });
            }
            else if (src is PersonAccountInfo)
            {
                //Delete:
                captions.Add("Löschen..");
                eventHandlers.Add(delegate (object sender, EventArgs e) { deleteAccountInfo(sender, e, src); });
            }
            for (int i = 0; i < captions.Count; i++)
            {
                Button btn = new Button();
                btn.Text = captions[i];
                if (prefSize.IsEmpty)
                {
                    prefSize = btn.PreferredSize;
                }
                btn.Click += eventHandlers[i];
                btn.Size = prefSize;
                btn.Width = prefSize.Width * 2;
                btn.Top = target.MainMenuStrip.Height + 10;
                btn.Left = 10 + (i * (btn.Width + 5));
                controls.Add(btn);
            }

            return controls.ToArray();
        }

        #endregion

        #region EventHandlers
        private static void deleteMember(object sender, EventArgs eventArgs, DBObject src)
        {

        }
        private static void openAccountInfo(object sender, EventArgs eventArgs, DBObject src)
        {

        }
        private static void deleteAccountInfo(object sender, EventArgs eventArgs, DBObject src)
        {

        }
        #endregion

    }
}