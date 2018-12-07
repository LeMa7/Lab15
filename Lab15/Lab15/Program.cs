using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab15
{
    

    class Program
    {
        

        public static void getProcessesInfo()
        {
            Console.WriteLine("Процессы:");
            foreach (Process process in Process.GetProcesses())
            {
                Console.WriteLine("Имя: " + process.ProcessName + "\tId:" + process.Id + "\tВыделенный объем памяти:" + process.VirtualMemorySize64);
            }
            Console.WriteLine();
        }

        public static void getDomainInfo()
        {
            Console.WriteLine("Домен:");
            AppDomain domain = AppDomain.CurrentDomain;
            Console.WriteLine("Имя:"+domain.FriendlyName+"\tКонфигурация:"+domain.SetupInformation);
            Console.WriteLine("Сборки домена:");
            Assembly[] assemblies = domain.GetAssemblies();
            foreach (Assembly asm in assemblies)
            {
                Console.WriteLine(asm.GetName().Name);
            }
            Console.WriteLine();
            AppDomain domain2 = AppDomain.CreateDomain("domain2");
            domain2.Load(new AssemblyName("mscorlib"));
            AppDomain.Unload(domain2);
        }

        public static void OneToN(object x)
        {
            StreamWriter sw = new StreamWriter("thread.txt");
            int n = (int)x;
            Console.WriteLine();
            for(int i = 1; i < n+1; i++)
            {
                sw.WriteLine(i);
                Console.WriteLine(i);
                Thread.Sleep(300);
            }
            sw.Close();
            Console.WriteLine();
        }

        static object locker = "null";

        public static void Even(object x)
        {
            lock (locker) { 
                StreamWriter sw = new StreamWriter("OddAndEven.txt",true);
                int n = (int)x;
                int i1 = 0;
                while (i1 <= n) { 
                    sw.WriteLine(i1);
                    Console.WriteLine("Четное: " + i1);
                    i1 += 2;
                    Thread.Sleep(500);
                }
                sw.Close();
            }
        }

        public static void Odd(object x)
        {
            lock (locker) { 
                StreamWriter sw = new StreamWriter("OddAndEven.txt",true);
                int n = (int)x;
                int i2 = 1;
                while (i2 <= n) { 
                    sw.WriteLine(i2);
                    Console.WriteLine("Нечетное: " + i2);
                    i2 += 2;
                    Thread.Sleep(1000);
                }
                sw.Close();
            }
        }

        static int q = 0;
        public static void Function(object obj)
        { 
                Console.WriteLine("Прошло времени: "+q+" секунд");
            q++;
        }

        static void Main(string[] args)
        {
            getProcessesInfo();
            getDomainInfo();
            ParameterizedThreadStart theDelegate0 = new ParameterizedThreadStart(OneToN);
            ParameterizedThreadStart theDelegate = new ParameterizedThreadStart(Even);
            ParameterizedThreadStart theDelegate2 = new ParameterizedThreadStart(Odd);
            Thread evenThread1 = new Thread(theDelegate);
            Thread oddThread1 = new Thread(theDelegate2);
            Thread threadOnetoN = new Thread(theDelegate0);
            threadOnetoN.Priority = ThreadPriority.Lowest;
            threadOnetoN.Name = "OneToN";
            Console.WriteLine("Введите число:");
            int n = int.Parse(Console.ReadLine());
            Console.WriteLine("Имя потока: " + threadOnetoN.Name);
            Console.WriteLine("Статус потока: " + threadOnetoN.ThreadState);
            Console.WriteLine("Приоритет потока: " + threadOnetoN.Priority);
            threadOnetoN.Start(n);
            Console.ReadKey();
            evenThread1.Start(n);
            oddThread1.Start(n);
            Console.ReadKey();
            Console.WriteLine();
            TimerCallback tm = new TimerCallback(Function);
            Timer timer = new Timer(tm, null,0, 1000);
            Console.ReadLine();
        }
    }
}