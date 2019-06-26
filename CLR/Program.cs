using System;
using CLRLogger;
using System.Runtime.InteropServices;

namespace CLR
{
    class Program
    {
        [DllImport("F:\\Workspace\\ArtasticWeb\\CLR\\bin\\passwordencoder.dll")]
        extern static string base64_encode(char[]  c, int len);

        static void Main(string[] args)
        {


            Console.WriteLine("Hello World!");
            string a = new string("wad");
            char[] inputstr = a.ToCharArray();
            string encodedstr = base64_encode(inputstr, 3);
            Console.WriteLine(encodedstr);
            Console.ReadKey();
        }
    }
}
