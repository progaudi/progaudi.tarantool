using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet.Models;
using Microsoft.AspNetCore.Mvc;
using Tarantool.Client;
using Tarantool.Client.Model;
using Tarantool.Client.Model.Enums;

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

        private async Task<Tarantool.Client.Model.Tuple<Space, Index, Index>> Initialize()
        {
            var schema = this._box.GetSchema();

            var space = await schema.GetSpace("some_space");
            var primaryIndex = await space.GetIndex("primary");
            var index = await space.GetIndex("some_secondary_index");

            return Tarantool.Client.Model.Tuple.Create(space, primaryIndex, index);
        }

        public async Task<ViewResult> Index()
        {
            var primaryData = await this._primaryIndex.Select<Tuple<long>, Tuple<long, string, long>>(Tuple.Create(-1L), new SelectOptions { Iterator = Iterator.All });
            var secondaryData = await this._secondaryIndex.Select<Tuple<long>, Tuple<long, string, long>>(Tuple.Create(5L), new SelectOptions { Iterator = Iterator.Ge });

            return View(new TestData
            {
                AllDogs = primaryData.Data.Select(x => new Dog(x)).ToArray(),
                DogsOlder5Years = secondaryData.Data.Select(x => new Dog(x)).ToArray()
            });
        }
    }
}
