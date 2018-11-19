namespace TestPipelines
{
    public interface ILog
    {
        void WriteLine(string message);

        void Flush();
    }
}