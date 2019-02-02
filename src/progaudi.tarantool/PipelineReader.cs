using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client
{
    internal class PipelineReader : IResponseReader
    {
        private readonly PipeReader _input;
        private readonly ITaskSource _source;
        private readonly Thread _thread;
        private readonly ManualResetEventSlim _exitEvent;
        private bool _disposed;

        public PipelineReader(PipeReader input, ITaskSource source, ClientOptions clientOptions)
        {
            _input = input;
            _source = source;
            _thread = new Thread(Read)
            {
                IsBackground = true,
                Name = $"{clientOptions.Name} :: Read"
            };
            _exitEvent = new ManualResetEventSlim(false);
        }

        private void Read()
        {
            while (!_disposed || _exitEvent.IsSet)
            {
                Impl().GetAwaiter().GetResult();
            }

            async Task Impl()
            {
                bool allowSyncRead = true;

                while (true)
                {
                    var input = _input;
                    if (input == null) break;

                    // note: TryRead will give us back the same buffer in a tight loop
                    // - so: only use that if we're making progress
                    if (!(allowSyncRead && input.TryRead(out var readResult)))
                    {
                        readResult = await input.ReadAsync().ConfigureAwait(false);
                    }


                    var buffer = readResult.Buffer;
                    int handled = 0;
                    if (!buffer.IsEmpty)
                    {
                        handled = ProcessBuffer(ref buffer); // updates buffer.Start
                    }

                    allowSyncRead = handled != 0;

                    input.AdvanceTo(buffer.Start, buffer.End);

                    if (handled == 0 && readResult.IsCompleted)
                    {
                        break; // no more data, or trailing incomplete messages
                    }
                }
            }

            int ProcessBuffer(ref ReadOnlySequence<byte> sequence)
            {
                var result = 0;
                while (true)
                {
                    if (!MsgPackSpec.TryReadFixUInt32(sequence, out var packetLength, out var readSize)) break;

                    if (sequence.Length < packetLength + readSize)
                    {
                        break;
                    }

                    sequence = sequence.Slice(readSize);
                    _source.MatchResult(sequence.Slice(0, packetLength));
                    sequence = sequence.Slice(packetLength);
                    result++;
                }

                return result;
            }
        }

        public void Dispose()
        {
            if (_exitEvent.IsSet || _disposed)
            {
                return;
            }

            _disposed = true;
            _thread.Join();
            _exitEvent.Dispose();
        }

        public void Start()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(PipelineReader));
            }
            
            _thread.Start();
        }
    }
}