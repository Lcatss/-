using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace 对位英雄查询
{
    class LoadingPictureBox : PictureBox
    {
        private readonly List<Bitmap> loadingGIfs = new List<Bitmap>()
        {
            Resource1.Loading,
            Resource1.Loading2,
            Resource1.Loading3
        };

        public LoadingPictureBox()
        {
            Random random = new Random();
            var index = random.Next(loadingGIfs.Count);
            Image = loadingGIfs[index];
            SizeMode = PictureBoxSizeMode.CenterImage;
            Dock = DockStyle.Fill;
        }
    }
}
