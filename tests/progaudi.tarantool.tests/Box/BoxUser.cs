using ProGaudi.Tarantool.Client.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProGaudi.Tarantool.Client.Tests.Box
{
    public class BoxUser : IDisposable
    {
        private readonly Client.Box _box;

        public BoxUser(Client.Box box)
        {
            _box = box;

            Initialize().GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            _box?.Dispose();
        }

        public async Task<double> TestMethod()
        {
            var result = await _box.Call_1_6<TarantoolTuple<double>, TarantoolTuple<double>>("math.sqrt", TarantoolTuple.Create(1.3));
            return result.Data.Single().Item1;
        }

        private async Task Initialize()
        {
            await _box.Connect();
        }
    }
}
