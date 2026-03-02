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
        List<CityData> List_cities;
        int options_number = 3;

        public Form1()
        {
            InitVariable();
            InitJson();
            InitUI();
            LoadImage();
            markNumberOnImage();
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
            List_cities = JsonSerializer.Deserialize<List<CityData>>(jsonString);
            Console.WriteLine($"{"List_cities.Count:"}{List_cities.Count()})");

            foreach (var city in List_cities)
            {
                Console.WriteLine($"Name: {city.CityName}, Latitude: {city.Latitude}, Longitude: {city.Longitude}");
            }
        }
        Button CreateButton(string text, EventHandler click)
        {
            Button btn = new Button()
            {
                Text = text,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btn.Click += click;
            return btn;
        }
        void InitUI()
        {
            this.Text = "Image Pixel Demo";
            this.Width = 800;
            this.Height = 600;

            // ===== Main Layout =====
            TableLayoutPanel mainLayout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                RowCount = 1,
                ColumnCount = 2,
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));// image
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));// right part with quest, button, result + score

            TableLayoutPanel rightLayout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1
                // Padding = new Padding(10)
            };
            // rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40)); // quest
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 34));
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33));

            // ===== PictureBox =====
            pictureBox = new PictureBox()
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle
            };
            // ===== request =====
            label_quiz = new Label()
            {
                Text = "Which option is target?",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };

            // ===== button（1x4）=====
            TableLayoutPanel buttonLayout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                RowCount = 4,
                // Padding = new Padding(20)
            };

            buttonLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            buttonLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            buttonLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            buttonLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25));

            button_option_A = CreateButton("Option A", OptionButton_ClickA);
            button_option_B = CreateButton("Option B", OptionButton_ClickB);
            button_option_C = CreateButton("Option C", OptionButton_ClickC);
            button_option_Refresh = CreateButton("Refresh", OptionButton_Refresh);

            buttonLayout.Controls.Add(button_option_A, 0, 0);
            buttonLayout.Controls.Add(button_option_B, 0, 1);
            buttonLayout.Controls.Add(button_option_C, 0, 2);
            buttonLayout.Controls.Add(button_option_Refresh, 0, 3);

            // ===== Bottom =====
            TableLayoutPanel bottomPanel = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
            };
            bottomPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            bottomPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

            label_result = new Label()
            {
                Text = "",
                Dock = DockStyle.Fill
            };

            label_score = new Label()
            {
                Text = $"Score: {history_quest.Count}",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Dock = DockStyle.Fill
            };

            bottomPanel.Controls.Add(label_result, 0, 0);
            bottomPanel.Controls.Add(label_score, 0, 1);

            rightLayout.Controls.Add(label_quiz, 0, 0);
            rightLayout.Controls.Add(buttonLayout, 0, 1);
            rightLayout.Controls.Add(bottomPanel, 0, 2);

            // ===== add main Layout =====
            mainLayout.Controls.Add(pictureBox, 0, 0);
            mainLayout.Controls.Add(rightLayout, 1, 0);

            this.Controls.Add(mainLayout);
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
        void markNumberOnImage()
        {
            int index = 0 + 1;
            foreach (var city in List_cities)
            {
                // 2. Get a Graphics object from the Bitmap
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    // Optional: Fill the background if needed
                    // g.Clear(Color.White); 

                    // 3. Define the Font and Brush for the text
                    using (Font myFont = new Font("Arial", 6, FontStyle.Regular))
                    using (Brush myBrush = new SolidBrush(Color.Black))
                    {
                        // Set rendering hint for smooth text
                        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                        // 4. Draw the string onto the Bitmap
                        // Parameters: text, font, brush, x-coordinate, y-coordinate
                        g.DrawString(index.ToString(), myFont, myBrush, new Point(city.Latitude, city.Longitude));
                    }
                }
                index++;
            }
            pictureBox.Refresh();
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
            markNumberOnImage();
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
            // PaintArea(List_cities[index].Latitude, List_cities[index].Longitude, 25, 25);//old method
            // Apply the color fill to white pixels
            PaintWhiteConnectedArea(List_cities[index].Latitude, List_cities[index].Longitude);
            // PaintNumberArea(List_cities[index].Latitude, List_cities[index].Longitude, 25, 25);
            pictureBox.Refresh();
        }
        bool IsWhite(Color c)
        {
            // Tolerate a slight error, avoid JPG compression issues.
            return c.R > 240 && c.G > 240 && c.B > 240;
        }
        void PaintWhiteConnectedArea(int cx, int cy)
        {
            if (cx < 0 || cy < 0 || cx >= bitmap.Width || cy >= bitmap.Height)
                return;

            Color startColor = bitmap.GetPixel(cx, cy);
            if (!IsWhite(startColor))
            {
                MessageBox.Show($"Not white cx:{cx}, cy:{cy}");
                return; // If the starting point is not white, then skip it.
            }

            float alpha = 0.4f;
            Color overlay = Color.Yellow;

            bool[,] visited = new bool[bitmap.Width, bitmap.Height];
            Stack<Point> stack = new Stack<Point>();
            stack.Push(new Point(cx, cy));

            while (stack.Count > 0)
            {
                Point p = stack.Pop();

                if (p.X < 0 || p.Y < 0 || p.X >= bitmap.Width || p.Y >= bitmap.Height)
                    continue;

                if (visited[p.X, p.Y])
                    continue;

                Color src = bitmap.GetPixel(p.X, p.Y);
                if (!IsWhite(src))
                    continue;

                visited[p.X, p.Y] = true;

                // alpha blend
                int r = (int)(src.R * (1 - alpha) + overlay.R * alpha);
                int g = (int)(src.G * (1 - alpha) + overlay.G * alpha);
                int b = (int)(src.B * (1 - alpha) + overlay.B * alpha);

                bitmap.SetPixel(p.X, p.Y, Color.FromArgb(src.A, r, g, b));

                // Spread in four directions
                stack.Push(new Point(p.X + 1, p.Y));
                stack.Push(new Point(p.X - 1, p.Y));
                stack.Push(new Point(p.X, p.Y + 1));
                stack.Push(new Point(p.X, p.Y - 1));
            }
        }
        void PaintNumberArea(int cx, int cy, int w, int h)
        {
            int sx = cx - w / 4;
            int sy = cy - h / 4;

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
                int temp_new_quest = random.Next(0, List_cities.Count);
                if (!history_quest.Contains(temp_new_quest))
                {
                    Console.WriteLine($"temp_new_quest :{temp_new_quest}");
                    quest_this_round = temp_new_quest;
                    break;
                }
                else if (history_quest.Count >= List_cities.Count)
                {
                    Console.WriteLine($"all quest had been shuffled :{List_cities.Count}");
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
                int value = random.Next(0, List_cities.Count);
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
                Console.WriteLine($"{console_index}.{List_cities[card].CityName}({card})");
                if (console_index == 1)
                {
                    button_option_A.Text = $"{"A"}.{List_cities[card].CityName}";
                }
                else if (console_index == 2)
                {
                    button_option_B.Text = $"{"B"}.{List_cities[card].CityName}";
                }
                else if (console_index == 3)
                {
                    button_option_C.Text = $"{"C"}.{List_cities[card].CityName}";
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
                update_result($"Correct! {List_cities[choose_answer].CityName}");
                return true;
            }
            else
            {
                update_result($"False! not {List_cities[choose_answer].CityName}({choose_answer + 1}), answer is {List_cities[quest_this_round].CityName}({quest_this_round + 1})");
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
                if (score % List_cities.Count == 0)
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