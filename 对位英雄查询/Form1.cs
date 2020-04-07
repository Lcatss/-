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
using System.Diagnostics;

namespace 对位英雄查询
{
    public partial class Form1 : Form
    {
        ChampionInfos championInfos;
        LoadingPictureBox loading;
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            this.IsMdiContainer = true;
            var width = tabControl1.Width / tabControl1.TabPages.Count;
            var height = tabControl1.ItemSize.Height;
            tabControl1.ItemSize = new Size(width, height);

            StartLoading();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            LoadIconAsync();
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            championInfos = await Task.Run(() => Crawler.GetPostionChampions());
            InitTabPage();
            StopLoading();

        }

        private void StartLoading()
        {
            loading = new LoadingPictureBox();
            Controls.Add(loading);
            tabControl1.Visible = false;
        }

        private void StopLoading()
        {
            Controls.Remove(loading);
            tabControl1.Visible = true;
        }

        private void InitTabPage()
        {
            InitListView(listView1, championInfos.Top);
            InitListView(listView2, championInfos.Jungle);
            InitListView(listView3, championInfos.Mid);
            InitListView(listView4, championInfos.Adc);
            InitListView(listView5, championInfos.Support);
        }

        private void InitListView(ListView listView, List<Champion> champions)
        {
            listView.LargeImageList = imageList1;
            listView.MultiSelect = false;
            foreach (var champion in champions)
            {
                var item = new ListViewItem()
                {
                    Text = champion.Name,
                    ImageIndex = champion.Index,
                    Tag = champion
                };
                listView.Items.Add(item);
            }
        }

        private void LoadIconAsync()
        {
            var bitmap = Resource1.champion;
            imageList1.ImageSize = new Size(bitmap.Width, bitmap.Width);
            var count = bitmap.Height / bitmap.Width;
            for (int i = 0; i < count; i++)
            {
                Rectangle rectangle = new Rectangle(0, i * bitmap.Width, bitmap.Width, bitmap.Width);
                Bitmap newBitmap = new Bitmap(bitmap.Width,bitmap.Width);
                using(Graphics g=Graphics.FromImage(newBitmap))
                {
                    g.DrawImage(bitmap,0,0,rectangle,GraphicsUnit.Pixel);
                    imageList1.Images.Add(newBitmap);
                }
                
            }
        }

        private async Task GetChampionPage(string url)
        {

            StartLoading();
            var table = await Task.Run(() => Crawler.GetCounterChampions(url, imageList1.Images));
            StopLoading();
            tabControl1.Visible = false;
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill
            };
            Form2 form = new Form2(table)
            {
                MdiParent = this,
                Parent = panel,
                Dock = DockStyle.Fill
            };
            Controls.Add(panel);
            form.Show();
        }

        public void ReturnHome(Control control)
        {
            Controls.Remove(control);
            tabControl1.Visible = true;
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private async Task listViewItemClicked(ListView listView)
        {
            if (listView.SelectedIndices.Count > 0)
            {
                var champion = listView.SelectedItems[0].Tag as Champion;
                await GetChampionPage(champion.Url);
            }
            listView.SelectedItems.Clear();
        }

        private async void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            await listViewItemClicked(listView1);
        }


        private async void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            await listViewItemClicked(listView2);
        }

        private async void listView3_SelectedIndexChanged(object sender, EventArgs e)
        {
            await listViewItemClicked(listView3);
        }

        private async void listView4_SelectedIndexChanged(object sender, EventArgs e)
        {
            await listViewItemClicked(listView4);
        }

        private async void listView5_SelectedIndexChanged(object sender, EventArgs e)
        {
            await listViewItemClicked(listView5);
        }
    }
}
