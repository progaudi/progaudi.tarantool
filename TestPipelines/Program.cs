using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Pipelines.Sockets.Unofficial;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Requests;
using ProGaudi.Tarantool.Client.Model.Responses;
using ProGaudi.Tarantool.Client.Utils;

namespace TestPipelines
{
    class Program
    {
        static void Main(string[] args) => MainAsync(args).GetAwaiter().GetResult();
        static async Task MainAsync(string[] args)
        {
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
                var authenticateRequest = AuthenticationRequest.Create(greetings, new UriBuilder("operator:operator@localhost:3301"));
                var length = SendAuth(sc.Output.GetSpan(1024), authenticateRequest);
                sc.Output.Advance(length);
                await sc.Output.FlushAsync();
            }
        }

        private static int SendAuth(Span<byte> span, AuthenticationRequest authenticationRequest)
        {
            var wroteSize = MsgPackSpec.WriteFixMapHeader(span, 2);
            wroteSize += MsgPackSpec.WritePositiveFixInt(span.Slice(wroteSize), (byte) Key.Username);
            wroteSize += MsgPackSpec.WriteString(span.Slice(wroteSize), authenticationRequest.Username, Encoding.ASCII);
            wroteSize += MsgPackSpec.WritePositiveFixInt(span.Slice(wroteSize), (byte) Key.Tuple);
            wroteSize += MsgPackSpec.WriteFixArrayHeader(span.Slice(wroteSize), 2);
            wroteSize += MsgPackSpec.WriteFixString(span.Slice(wroteSize), "chap-sha1", Encoding.ASCII);
            wroteSize += MsgPackSpec.WriteBinary8(span.Slice(wroteSize), authenticationRequest.Scramble.Memory.Span);

            return wroteSize;
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
