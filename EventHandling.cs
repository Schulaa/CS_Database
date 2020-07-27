using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Linq;


namespace Datenbank
{
    class EventHandling
    {
        #region Member
        public static void deleteMember(object sender, EventArgs e, DBObject src)
        {

        }
        public static void addMember(object sender, EventArgs e)
        {

        }
        public static void editMember(object sender, EventArgs e, DBObject src)
        {

        }

        #endregion

        #region Account Information
        public static void openAccountInfo(object sender, EventArgs e, DBObject src)
        {

        }

        public static void deleteAccountInfo(Object sender, EventArgs e, PersonAccountInfo src)
        {

        }
        #endregion

        #region General Events

        public static void DBControlTextChanged(object sender, EventArgs e, DBObject src, object[] data)
        {
            Control ctrl = (Control)sender;
            ctrl.Validated += delegate (object sender, EventArgs e) { DBControlValidated(sender, e, src, data); };
        }
        public static void DBControlValidated(object sender, EventArgs eventArgs, DBObject src, object[] data)
        {
            Control ctrl = (Control)sender;
            var fields = src.dataColumns.ToList();
            int fieldId = fields.IndexOf(fields.Where(x => x.ColumnName == ctrl.Name).First());
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
                            System.Windows.Forms.MessageBox.Show("Nach einer Primärschlüsseländerung muss die aktuelle Seite geschlossen werden!", "Meldung");
                            src = src.UpdateId(num);
                            data[fieldId] = num;
                            ctrl.Parent.Dispose();
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
            src.Upsert();
        }
    }
    #endregion
}

}