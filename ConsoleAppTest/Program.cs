using ConcurrentList;

namespace ConsoleAppTest
{
    internal class Program
    {
        static List<int> ints = new List<int>();

        static void Main(string[] args)
        {

            Task.Run(ChangeList);

            /*
            //++
            foreach (int el in ints.ToArray())
            {
                Task.Delay(500).Wait();
                Console.WriteLine(el);
            }
            */
            Console.WriteLine(new string('-', 25));

            foreach (int el in ints)
            {
                Task.Delay(500).Wait();
                Console.WriteLine(el);
            }

            /*
            foreach (int el in ints.AsEnumerable())
            {
                Task.Delay(500).Wait();
                Console.WriteLine(el);
            }
            */

            /*
            //+
            foreach (int el in ints.Select(i => { Console.WriteLine($"Select: {i}"); return i; }).ToArray())
            {
                Task.Delay(500).Wait();
                Console.WriteLine(el);
            }
            */

            /*
            foreach (int el in ints.Select(i => { Console.WriteLine($"Select: {i}"); return i; }))
            {
                Task.Delay(500).Wait();
                Console.WriteLine(el);
            }
            */

            /*
            foreach (int el in ints.AsReadOnly())
            {
                Task.Delay(500).Wait();
                Console.WriteLine(el);
            }
            */



            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

        public static void ChangeList()
        {
            Console.WriteLine("ChangeList");
            Task.Delay(2500).Wait();
            ints.Add(0);
        }
    }
}