using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMM_Server
{
    class Program
    {


        static void Main(string[] args)
        {
            GlobalBase gb = new GlobalBase();
            //тест команд сервера
            while (true)
            {
                Console.WriteLine(gb.Command(Console.ReadLine()));
            }
        }
    }
}
