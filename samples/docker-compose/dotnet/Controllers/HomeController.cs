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
        private readonly IIndex _primaryIndex;
        private readonly IIndex _secondaryIndex;

        public HomeController(Box box) => (_primaryIndex, _secondaryIndex) = Initialize(box).GetAwaiter().GetResult();

        private static async Task<(IIndex, IIndex)> Initialize(IBox box)
        {
            var schema = box.GetSchema();

            var space = await schema.GetSpace("some_space");
            var primaryIndex = await space.GetIndex("primary");
            var index = await space.GetIndex("some_secondary_index");

            return (primaryIndex, index);
        }

        public async Task<ViewResult> Index()
        {
            var allDogs = await _primaryIndex.Select<TarantoolTuple<long>, Dog>(TarantoolTuple.Create(-1L), new SelectOptions { Iterator = Iterator.All });
            var seniorDogs = await _secondaryIndex.Select<TarantoolTuple<long>, Dog>(TarantoolTuple.Create(5L), new SelectOptions { Iterator = Iterator.Ge });
            var juniorDogs = await _secondaryIndex.Select<TarantoolTuple<long>, Dog>(TarantoolTuple.Create(5L), new SelectOptions { Iterator = Iterator.Le });

            return View(new []
            {
                allDogs.Data,
                seniorDogs.Data,
                juniorDogs.Data
            });
        }
    }
}
