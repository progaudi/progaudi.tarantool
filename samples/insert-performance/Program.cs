using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client;
using ProGaudi.Tarantool.Client.Model;

namespace Tarantool.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var box = Box.Connect("localhost:3301").GetAwaiter().GetResult())
            {
                var schema = box.GetSchema();
                var space = schema["pivot"];
                var lst = new Task[1000];
                var sw = Stopwatch.StartNew();
                for (var i = 0; i < 1_000_000; i++)
                {
                    lst[i % 1000] = space.Insert((i, (i, i), i));
                    
                    if (i % 1000 == 999)
                    {
                        Task.WhenAll(lst);
                    }

                    if (i % 10000 == 9999)
                    {
                        Console.Write("*");
                    }
                }
                sw.Stop();

                Console.WriteLine();
                Console.WriteLine(sw.ElapsedMilliseconds);
            }
        }
    }
}
