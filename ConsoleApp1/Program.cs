using System;
using System.Collections.Generic;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] nation_names = new string[] { "Argentina", "Australia", "Belarus", "Belgium", "Bolivia", "Brazil", "Canada" };
            int options_number = 3;
            int options_sum = nation_names.Length;
            int console_index = 0;

            Random random = new Random();
            List<int> cards_options = new List<int>();
            while (true)
            {
                if (cards_options.Count == options_number)
                {
                    break;
                }
                int value = random.Next(0, options_sum);
                if (!cards_options.Contains(value))
                {
                    cards_options.Add(value);
                }
            }
            int answer_index = random.Next(0, options_number);
            int answer = cards_options[answer_index];

            Console.WriteLine($"where is number:{answer} ?");
            foreach (var card in cards_options)
            {
                console_index ++;
                Console.WriteLine($"{console_index}.{nation_names[card]}({card})");
            }

            Console.WriteLine("Enter a option:");
            int choose_option_index = int.Parse(Console.ReadLine()) - 1;
            if (choose_option_index >= options_number || choose_option_index < 0)
            {
                Console.WriteLine($"Out of option!  Max:{options_number} ");
            }
            else
            {
                Console.WriteLine($"You choose: [{choose_option_index + 1}]  {nation_names[cards_options[choose_option_index]]}({cards_options[choose_option_index]}) vs answer: {nation_names[cards_options[answer_index]]}({cards_options[answer_index]})");
                if (cards_options[choose_option_index] == cards_options[answer_index])
                {
                    Console.WriteLine($"Correct! {nation_names[cards_options[choose_option_index]]}({cards_options[choose_option_index]})");
                }
                else
                {
                    Console.WriteLine($"False! not {nation_names[cards_options[choose_option_index]]}({cards_options[choose_option_index]})");
                }
            }

        }
    }
}
