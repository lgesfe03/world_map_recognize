using System;
using System.Drawing;
using System.Text.Json;
using System.Windows.Forms;
using System.Collections.Generic;

public class CityData
{
    public string CityName { get; set; }
    public int Latitude { get; set; }
    public int Longitude { get; set; }
    // ... other properties
}

namespace ImagePixelDemo
{
    public class Form1 : Form
    {
        int quest_this_round;
        int score;
        List<int> history_quest;
        List<int> option_list_each_round;
        Random random;
        PictureBox pictureBox;
        Button button_option_A;
        Button button_option_B;
        Button button_option_C;
        Button button_option_Refresh;
        Label label_quiz;
        Label label_score;
        Label label_result;

        Bitmap bitmap;
        List<CityData> cities;
        int options_number = 3;

        public Form1()
        {
            InitVariable();
            InitJson();
            InitUI();
            LoadImage();
            prepare_quest_and_option();
            PaintQuizArea(quest_this_round);
        }
        void InitVariable()
        {
            history_quest = new List<int>();
            option_list_each_round = new List<int>();
            random = new Random();
        }
        void InitJson()
        {
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "json",
                "taiwan_cities.json"
            );
            Console.WriteLine($"{"BaseDirectory:"}{AppDomain.CurrentDomain.BaseDirectory})");
            Console.WriteLine($"{"Image Path:"}{jsonPath})");

            if (!File.Exists(jsonPath))
            {
                MessageBox.Show($"Json not found:\n{jsonPath}");
                return;
            }
            string jsonString = File.ReadAllText(jsonPath);
            cities = JsonSerializer.Deserialize<List<CityData>>(jsonString);
            Console.WriteLine($"{"cities.Count:"}{cities.Count()})");

            foreach (var city in cities)
            {
                Console.WriteLine($"Name: {city.CityName}, Latitude: {city.Latitude}, Longitude: {city.Longitude}");
            }
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

            label_score = new Label()
            {
                Text = $"score:{history_quest.Count}",
                Dock = DockStyle.Bottom,
                Height = 40
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
            Controls.Add(label_score);
            Controls.Add(label_result);
            Controls.Add(button_option_A);
            Controls.Add(button_option_B);
            Controls.Add(button_option_C);
            Controls.Add(button_option_Refresh);
        }

        void LoadImage()
        {

            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "image",
                "taiwan.jpg"
            );
            Console.WriteLine($"{"BaseDirectory:"}{AppDomain.CurrentDomain.BaseDirectory})");
            Console.WriteLine($"{"Image Path:"}{imagePath})");

            if (!File.Exists(imagePath))
            {
                MessageBox.Show($"Image not found:\n{imagePath}");
                return;
            }

            bitmap = new Bitmap(imagePath);
            pictureBox.Image = bitmap;
        }
        void update_quiz(string string_in)
        {
            label_quiz.Text = string_in;
        }
        void update_score()
        {
            label_score.Text = $"score:{score}";
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
            prepare_quest_and_option();
            PaintQuizArea(quest_this_round);
            switch_all_option_button(true);
            update_result("");
            update_score();
        }
        void switch_all_option_button(bool onoff)
        {
            button_option_A.Enabled = onoff;
            button_option_B.Enabled = onoff;
            button_option_C.Enabled = onoff;
        }
        void PaintQuizArea(int index)
        {
            PaintArea(cities[index].Latitude, cities[index].Longitude, 25, 25);
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
        void shuffle_quest_not_existed()
        {
            Console.WriteLine($"history_quest count:{history_quest.Count}");
            while (true)
            {
                int temp_new_quest = random.Next(0, cities.Count);
                if (!history_quest.Contains(temp_new_quest))
                {
                    Console.WriteLine($"temp_new_quest :{temp_new_quest}");
                    quest_this_round = temp_new_quest;
                    break;
                }
                else if (history_quest.Count >= cities.Count)
                {
                    Console.WriteLine($"all quest had been shuffled :{cities.Count}");
                    history_quest.Clear();
                }
            }
        }
        void fill_remain_option()
        {
            option_list_each_round.Clear();
            option_list_each_round.Add(quest_this_round);
            while (true)
            {
                if (option_list_each_round.Count >= options_number)
                {
                    break;
                }
                int value = random.Next(0, cities.Count);
                if (!option_list_each_round.Contains(value))
                {
                    option_list_each_round.Add(value);
                }
            }
            // shuffle option_list_each_round 
            for (int i = option_list_each_round.Count - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                int temp = option_list_each_round[j];
                option_list_each_round[j] = option_list_each_round[i];
                option_list_each_round[i] = temp;
            }
        }
        void fill_option_button(List<int> cards_options)
        {
            int console_index = 0;
            foreach (var card in cards_options)
            {
                console_index++;
                Console.WriteLine($"{console_index}.{cities[card].CityName}({card})");
                if (console_index == 1)
                {
                    button_option_A.Text = $"{"A"}.{cities[card].CityName}";
                }
                else if (console_index == 2)
                {
                    button_option_B.Text = $"{"B"}.{cities[card].CityName}";
                }
                else if (console_index == 3)
                {
                    button_option_C.Text = $"{"C"}.{cities[card].CityName}";
                }
            }
        }
        void prepare_quest_and_option()
        {
            shuffle_quest_not_existed();
            fill_remain_option();
            update_quiz($"where is number:{quest_this_round + 1} ?");
            fill_option_button(option_list_each_round);
        }
        bool IsAnswerCorrect(int choose_option_index)
        {
            int choose_answer = option_list_each_round[choose_option_index];
            if (choose_answer == quest_this_round)
            {
                update_result($"Correct! {cities[choose_answer].CityName}");
                return true;
            }
            else
            {
                update_result($"False! not {cities[choose_answer].CityName}({choose_answer + 1}), answer is {cities[quest_this_round].CityName}({quest_this_round + 1})");
                return false;
            }
        }
        void compare_and_update_result(int choose_option_index)
        {
            bool is_correct = IsAnswerCorrect(choose_option_index);
            if (is_correct)
            {
                history_quest.Add(quest_this_round);
                score++;
                update_score();
                if (score % cities.Count == 0)
                {
                    MessageBox.Show("Congratulate! All request done!");
                }
            }
            else
            {
                update_score();
                score = 0;
                history_quest.Clear();
            }
        }
    }
}