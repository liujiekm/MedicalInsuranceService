using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string command = "dick";

            Modify(command);

            Console.WriteLine(command);
        }


        public static void Modify(String command)
        {
            command = "jirk";
        }
    }
}
