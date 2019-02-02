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
        static void Main(string[] args)
        {
            var log = new TextWriterLog(Console.Out);
            var options = new ClientOptions(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "localhost:3301" : "tarantool_1_8:3301");
//            options = new ClientOptions(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "localhost:3301" : "tarantool_1_8:3301", log);

            options.ConnectionOptions.UsePipelines = args.Length > 0;
//            options.ConnectionOptions.UsePipelines = true;
            options.ConnectionOptions.PingCheckInterval = 0;
            var sw = new Stopwatch();
            try
            {
                using (var box = new Box(options))
                {
                    box.Connect().GetAwaiter().GetResult();
                    var space = box.Schema["pivot"];
                    const int batchSize = 10;
                    var lst = new Task[batchSize];
                    sw.Start();
                    for (var i = 0; i < 1_000_000; i++)
                    {
                        lst[i % batchSize] = space.Insert((i, (i, i), i));

                        if (i % batchSize == batchSize - 1)
                        {
                            Task.WaitAll(lst);
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
