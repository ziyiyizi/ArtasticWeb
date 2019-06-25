using System;
using CLRLogger;

namespace CLR
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Class1 cls = new Class1();
            Console.WriteLine(cls.log(1));
            Console.Read();
        }
    }
}
