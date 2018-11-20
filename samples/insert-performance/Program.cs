using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client;
using ProGaudi.Tarantool.Client.Model;

namespace Tarantool.Test
{
    class Program
    {
        static void Main()
        {
            var log = new TextWriterLog(Console.Out);
//            var options = new ClientOptions(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "localhost:3301" : "tarantool_1_8:3301");
            var options = new ClientOptions(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "localhost:3301" : "tarantool_1_8:3301", log);
            var sw = new Stopwatch();
            try
            {
                using (var box = new Box(options))
                {
                    box.Connect().GetAwaiter().GetResult();
                    var space = box.Schema["pivot"];
                    var lst = new Task[1000];
                    sw.Start();
                    for (var i = 0; i < 1_000_000; i++)
                    {
                        lst[i % 1000] = space.Insert((i, (i, i), i));

                        if (i % 1000 == 999)
                        {
                            Task.WaitAll(lst);
                            //return;
                        }

                        if (i % 10000 == 9999)
                        {
                            Console.Write("*");
                            if (i % 100000 == 99999)
                            {
                                Console.WriteLine();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                sw.Stop();

                Console.WriteLine();
                Console.WriteLine(sw.ElapsedMilliseconds);
            }
        }
    }
}
