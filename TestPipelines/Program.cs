using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Pipelines.Sockets.Unofficial;
using ProGaudi.MsgPack;

namespace TestPipelines
{
    class Program
    {
        static void Main(string[] args) => MainAsync(args).GetAwaiter().GetResult();

        static async Task MainAsync(string[] args)
        {
            SocketConnection.SetLog(Console.Out);
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await socket.ConnectAsync("localhost", 3301);
            var pipeline = SocketConnection.Create(socket, PipeOptions.Default, PipeOptions.Default, SocketConnectionOptions.InlineConnect, "123");
            await OnConnected(pipeline);
        }

        private static async Task OnConnected(SocketConnection sc)
        {
            var resultTask = sc.Input.ReadAsync();
            var result = resultTask.IsCompletedSuccessfully ? resultTask.Result : await resultTask;
            Console.WriteLine(result.Buffer.Length);
            using (var owner = new SequenceOwner(result.Buffer))
            {
                var greetings = new GreetingsResponse(owner.RealMemory.Span);
                Console.WriteLine($"Greetings received, salt is {Convert.ToBase64String(greetings.Salt)} .");
                sc.Input.AdvanceTo(result.Buffer.End);
                var authenticateRequest = AuthenticationRequest.Create(greetings, "operator", "operator");
                var length = SendAuth(sc.Output.GetSpan(1024), authenticateRequest);
                sc.Output.Advance(length);
                Console.WriteLine(length);
                Console.WriteLine(await Flush(sc.Output));
            }
            
            resultTask = sc.Input.ReadAsync();
            result = resultTask.IsCompletedSuccessfully ? resultTask.Result : await resultTask;

            using (var owner = new SequenceOwner(result.Buffer))
            {
                Console.WriteLine(owner.RealMemory.Length);
            }
        }
        
        private static ValueTask<bool> Flush(PipeWriter writer)
        {
            bool GetResult(FlushResult flush)
            // tell the calling code whether any more messages
            // should be written
                => !(flush.IsCanceled || flush.IsCompleted);

            async ValueTask<bool> Awaited(ValueTask<FlushResult> incomplete)
                => GetResult(await incomplete);

            // apply back-pressure etc
            var flushTask = writer.FlushAsync();

            return flushTask.IsCompletedSuccessfully
                ? new ValueTask<bool>(GetResult(flushTask.Result))
                : Awaited(flushTask);
        }

        private static int SendAuth(Span<byte> span, AuthenticationRequest authenticationRequest)
        {
            const int lengthLength = 5;
            var body = span.Slice(lengthLength);
            
            var wroteSize = 0;
            wroteSize += MsgPackSpec.WriteFixMapHeader(body, 2);
            wroteSize += MsgPackSpec.WritePositiveFixInt(body.Slice(wroteSize), (byte) Key.Code);
            wroteSize += MsgPackSpec.WritePositiveFixInt(body.Slice(wroteSize), (byte) CommandCode.Auth);
            wroteSize += MsgPackSpec.WritePositiveFixInt(body.Slice(wroteSize), (byte) Key.Sync);
            wroteSize += MsgPackSpec.WriteFixUInt64(body.Slice(wroteSize), 1);

            wroteSize += MsgPackSpec.WriteFixMapHeader(body.Slice(wroteSize), 2);
            wroteSize += MsgPackSpec.WritePositiveFixInt(body.Slice(wroteSize), (byte) Key.Username);
            wroteSize += MsgPackSpec.WriteString(body.Slice(wroteSize), authenticationRequest.Username, Encoding.ASCII);
            wroteSize += MsgPackSpec.WritePositiveFixInt(body.Slice(wroteSize), (byte) Key.Tuple);
            wroteSize += MsgPackSpec.WriteFixArrayHeader(body.Slice(wroteSize), 2);
            wroteSize += MsgPackSpec.WriteFixString(body.Slice(wroteSize), "chap-sha1", Encoding.ASCII);
            wroteSize += MsgPackSpec.WriteBinary8(body.Slice(wroteSize), authenticationRequest.Scramble.Memory.Span);
            MsgPackSpec.WriteFixUInt32(span, (uint) wroteSize);

            return wroteSize + lengthLength;
        }

        public sealed class SequenceOwner : IDisposable
        {
            private readonly IMemoryOwner<byte> _owner;

            public SequenceOwner(ReadOnlySequence<byte> sequence)
            {
                if (sequence.IsSingleSegment)
                {
                    RealMemory = sequence.First;
                }
                else
                {
                    _owner = MemoryPool<byte>.Shared.Rent((int) sequence.Length);
                    sequence.CopyTo(_owner.Memory.Span);
                }
            }

            public void Dispose()
            {
                _owner?.Dispose();
            }

            public ReadOnlyMemory<byte> RealMemory { get; }
        }
    }
}
