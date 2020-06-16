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
        public Form1()
        {
            InitializeComponent();
            // CreateListView();
            createGridView();
        }

        private void CreateListView() {
            ListView lv = new ListView();
            lv.Bounds = new Rectangle(new Point(10,10), new Size(300,200));
            // lv.View = View.Details;
            lv.AllowColumnReorder = true;
            // lv.GridLines = true;
            lv.Sorting = SortOrder.Ascending;
            List<DBObject> people = new List<DBObject>();
            DBCon.readDb(new Person(),people);
            Console.WriteLine(people.Count);
            ListViewItem[] items = new ListViewItem[people.Count];
            for (int i = 0; i < people.Count; i++)
            {
                Person dmy = (Person) people[i];
                items[i] = new ListViewItem(dmy.firstName);
                items[i].SubItems.Add(dmy.lastName);
                
            }
            lv.Items.AddRange(items);

            this.Controls.Add(lv);
        }
    private void createGridView() {
        DataGridView dgv = new DataGridView();
        dgv.DataSource = DBCon.getDataSet();
        dgv.DataMember = "person";

        dgv.SetBounds(10,10,300,200);
        this.Controls.Add(dgv);
    }
    }
}
