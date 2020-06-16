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
        // dgv.DataSource = DBCon.getDataSet();
        dgv.DataMember = "person";
        
        dgv.SetBounds(10,10,300,200);
        this.Controls.Add(dgv);
    }
    }
}
