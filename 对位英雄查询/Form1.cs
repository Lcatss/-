using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace 对位英雄查询
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.MultiSelect = false;
            LoadIcon();
            //this.IsMdiContainer = true;
            //Form2 form = new Form2();
            //form.MdiParent = this;
            //tabControl1.Visible = false;
            //Panel panel = new Panel();
            //panel.Dock = DockStyle.Fill;
            //panel.BringToFront();
            //this.Controls.Add(panel);
            //form.Parent = panel;
            //form.Dock = DockStyle.Fill;
            //form.Show();
        }

        private void LoadIcon()
        {
            var bitmap = Resource1.champion;
            imageList1.ImageSize = new Size(bitmap.Width, bitmap.Width);
            var count = bitmap.Height / bitmap.Width;
            for (int i = 0; i < count; i++)
            {
                Rectangle rectangle = new Rectangle(0, i * bitmap.Width, bitmap.Width, bitmap.Width);
                imageList1.Images.Add(bitmap.Clone(rectangle, bitmap.PixelFormat));
                listView1.Items.Add("", i);
            }
            listView1.View = View.LargeIcon;
            listView1.LargeImageList = imageList1;
        }


        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var index = listView1.SelectedItems[0].Index.ToString();
                
                MessageBox.Show(index);
            }
        }
    }
}
