using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client
{
    public static class FeatureExtensions
    {
        public static bool IsSqlAvailable(this BoxInfo info)
        {
            if (info == null)
                return true;

            return info.Version >= (1, 8);
        }
    }
}
