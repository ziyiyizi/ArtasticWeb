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
        [System.Runtime.InteropServices.DllImport("F:\\Workspace\\ArtasticWeb\\CLR\\bin\\passwordencoder.dll")]
        extern static string base64_encode(char[] c, int len);

        static void Main(string[] args)
        {
            //测试调用c++/cli
                Console.WriteLine("开始日志记录功能!");
                Class1 cls = new Class1();
                Console.WriteLine(cls.log(1));
                Console.Read();

        }


        

        public static void TestEncode()
        {
            //测试调用win32 dll
            Console.WriteLine("Hello World!");
            string a = "example_password";
            char[] inputstr = a.ToCharArray();
            string encodedstr = base64_encode(inputstr, 3);
            Console.WriteLine(encodedstr);
            Console.ReadKey();
        }

        public static void TestCom()
        {
            //测试Com组件的ID生成。
            TEstLib.ATLSimpleObject o = new TEstLib.ATLSimpleObject();
            Console.WriteLine(o.Add(2, 2));
            Console.Read();
            Console.WriteLine(o.GenerateID(10001));
        }
    }
}
