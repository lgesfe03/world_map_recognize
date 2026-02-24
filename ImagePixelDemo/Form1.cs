using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ImagePixelDemo
{
    public class Form1 : Form
    {
        int answer_index;
        int answer;
        List<int> cards_options;
        PictureBox pictureBox;
        Button paintButton;
        Button button_option_A;
        Button button_option_B;
        Button button_option_C;
        Label label_quiz;
        Label label_result;

        Bitmap bitmap;
        string[] nation_names = new string[] { "Argentina", "Australia", "Belarus", "Belgium", "Bolivia", "Brazil", "Canada" };
        int[] nation_xy = new int[] { 25, 25, 50, 50, 75, 75, 100, 100, 125, 125, 150, 150, 175, 175, 200, 200 };
        int options_number = 3;

        public Form1()
        {
            InitUI();
            LoadImage();
            shuffle_quiz();
            PaintQuizArea(answer_index);
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

            paintButton = new Button()
            {
                Text = "dye (100,100)",
                Dock = DockStyle.Bottom,
                Height = 40
            };
            paintButton.Click += PaintButton_Click;

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
                Text = "Result: ",
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


            Controls.Add(pictureBox);
            // Controls.Add(paintButton);
            Controls.Add(label_quiz);
            Controls.Add(label_result);
            Controls.Add(button_option_A);
            Controls.Add(button_option_B);
            Controls.Add(button_option_C);
        }

        void LoadImage()
        {
            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "D:\\github\\world_map_recognize\\ImagePixelDemo\\image\\", "a.jpg");

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

        void PaintButton_Click(object sender, EventArgs e)
        {
            PaintArea(100, 100, 50, 50);
            pictureBox.Refresh();
        }
        void OptionButton_ClickA(object sender, EventArgs e)
        {   
            int choose_option_index = 0;
            compare_and_update_result(choose_option_index);
        }
        void OptionButton_ClickB(object sender, EventArgs e)
        {
            int choose_option_index = 1;
            compare_and_update_result(choose_option_index);
        }
        void OptionButton_ClickC(object sender, EventArgs e)
        {
            int choose_option_index = 2;
            compare_and_update_result(choose_option_index);
        }
        void PaintQuizArea(int answer_index)
        {
            PaintArea(nation_xy[answer_index * 2], nation_xy[answer_index * 2 + 1], 25, 25);
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
            answer_index = random.Next(0, options_number);
            answer = cards_options[answer_index];

            update_quiz($"where is number:{answer} ?");
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
                    button_option_A.Text = $"{console_index}.{nation_names[card]}";
                }
                else if (console_index == 2)
                {
                    button_option_B.Text = $"{console_index}.{nation_names[card]}";
                }
                else if (console_index == 3)
                {
                    button_option_C.Text = $"{console_index}.{nation_names[card]}";
                }
                
            }
        }
        void compare_and_update_result(int choose_option_index)
        {
            if(compare_answer_is_true(choose_option_index))
            {
                update_result($"Correct! {nation_names[cards_options[choose_option_index]]}({cards_options[choose_option_index]})");
            }else
            {
                update_result($"False! not {nation_names[cards_options[choose_option_index]]}({cards_options[choose_option_index]})");
            }
        }
        bool compare_answer_is_true(int choose_option_index)
        {
            Console.WriteLine($"You choose: [{choose_option_index + 1}]  {nation_names[cards_options[choose_option_index]]}({cards_options[choose_option_index]}) vs answer: {nation_names[cards_options[answer_index]]}({cards_options[answer_index]})");
            if (cards_options[choose_option_index] == cards_options[answer_index])
            {
                Console.WriteLine($"Correct! {nation_names[cards_options[choose_option_index]]}({cards_options[choose_option_index]})");
                return true;
            }
            else
            {
                Console.WriteLine($"False! not {nation_names[cards_options[choose_option_index]]}({cards_options[choose_option_index]})");
                return false;
            }

        }
    }
}