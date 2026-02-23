using System;
using System.Windows.Forms;
using System.Drawing;

namespace WinFormsApp
{
    class Program
    {
        [STAThread] // Required for WinForms
        void freeze_button_option_all()
        {
            // button_option_A.Enabled = false;
            // button_option_B.Enabled = false;
            // button_option_C.Enabled = false;
        }
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form mainForm = new Form();
            mainForm.Text = "C# GUI Sample (VS Code)";
            mainForm.Width = 400;
            mainForm.Height = 400;

            Label label = new Label();
            label.Text = "Which option is target?";
            label.Location = new Point(200, 50);
            label.AutoSize = true;

            Label label_result = new Label();
            label_result.Text = "Result: ";
            label_result.Location = new Point(200, 250);
            label_result.AutoSize = true;

            Button button_option_A = new Button();
            button_option_A.Text = "option A";
            button_option_A.Location = new Point(200, 100);
            button_option_A.Click += (sender, e) =>
            {
                // MessageBox.Show("option A clicked!");
                label_result.Text = "Result: option A";
            };

            Button button_option_B = new Button();
            button_option_B.Text = "option B";
            button_option_B.Location = new Point(200, 150);
            button_option_B.Click += (sender, e) =>
            {
                label_result.Text = "Result: option B";
                // freeze_button_option_all();
            };

            Button button_option_C = new Button();
            button_option_C.Text = "option C";
            button_option_C.Location = new Point(200, 200);
            button_option_C.Click += (sender, e) =>
            {
                label_result.Text = "Result: option C";
            };

            mainForm.Controls.Add(label);
            mainForm.Controls.Add(label_result);
            mainForm.Controls.Add(button_option_A);
            mainForm.Controls.Add(button_option_B);
            mainForm.Controls.Add(button_option_C);

            Application.Run(mainForm);
        }
    }
}