// namespace gui;

// static class Program
// {
//     /// <summary>
//     ///  The main entry point for the application.
//     /// </summary>
//     [STAThread]
//     static void Main()
//     {
//         // To customize application configuration such as set high DPI settings or default font,
//         // see https://aka.ms/applicationconfiguration.
//         ApplicationConfiguration.Initialize();
//         Application.Run(new Form1());
//     }    
// }
using System;
using System.Windows.Forms;
using System.Drawing;

namespace WinFormsApp
{
    class Program
    {
        [STAThread] // Required for WinForms
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form mainForm = new Form();
            mainForm.Text = "C# GUI Sample (VS Code)";
            mainForm.Width = 400;
            mainForm.Height = 300;

            Label label = new Label();
            label.Text = "Hello World from VS Code!";
            label.Location = new Point(100, 100);
            label.AutoSize = true;

            Button button = new Button();
            button.Text = "Click Me";
            button.Location = new Point(100, 150);
            button.Click += (sender, e) =>
            {
                MessageBox.Show("Button clicked!");
            };

            mainForm.Controls.Add(label);
            mainForm.Controls.Add(button);

            Application.Run(mainForm);
        }
    }
}