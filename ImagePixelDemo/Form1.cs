using System;
using System.Drawing;
using System.Windows.Forms;

namespace ImagePixelDemo
{
    public class Form1 : Form
    {
        PictureBox pictureBox;
        Button paintButton;
        Bitmap bitmap;

        public Form1()
        {
            InitUI();
            LoadImage();
        }

        void InitUI()
        {
            this.Text = "Image Pixel Demo";
            this.Width = 600;
            this.Height = 500;

            pictureBox = new PictureBox()
            {
                Dock = DockStyle.Top,
                Height = 400,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            paintButton = new Button()
            {
                Text = "染色 (100,100)",
                Dock = DockStyle.Bottom,
                Height = 40
            };
            paintButton.Click += PaintButton_Click;

            Controls.Add(pictureBox);
            Controls.Add(paintButton);
        }

        void LoadImage()
        {
            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "D:\\github\\world_map_recognize\\ImagePixelDemo\\image\\", "a.jpg");

            MessageBox.Show(imagePath);

            bitmap = new Bitmap(imagePath);
            pictureBox.Image = bitmap;
        }

        void PaintButton_Click(object sender, EventArgs e)
        {
            PaintArea(100, 100, 50, 50);
            pictureBox.Refresh();
        }

        void PaintArea(int cx, int cy, int w, int h)
        {
            int sx = cx - w / 2;
            int sy = cy - h / 2;

            for (int x = sx; x < sx + w; x++)
            {
                for (int y = sy; y < sy + h; y++)
                {
                    if (x >= 0 && y >= 0 &&
                        x < bitmap.Width && y < bitmap.Height)
                    {
                        bitmap.SetPixel(x, y, Color.Yellow);
                    }
                }
            }
        }
    }
}