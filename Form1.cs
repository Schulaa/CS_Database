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
            createGridView();
        }

    private void createGridView() {
        DataGridView dgv = new DataGridView();
        dgv.DataSource = DBCon.GetDataSet(new Person());
        dgv.DataMember = Person.CollectionName;
        dgv.SetBounds(10,10,this.Width,this.Height);
        this.Controls.Add(dgv);
    }
    }
}
