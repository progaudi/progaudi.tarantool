using System.Linq;
using System.Threading.Tasks;
using dotnet.Models;
using Microsoft.AspNetCore.Mvc;
using ProGaudi.Tarantool.Client;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;

namespace dotnet.Controllers
{
    public class HomeController : Controller
    {
        private readonly Box _box;
        private readonly Space _space;
        private readonly Index _primaryIndex;
        private readonly Index _secondaryIndex;

        public HomeController(Box box)
        {
            this._box = box;

            var result = this.Initialize().GetAwaiter().GetResult();
            this._space = result.Item1;
            this._primaryIndex = result.Item2;
            this._secondaryIndex = result.Item3;
        }

        private async Task<TarantoolTuple<Space, Index, Index>> Initialize()
        {
            var schema = this._box.GetSchema();

            var space = await schema.GetSpace("some_space");
            var primaryIndex = await space.GetIndex("primary");
            var index = await space.GetIndex("some_secondary_index");

            return TarantoolTuple.Create(space, primaryIndex, index);
        }

        public async Task<ViewResult> Index()
        {
            var allDogs = await this._primaryIndex.Select<TarantoolTuple<long>, TarantoolTuple<long, string, long>>(TarantoolTuple.Create(-1L), new SelectOptions { Iterator = Iterator.All });
            var seniorDogs = await this._secondaryIndex.Select<TarantoolTuple<long>, TarantoolTuple<long, string, long>>(TarantoolTuple.Create(5L), new SelectOptions { Iterator = Iterator.Ge });
            var juniorDogs = await this._secondaryIndex.Select<TarantoolTuple<long>, TarantoolTuple<long, string, long>>(TarantoolTuple.Create(5L), new SelectOptions { Iterator = Iterator.Le });

            return View(new []
            {
                allDogs.Data.Select(x => new Dog(x)).ToArray(),
                seniorDogs.Data.Select(x => new Dog(x)).ToArray(),
                juniorDogs.Data.Select(x => new Dog(x)).ToArray()
            });
        }
    }
}
