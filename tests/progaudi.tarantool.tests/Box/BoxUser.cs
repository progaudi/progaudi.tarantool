using ProGaudi.Tarantool.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProGaudi.Tarantool.Client.Tests.Box
{
    public class BoxUser : IDisposable
    {
        private readonly Client.Box _box;

        public BoxUser(Client.Box box)
        {
            this._box = box;

            Initialize().GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            this._box?.Dispose();
        }

        public async Task<double> TestMethod()
        {
            var result = await this._box.Call_1_6<TarantoolTuple<double>, TarantoolTuple<double>>("math.sqrt", TarantoolTuple.Create(1.3));
            return result.Data.Single().Item1;
        }

        private async Task Initialize()
        {
            await this._box.Connect();
        }
    }
}
