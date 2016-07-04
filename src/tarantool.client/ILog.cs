namespace Tarantool.Client
{
    public interface ILog
    {
        void WriteLine(string message);

        void Flush();
    }
}