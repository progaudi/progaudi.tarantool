using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client
{
    public interface ISchema
    {
        Task<ISpace> CreateSpaceAsync(string spaceName, SpaceCreationOptions options = null);
        Task<ISpace> GetSpace(string name);
        Task<ISpace> GetSpace(uint id);
    }
}