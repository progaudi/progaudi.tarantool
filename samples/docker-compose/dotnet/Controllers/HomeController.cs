using System;
using System.Linq;
using System.Threading.Tasks;
using dotnet.Models;
using Microsoft.AspNetCore.Mvc;
using ProGaudi.MsgPack.Light;
using ProGaudi.Tarantool.Client;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;

namespace dotnet.Controllers
{
    public class HomeController : Controller
    {
        private readonly Box _box;
        private readonly ISpace _space;
        private readonly IIndex _primaryIndex;
        private readonly IIndex _secondaryIndex;

        public HomeController(Box box)
        {
            _box = box;

            var result = Initialize().GetAwaiter().GetResult();
            _space = result.Item1;
            _primaryIndex = result.Item2;
            _secondaryIndex = result.Item3;
        }

        private async Task<Tuple<ISpace, IIndex, IIndex>> Initialize()
        {
            var schema = _box.GetSchema();

            var space = await schema.GetSpace("some_space");
            var primaryIndex = await space.GetIndex("primary");
            var index = await space.GetIndex("some_secondary_index");

            return Tuple.Create(space, primaryIndex, index);
        }

        public async Task<ViewResult> Index()
        {
            var allDogs = await _primaryIndex.Select<TarantoolTuple<long>, TarantoolTuple<long, string, long, MsgPackToken>>(TarantoolTuple.Create(-1L), new SelectOptions { Iterator = Iterator.All });
            var seniorDogs = await _secondaryIndex.Select<TarantoolTuple<long>, TarantoolTuple<long, string, long, MsgPackToken>>(TarantoolTuple.Create(5L), new SelectOptions { Iterator = Iterator.Ge });
            var juniorDogs = await _secondaryIndex.Select<TarantoolTuple<long>, TarantoolTuple<long, string, long, MsgPackToken>>(TarantoolTuple.Create(5L), new SelectOptions { Iterator = Iterator.Le });

            return View(new []
            {
                allDogs.Data.Select(x => new Dog(x)).ToArray(),
                seniorDogs.Data.Select(x => new Dog(x)).ToArray(),
                juniorDogs.Data.Select(x => new Dog(x)).ToArray()
            });
        }
    }
}
