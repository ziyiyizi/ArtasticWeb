using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLRLogger;

namespace CLRF
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
        }

        public static void Test()
        {
            Console.WriteLine("Hello World!");
            Class1 cls = new Class1();
            Console.WriteLine(cls.log(1));
            Console.Read();
        }
    }
}
