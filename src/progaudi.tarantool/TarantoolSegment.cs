using System;
using System.Buffers;

namespace ProGaudi.Tarantool.Client
{
    internal class TarantoolSegment : ReadOnlySequenceSegment<byte>
    {
        private TarantoolSegment(in ReadOnlyMemory<byte> memory, in TarantoolSegment next, in long runningIndex)
        {
            Memory = memory;
            Next = next;
            RunningIndex = runningIndex;
        }

        public static ReadOnlySequence<byte> CreateSequence(in ReadOnlyMemory<byte> length, in ReadOnlyMemory<byte> header, in ReadOnlyMemory<byte> body)
        {
            var b = new TarantoolSegment(body, null, length.Length + header.Length);
            var h = new TarantoolSegment(header, b, length.Length);
            var l = new TarantoolSegment(length, h, 0);

            return new ReadOnlySequence<byte>(l, 0, b, b.Memory.Length);
        }
    }
}