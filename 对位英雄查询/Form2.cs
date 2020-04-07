using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 对位英雄查询
{
    public partial class Form2 : Form
    {
        public Form2(DataTable table)
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = table;
            dataGridView1.Columns[0].DataPropertyName = "icon";
            dataGridView1.Columns[1].DataPropertyName = "英雄";
            dataGridView1.Columns[2].DataPropertyName = "胜率";
            dataGridView1.Columns[3].DataPropertyName = "线杀率";

            dataGridView1.Sort(dataGridView1.Columns[2], ListSortDirection.Descending);
            dataGridView1.Sort(dataGridView1.Columns[3], ListSortDirection.Descending);

            //foreach (DataRow row in table.Rows)
            //{

            //    var newRow = new DataGridViewRow();
            //    newRow.Cells.Add(new DataGridViewImageCell() { Value = collection[(int)row[0]] });
            //    newRow.Cells.Add(new DataGridViewTextBoxCell() { Value = row[1].ToString() });
            //    newRow.Cells.Add(new DataGridViewTextBoxCell() { Value = row[2].ToString() });
            //    newRow.Cells.Add(new DataGridViewTextBoxCell() { Value = row[3].ToString() });
            //    dataGridView1.Rows.Add(newRow);
            //}
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var form = this.Parent.Parent as Form1;
            form.ReturnHome(this.Parent);
        }
    }
}
