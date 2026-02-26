using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ImagePixelDemo
{
    public class Form1 : Form
    {
        int answer;
        List<int> cards_options;
        PictureBox pictureBox;
        Button button_option_A;
        Button button_option_B;
        Button button_option_C;
        Button button_option_Refresh;
        Label label_quiz;
        Label label_result;

        Bitmap bitmap;
        string[] nation_names = new string[] { "Keelung", "Taipei", "New Taipei", "Taoyuan" };
        int[] nation_xy = new int[] { 273, 42, 254, 48, 248, 68, 216, 60 };
        int options_number = 3;

        public Form1()
        {
            InitUI();
            LoadImage();
            shuffle_quiz();
            PaintQuizArea(answer);
        }
        void InitUI()
        {
            this.Text = "Image Pixel Demo";
            this.Width = 800;
            this.Height = 600;

            pictureBox = new PictureBox()
            {
                Dock = DockStyle.Top,
                Height = 400,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            label_quiz = new Label()
            {
                Text = "Which option is target?",
                // Location = new Point(600, 200),
                Dock = DockStyle.Right,
                Height = 40,
                // AutoSize = true
            };

            label_result = new Label()
            {
                Text = "",
                Dock = DockStyle.Bottom,
                Height = 40
            };

            button_option_A = new Button()
            {
                Text = "option A",
                Dock = DockStyle.Right,
                Height = 40
            };
            button_option_A.Click += OptionButton_ClickA;

            button_option_B = new Button()
            {
                Text = "option B",
                Dock = DockStyle.Right,
                Height = 40
            };
            button_option_B.Click += OptionButton_ClickB;

            button_option_C = new Button()
            {
                Text = "option C",
                Dock = DockStyle.Right,
                Height = 40
            };
            button_option_C.Click += OptionButton_ClickC;

            button_option_Refresh = new Button()
            {
                Text = "Refresh",
                Dock = DockStyle.Right,
                Height = 40
            };
            button_option_Refresh.Click += OptionButton_Refresh;

            Controls.Add(pictureBox);
            Controls.Add(label_quiz);
            Controls.Add(label_result);
            Controls.Add(button_option_A);
            Controls.Add(button_option_B);
            Controls.Add(button_option_C);
            Controls.Add(button_option_Refresh);
        }

        void LoadImage()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string relativePath = @"..\\..\\..\\image\\taiwan.jpg";

            string combinedPath = Path.Combine(basePath, relativePath);

            string imagePath = Path.GetFullPath(combinedPath);// Resolve the combined path to an absolute path

            // MessageBox.Show(imagePath);

            bitmap = new Bitmap(imagePath);
            pictureBox.Image = bitmap;
        }
        void update_quiz(string string_in)
        {
            label_quiz.Text = string_in;
        }
        void update_result(string string_in)
        {
            label_result.Text = string_in;
        }
        void OptionButton_ClickA(object sender, EventArgs e)
        {
            switch_all_option_button(false);
            int choose_option_index = 0;
            compare_and_update_result(choose_option_index);
        }
        void OptionButton_ClickB(object sender, EventArgs e)
        {
            switch_all_option_button(false);
            int choose_option_index = 1;
            compare_and_update_result(choose_option_index);
        }
        void OptionButton_ClickC(object sender, EventArgs e)
        {
            switch_all_option_button(false);
            int choose_option_index = 2;
            compare_and_update_result(choose_option_index);
        }
        void OptionButton_Refresh(object sender, EventArgs e)
        {
            LoadImage();
            shuffle_quiz();
            PaintQuizArea(answer);
            switch_all_option_button(true);
        }
        void switch_all_option_button(bool onoff)
        {
            button_option_A.Enabled = onoff;
            button_option_B.Enabled = onoff;
            button_option_C.Enabled = onoff;
        }
        void PaintQuizArea(int index)
        {
            PaintArea(nation_xy[index * 2], nation_xy[index * 2 + 1], 25, 25);
            pictureBox.Refresh();
        }
        void PaintArea(int cx, int cy, int w, int h)
        {
            int sx = cx - w / 2;
            int sy = cy - h / 2;

            float alpha = 0.4f;   // 40% transparency 

            for (int x = sx; x < sx + w; x++)
            {
                for (int y = sy; y < sy + h; y++)
                {
                    if (x < 0 || y < 0 || x >= bitmap.Width || y >= bitmap.Height)
                        continue;

                    Color src = bitmap.GetPixel(x, y);
                    Color overlay = Color.Yellow;

                    int r = (int)(src.R * (1 - alpha) + overlay.R * alpha);
                    int g = (int)(src.G * (1 - alpha) + overlay.G * alpha);
                    int b = (int)(src.B * (1 - alpha) + overlay.B * alpha);

                    bitmap.SetPixel(x, y, Color.FromArgb(src.A, r, g, b));
                }
            }
        }
        void shuffle_quiz()
        {
            Random random = new Random();
            cards_options = new List<int>();
            while (true)
            {
                if (cards_options.Count == options_number)
                {
                    break;
                }
                int value = random.Next(0, nation_names.Length);
                if (!cards_options.Contains(value))
                {
                    cards_options.Add(value);
                }
            }
            foreach (var card in cards_options)
            {
                Console.WriteLine($"{"cards_options:"}{card})");
            }
            int answer_index = random.Next(0, options_number);
            answer = cards_options[answer_index];

            Console.WriteLine($"where is number:{answer} ?, {"answer_index:"}{answer_index}");
            update_quiz($"where is number:{answer + 1} ?");
            fill_option(cards_options);
        }
        void fill_option(List<int> cards_options)
        {
            int console_index = 0;
            foreach (var card in cards_options)
            {
                console_index++;
                Console.WriteLine($"{console_index}.{nation_names[card]}({card})");
                if (console_index == 1)
                {
                    button_option_A.Text = $"{"A"}.{nation_names[card]}";
                }
                else if (console_index == 2)
                {
                    button_option_B.Text = $"{"B"}.{nation_names[card]}";
                }
                else if (console_index == 3)
                {
                    button_option_C.Text = $"{"C"}.{nation_names[card]}";
                }

            }
        }
        void compare_and_update_result(int choose_option_index)
        {
            int choose_answer = cards_options[choose_option_index];
            if (choose_answer == answer)
            {
                update_result($"Correct! {nation_names[choose_answer]}({choose_answer})");
            }
            else
            {
                update_result($"False! not {nation_names[choose_answer]}({choose_answer}), answer is {nation_names[answer]}({answer})");
            }
        }
    }
}