  

using System;
using System.Buffers;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client.Converters
{
	public class ValueTupleFormatter<T1> : IMsgPackFormatter<ValueTuple<T1>>
	{
		private readonly IMsgPackFormatter<T1> _t1Formatter;

		public ValueTupleFormatter(MsgPackContext context)
		{
			_t1Formatter = context.GetRequiredFormatter<T1>();
		}

		public int GetBufferSize(ValueTuple<T1> value) =>
			DataLengths.FixArrayHeader + _t1Formatter.GetBufferSize(value.Item1);

		public bool HasConstantSize => _t1Formatter.HasConstantSize;

		public int Format(Span<byte> destination, ValueTuple<T1> value)
		{
			var result = MsgPackSpec.WriteFixArrayHeader(destination, 1);

			result += _t1Formatter.Format(destination.Slice(result), value.Item1);

			return result;
		}
	}

	public class ValueTupleFormatter<T1, T2> : IMsgPackFormatter<(T1, T2)>
	{
		private readonly IMsgPackFormatter<T1> _t1Formatter;
		private readonly IMsgPackFormatter<T2> _t2Formatter;

		public ValueTupleFormatter(MsgPackContext context)
		{
			_t1Formatter = context.GetRequiredFormatter<T1>();
			_t2Formatter = context.GetRequiredFormatter<T2>();
		}

		public int GetBufferSize((T1, T2) value) =>
			DataLengths.FixArrayHeader
			+ _t1Formatter.GetBufferSize(value.Item1)
			+ _t2Formatter.GetBufferSize(value.Item2);

		public bool HasConstantSize => _t1Formatter.HasConstantSize;

		public int Format(Span<byte> destination, (T1, T2) value)
		{
			var result = MsgPackSpec.WriteFixArrayHeader(destination, 2);

			result += _t1Formatter.Format(destination.Slice(result), value.Item1);
			result += _t2Formatter.Format(destination.Slice(result), value.Item2);

			return result;
		}
	}

	public class ValueTupleFormatter<T1, T2, T3> : IMsgPackFormatter<(T1, T2, T3)>
	{
		private readonly IMsgPackFormatter<T1> _t1Formatter;
		private readonly IMsgPackFormatter<T2> _t2Formatter;
		private readonly IMsgPackFormatter<T3> _t3Formatter;

		public ValueTupleFormatter(MsgPackContext context)
		{
			_t1Formatter = context.GetRequiredFormatter<T1>();
			_t2Formatter = context.GetRequiredFormatter<T2>();
			_t3Formatter = context.GetRequiredFormatter<T3>();
		}

		public int GetBufferSize((T1, T2, T3) value) =>
			DataLengths.FixArrayHeader
			+ _t1Formatter.GetBufferSize(value.Item1)
			+ _t2Formatter.GetBufferSize(value.Item2)
			+ _t3Formatter.GetBufferSize(value.Item3);

		public bool HasConstantSize => _t1Formatter.HasConstantSize
		    && _t2Formatter.HasConstantSize
		    && _t3Formatter.HasConstantSize;

		public int Format(Span<byte> destination, (T1, T2, T3) value)
		{
			var result = MsgPackSpec.WriteFixArrayHeader(destination, 3);

			result += _t1Formatter.Format(destination.Slice(result), value.Item1);
			result += _t2Formatter.Format(destination.Slice(result), value.Item2);
			result += _t3Formatter.Format(destination.Slice(result), value.Item3);

			return result;
		}
	}

	public class ValueTupleFormatter<T1, T2, T3, T4> : IMsgPackFormatter<(T1, T2, T3, T4)>
	{
		private readonly IMsgPackFormatter<T1> _t1Formatter;
		private readonly IMsgPackFormatter<T2> _t2Formatter;
		private readonly IMsgPackFormatter<T3> _t3Formatter;
		private readonly IMsgPackFormatter<T4> _t4Formatter;

		public ValueTupleFormatter(MsgPackContext context)
		{
			_t1Formatter = context.GetRequiredFormatter<T1>();
			_t2Formatter = context.GetRequiredFormatter<T2>();
			_t3Formatter = context.GetRequiredFormatter<T3>();
			_t4Formatter = context.GetRequiredFormatter<T4>();
		}

		public int GetBufferSize((T1, T2, T3, T4) value) =>
			DataLengths.FixArrayHeader
			+ _t1Formatter.GetBufferSize(value.Item1)
			+ _t2Formatter.GetBufferSize(value.Item2)
			+ _t3Formatter.GetBufferSize(value.Item3)
			+ _t4Formatter.GetBufferSize(value.Item4);

		public bool HasConstantSize => _t1Formatter.HasConstantSize
		    && _t2Formatter.HasConstantSize
		    && _t3Formatter.HasConstantSize
		    && _t4Formatter.HasConstantSize;

		public int Format(Span<byte> destination, (T1, T2, T3, T4) value)
		{
			var result = MsgPackSpec.WriteFixArrayHeader(destination, 4);

			result += _t1Formatter.Format(destination.Slice(result), value.Item1);
			result += _t2Formatter.Format(destination.Slice(result), value.Item2);
			result += _t3Formatter.Format(destination.Slice(result), value.Item3);
			result += _t4Formatter.Format(destination.Slice(result), value.Item4);

			return result;
		}
	}

	public class ValueTupleFormatter<T1, T2, T3, T4, T5> : IMsgPackFormatter<(T1, T2, T3, T4, T5)>
	{
		private readonly IMsgPackFormatter<T1> _t1Formatter;
		private readonly IMsgPackFormatter<T2> _t2Formatter;
		private readonly IMsgPackFormatter<T3> _t3Formatter;
		private readonly IMsgPackFormatter<T4> _t4Formatter;
		private readonly IMsgPackFormatter<T5> _t5Formatter;

		public ValueTupleFormatter(MsgPackContext context)
		{
			_t1Formatter = context.GetRequiredFormatter<T1>();
			_t2Formatter = context.GetRequiredFormatter<T2>();
			_t3Formatter = context.GetRequiredFormatter<T3>();
			_t4Formatter = context.GetRequiredFormatter<T4>();
			_t5Formatter = context.GetRequiredFormatter<T5>();
		}

		public int GetBufferSize((T1, T2, T3, T4, T5) value) =>
			DataLengths.FixArrayHeader
			+ _t1Formatter.GetBufferSize(value.Item1)
			+ _t2Formatter.GetBufferSize(value.Item2)
			+ _t3Formatter.GetBufferSize(value.Item3)
			+ _t4Formatter.GetBufferSize(value.Item4)
			+ _t5Formatter.GetBufferSize(value.Item5);

		public bool HasConstantSize => _t1Formatter.HasConstantSize
		    && _t2Formatter.HasConstantSize
		    && _t3Formatter.HasConstantSize
		    && _t4Formatter.HasConstantSize
		    && _t5Formatter.HasConstantSize;

		public int Format(Span<byte> destination, (T1, T2, T3, T4, T5) value)
		{
			var result = MsgPackSpec.WriteFixArrayHeader(destination, 5);

			result += _t1Formatter.Format(destination.Slice(result), value.Item1);
			result += _t2Formatter.Format(destination.Slice(result), value.Item2);
			result += _t3Formatter.Format(destination.Slice(result), value.Item3);
			result += _t4Formatter.Format(destination.Slice(result), value.Item4);
			result += _t5Formatter.Format(destination.Slice(result), value.Item5);

			return result;
		}
	}

	public class ValueTupleFormatter<T1, T2, T3, T4, T5, T6> : IMsgPackFormatter<(T1, T2, T3, T4, T5, T6)>
	{
		private readonly IMsgPackFormatter<T1> _t1Formatter;
		private readonly IMsgPackFormatter<T2> _t2Formatter;
		private readonly IMsgPackFormatter<T3> _t3Formatter;
		private readonly IMsgPackFormatter<T4> _t4Formatter;
		private readonly IMsgPackFormatter<T5> _t5Formatter;
		private readonly IMsgPackFormatter<T6> _t6Formatter;

		public ValueTupleFormatter(MsgPackContext context)
		{
			_t1Formatter = context.GetRequiredFormatter<T1>();
			_t2Formatter = context.GetRequiredFormatter<T2>();
			_t3Formatter = context.GetRequiredFormatter<T3>();
			_t4Formatter = context.GetRequiredFormatter<T4>();
			_t5Formatter = context.GetRequiredFormatter<T5>();
			_t6Formatter = context.GetRequiredFormatter<T6>();
		}

		public int GetBufferSize((T1, T2, T3, T4, T5, T6) value) =>
			DataLengths.FixArrayHeader
			+ _t1Formatter.GetBufferSize(value.Item1)
			+ _t2Formatter.GetBufferSize(value.Item2)
			+ _t3Formatter.GetBufferSize(value.Item3)
			+ _t4Formatter.GetBufferSize(value.Item4)
			+ _t5Formatter.GetBufferSize(value.Item5)
			+ _t6Formatter.GetBufferSize(value.Item6);

		public bool HasConstantSize => _t1Formatter.HasConstantSize
		    && _t2Formatter.HasConstantSize
		    && _t3Formatter.HasConstantSize
		    && _t4Formatter.HasConstantSize
		    && _t5Formatter.HasConstantSize
		    && _t6Formatter.HasConstantSize;

		public int Format(Span<byte> destination, (T1, T2, T3, T4, T5, T6) value)
		{
			var result = MsgPackSpec.WriteFixArrayHeader(destination, 6);

			result += _t1Formatter.Format(destination.Slice(result), value.Item1);
			result += _t2Formatter.Format(destination.Slice(result), value.Item2);
			result += _t3Formatter.Format(destination.Slice(result), value.Item3);
			result += _t4Formatter.Format(destination.Slice(result), value.Item4);
			result += _t5Formatter.Format(destination.Slice(result), value.Item5);
			result += _t6Formatter.Format(destination.Slice(result), value.Item6);

			return result;
		}
	}

	public class ValueTupleFormatter<T1, T2, T3, T4, T5, T6, T7> : IMsgPackFormatter<(T1, T2, T3, T4, T5, T6, T7)>
	{
		private readonly IMsgPackFormatter<T1> _t1Formatter;
		private readonly IMsgPackFormatter<T2> _t2Formatter;
		private readonly IMsgPackFormatter<T3> _t3Formatter;
		private readonly IMsgPackFormatter<T4> _t4Formatter;
		private readonly IMsgPackFormatter<T5> _t5Formatter;
		private readonly IMsgPackFormatter<T6> _t6Formatter;
		private readonly IMsgPackFormatter<T7> _t7Formatter;

		public ValueTupleFormatter(MsgPackContext context)
		{
			_t1Formatter = context.GetRequiredFormatter<T1>();
			_t2Formatter = context.GetRequiredFormatter<T2>();
			_t3Formatter = context.GetRequiredFormatter<T3>();
			_t4Formatter = context.GetRequiredFormatter<T4>();
			_t5Formatter = context.GetRequiredFormatter<T5>();
			_t6Formatter = context.GetRequiredFormatter<T6>();
			_t7Formatter = context.GetRequiredFormatter<T7>();
		}

		public int GetBufferSize((T1, T2, T3, T4, T5, T6, T7) value) =>
			DataLengths.FixArrayHeader
			+ _t1Formatter.GetBufferSize(value.Item1)
			+ _t2Formatter.GetBufferSize(value.Item2)
			+ _t3Formatter.GetBufferSize(value.Item3)
			+ _t4Formatter.GetBufferSize(value.Item4)
			+ _t5Formatter.GetBufferSize(value.Item5)
			+ _t6Formatter.GetBufferSize(value.Item6)
			+ _t7Formatter.GetBufferSize(value.Item7);

		public bool HasConstantSize => _t1Formatter.HasConstantSize
		    && _t2Formatter.HasConstantSize
		    && _t3Formatter.HasConstantSize
		    && _t4Formatter.HasConstantSize
		    && _t5Formatter.HasConstantSize
		    && _t6Formatter.HasConstantSize
		    && _t7Formatter.HasConstantSize;

		public int Format(Span<byte> destination, (T1, T2, T3, T4, T5, T6, T7) value)
		{
			var result = MsgPackSpec.WriteFixArrayHeader(destination, 7);

			result += _t1Formatter.Format(destination.Slice(result), value.Item1);
			result += _t2Formatter.Format(destination.Slice(result), value.Item2);
			result += _t3Formatter.Format(destination.Slice(result), value.Item3);
			result += _t4Formatter.Format(destination.Slice(result), value.Item4);
			result += _t5Formatter.Format(destination.Slice(result), value.Item5);
			result += _t6Formatter.Format(destination.Slice(result), value.Item6);
			result += _t7Formatter.Format(destination.Slice(result), value.Item7);

			return result;
		}
	}

	public class ValueTupleFormatter<T1, T2, T3, T4, T5, T6, T7, T8> : IMsgPackFormatter<(T1, T2, T3, T4, T5, T6, T7, T8)>
	{
		private readonly IMsgPackFormatter<T1> _t1Formatter;
		private readonly IMsgPackFormatter<T2> _t2Formatter;
		private readonly IMsgPackFormatter<T3> _t3Formatter;
		private readonly IMsgPackFormatter<T4> _t4Formatter;
		private readonly IMsgPackFormatter<T5> _t5Formatter;
		private readonly IMsgPackFormatter<T6> _t6Formatter;
		private readonly IMsgPackFormatter<T7> _t7Formatter;
		private readonly IMsgPackFormatter<T8> _t8Formatter;

		public ValueTupleFormatter(MsgPackContext context)
		{
			_t1Formatter = context.GetRequiredFormatter<T1>();
			_t2Formatter = context.GetRequiredFormatter<T2>();
			_t3Formatter = context.GetRequiredFormatter<T3>();
			_t4Formatter = context.GetRequiredFormatter<T4>();
			_t5Formatter = context.GetRequiredFormatter<T5>();
			_t6Formatter = context.GetRequiredFormatter<T6>();
			_t7Formatter = context.GetRequiredFormatter<T7>();
			_t8Formatter = context.GetRequiredFormatter<T8>();
		}

		public int GetBufferSize((T1, T2, T3, T4, T5, T6, T7, T8) value) =>
			DataLengths.FixArrayHeader
			+ _t1Formatter.GetBufferSize(value.Item1)
			+ _t2Formatter.GetBufferSize(value.Item2)
			+ _t3Formatter.GetBufferSize(value.Item3)
			+ _t4Formatter.GetBufferSize(value.Item4)
			+ _t5Formatter.GetBufferSize(value.Item5)
			+ _t6Formatter.GetBufferSize(value.Item6)
			+ _t7Formatter.GetBufferSize(value.Item7)
			+ _t8Formatter.GetBufferSize(value.Item8);

		public bool HasConstantSize => _t1Formatter.HasConstantSize
		    && _t2Formatter.HasConstantSize
		    && _t3Formatter.HasConstantSize
		    && _t4Formatter.HasConstantSize
		    && _t5Formatter.HasConstantSize
		    && _t6Formatter.HasConstantSize
		    && _t7Formatter.HasConstantSize
		    && _t8Formatter.HasConstantSize;

		public int Format(Span<byte> destination, (T1, T2, T3, T4, T5, T6, T7, T8) value)
		{
			var result = MsgPackSpec.WriteFixArrayHeader(destination, 8);

			result += _t1Formatter.Format(destination.Slice(result), value.Item1);
			result += _t2Formatter.Format(destination.Slice(result), value.Item2);
			result += _t3Formatter.Format(destination.Slice(result), value.Item3);
			result += _t4Formatter.Format(destination.Slice(result), value.Item4);
			result += _t5Formatter.Format(destination.Slice(result), value.Item5);
			result += _t6Formatter.Format(destination.Slice(result), value.Item6);
			result += _t7Formatter.Format(destination.Slice(result), value.Item7);
			result += _t8Formatter.Format(destination.Slice(result), value.Item8);

			return result;
		}
	}

	public class ValueTupleParser<T1> : IMsgPackSequenceParser<ValueTuple<T1>>
	{
		private readonly IMsgPackSequenceParser<T1> _t1Parser;

		public ValueTupleParser(MsgPackContext context)
		{
			_t1Parser = context.GetRequiredSequenceParser<T1>();
		}

		public ValueTuple<T1> Parse(ReadOnlySequence<byte> source, out int readSize)
		{
			var length = MsgPackSpec.ReadArrayHeader(source, out readSize);
			const uint expected = 1u;
			if (length != expected) throw ExceptionHelper.InvalidArrayLength(expected, length);

			// ReSharper disable once InlineOutVariableDeclaration
			int temp;
			var item1 = _t1Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			return ValueTuple.Create(item1);
		}
	}

	public class ValueTupleParser<T1, T2> : IMsgPackSequenceParser<(T1, T2)>
	{
		private readonly IMsgPackSequenceParser<T1> _t1Parser;
		private readonly IMsgPackSequenceParser<T2> _t2Parser;

		public ValueTupleParser(MsgPackContext context)
		{
			_t1Parser = context.GetRequiredSequenceParser<T1>();
			_t2Parser = context.GetRequiredSequenceParser<T2>();
		}

		public (T1, T2) Parse(ReadOnlySequence<byte> source, out int readSize)
		{
			var length = MsgPackSpec.ReadArrayHeader(source, out readSize);
			const uint expected = 2u;
			if (length != expected) throw ExceptionHelper.InvalidArrayLength(expected, length);

			// ReSharper disable once InlineOutVariableDeclaration
			int temp;
			var item1 = _t1Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			var item2 = _t2Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			return ValueTuple.Create(
				item1,
				item2
			);
		}
	}

	public class ValueTupleParser<T1, T2, T3> : IMsgPackSequenceParser<(T1, T2, T3)>
	{
		private readonly IMsgPackSequenceParser<T1> _t1Parser;
		private readonly IMsgPackSequenceParser<T2> _t2Parser;
		private readonly IMsgPackSequenceParser<T3> _t3Parser;

		public ValueTupleParser(MsgPackContext context)
		{
			_t1Parser = context.GetRequiredSequenceParser<T1>();
			_t2Parser = context.GetRequiredSequenceParser<T2>();
			_t3Parser = context.GetRequiredSequenceParser<T3>();
		}

		public (T1, T2, T3) Parse(ReadOnlySequence<byte> source, out int readSize)
		{
			var length = MsgPackSpec.ReadArrayHeader(source, out readSize);
			const uint expected = 3u;
			if (length != expected) throw ExceptionHelper.InvalidArrayLength(expected, length);

			// ReSharper disable once InlineOutVariableDeclaration
			int temp;
			var item1 = _t1Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			var item2 = _t2Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			var item3 = _t3Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			return ValueTuple.Create(
				item1,
				item2,
				item3
			);
		}
	}

	public class ValueTupleParser<T1, T2, T3, T4> : IMsgPackSequenceParser<(T1, T2, T3, T4)>
	{
		private readonly IMsgPackSequenceParser<T1> _t1Parser;
		private readonly IMsgPackSequenceParser<T2> _t2Parser;
		private readonly IMsgPackSequenceParser<T3> _t3Parser;
		private readonly IMsgPackSequenceParser<T4> _t4Parser;

		public ValueTupleParser(MsgPackContext context)
		{
			_t1Parser = context.GetRequiredSequenceParser<T1>();
			_t2Parser = context.GetRequiredSequenceParser<T2>();
			_t3Parser = context.GetRequiredSequenceParser<T3>();
			_t4Parser = context.GetRequiredSequenceParser<T4>();
		}

		public (T1, T2, T3, T4) Parse(ReadOnlySequence<byte> source, out int readSize)
		{
			var length = MsgPackSpec.ReadArrayHeader(source, out readSize);
			const uint expected = 4u;
			if (length != expected) throw ExceptionHelper.InvalidArrayLength(expected, length);

			// ReSharper disable once InlineOutVariableDeclaration
			int temp;
			var item1 = _t1Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			var item2 = _t2Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			var item3 = _t3Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			var item4 = _t4Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			return ValueTuple.Create(
				item1,
				item2,
				item3,
				item4
			);
		}
	}

	public class ValueTupleParser<T1, T2, T3, T4, T5> : IMsgPackSequenceParser<(T1, T2, T3, T4, T5)>
	{
		private readonly IMsgPackSequenceParser<T1> _t1Parser;
		private readonly IMsgPackSequenceParser<T2> _t2Parser;
		private readonly IMsgPackSequenceParser<T3> _t3Parser;
		private readonly IMsgPackSequenceParser<T4> _t4Parser;
		private readonly IMsgPackSequenceParser<T5> _t5Parser;

		public ValueTupleParser(MsgPackContext context)
		{
			_t1Parser = context.GetRequiredSequenceParser<T1>();
			_t2Parser = context.GetRequiredSequenceParser<T2>();
			_t3Parser = context.GetRequiredSequenceParser<T3>();
			_t4Parser = context.GetRequiredSequenceParser<T4>();
			_t5Parser = context.GetRequiredSequenceParser<T5>();
		}

		public (T1, T2, T3, T4, T5) Parse(ReadOnlySequence<byte> source, out int readSize)
		{
			var length = MsgPackSpec.ReadArrayHeader(source, out readSize);
			const uint expected = 5u;
			if (length != expected) throw ExceptionHelper.InvalidArrayLength(expected, length);

			// ReSharper disable once InlineOutVariableDeclaration
			int temp;
			var item1 = _t1Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			var item2 = _t2Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			var item3 = _t3Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			var item4 = _t4Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			var item5 = _t5Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			return ValueTuple.Create(
				item1,
				item2,
				item3,
				item4,
				item5
			);
		}
	}

	public class ValueTupleParser<T1, T2, T3, T4, T5, T6> : IMsgPackSequenceParser<(T1, T2, T3, T4, T5, T6)>
	{
		private readonly IMsgPackSequenceParser<T1> _t1Parser;
		private readonly IMsgPackSequenceParser<T2> _t2Parser;
		private readonly IMsgPackSequenceParser<T3> _t3Parser;
		private readonly IMsgPackSequenceParser<T4> _t4Parser;
		private readonly IMsgPackSequenceParser<T5> _t5Parser;
		private readonly IMsgPackSequenceParser<T6> _t6Parser;

		public ValueTupleParser(MsgPackContext context)
		{
			_t1Parser = context.GetRequiredSequenceParser<T1>();
			_t2Parser = context.GetRequiredSequenceParser<T2>();
			_t3Parser = context.GetRequiredSequenceParser<T3>();
			_t4Parser = context.GetRequiredSequenceParser<T4>();
			_t5Parser = context.GetRequiredSequenceParser<T5>();
			_t6Parser = context.GetRequiredSequenceParser<T6>();
		}

		public (T1, T2, T3, T4, T5, T6) Parse(ReadOnlySequence<byte> source, out int readSize)
		{
			var length = MsgPackSpec.ReadArrayHeader(source, out readSize);
			const uint expected = 6u;
			if (length != expected) throw ExceptionHelper.InvalidArrayLength(expected, length);

			// ReSharper disable once InlineOutVariableDeclaration
			int temp;
			var item1 = _t1Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			var item2 = _t2Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			var item3 = _t3Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			var item4 = _t4Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			var item5 = _t5Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			var item6 = _t6Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			return ValueTuple.Create(
				item1,
				item2,
				item3,
				item4,
				item5,
				item6
			);
		}
	}

	public class ValueTupleParser<T1, T2, T3, T4, T5, T6, T7> : IMsgPackSequenceParser<(T1, T2, T3, T4, T5, T6, T7)>
	{
		private readonly IMsgPackSequenceParser<T1> _t1Parser;
		private readonly IMsgPackSequenceParser<T2> _t2Parser;
		private readonly IMsgPackSequenceParser<T3> _t3Parser;
		private readonly IMsgPackSequenceParser<T4> _t4Parser;
		private readonly IMsgPackSequenceParser<T5> _t5Parser;
		private readonly IMsgPackSequenceParser<T6> _t6Parser;
		private readonly IMsgPackSequenceParser<T7> _t7Parser;

		public ValueTupleParser(MsgPackContext context)
		{
			_t1Parser = context.GetRequiredSequenceParser<T1>();
			_t2Parser = context.GetRequiredSequenceParser<T2>();
			_t3Parser = context.GetRequiredSequenceParser<T3>();
			_t4Parser = context.GetRequiredSequenceParser<T4>();
			_t5Parser = context.GetRequiredSequenceParser<T5>();
			_t6Parser = context.GetRequiredSequenceParser<T6>();
			_t7Parser = context.GetRequiredSequenceParser<T7>();
		}

		public (T1, T2, T3, T4, T5, T6, T7) Parse(ReadOnlySequence<byte> source, out int readSize)
		{
			var length = MsgPackSpec.ReadArrayHeader(source, out readSize);
			const uint expected = 7u;
			if (length != expected) throw ExceptionHelper.InvalidArrayLength(expected, length);

			// ReSharper disable once InlineOutVariableDeclaration
			int temp;
			var item1 = _t1Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			var item2 = _t2Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			var item3 = _t3Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			var item4 = _t4Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			var item5 = _t5Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			var item6 = _t6Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			var item7 = _t7Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			return ValueTuple.Create(
				item1,
				item2,
				item3,
				item4,
				item5,
				item6,
				item7
			);
		}
	}

	public class ValueTupleParser<T1, T2, T3, T4, T5, T6, T7, T8> : IMsgPackSequenceParser<(T1, T2, T3, T4, T5, T6, T7, T8)>
	{
		private readonly IMsgPackSequenceParser<T1> _t1Parser;
		private readonly IMsgPackSequenceParser<T2> _t2Parser;
		private readonly IMsgPackSequenceParser<T3> _t3Parser;
		private readonly IMsgPackSequenceParser<T4> _t4Parser;
		private readonly IMsgPackSequenceParser<T5> _t5Parser;
		private readonly IMsgPackSequenceParser<T6> _t6Parser;
		private readonly IMsgPackSequenceParser<T7> _t7Parser;
		private readonly IMsgPackSequenceParser<T8> _t8Parser;

		public ValueTupleParser(MsgPackContext context)
		{
			_t1Parser = context.GetRequiredSequenceParser<T1>();
			_t2Parser = context.GetRequiredSequenceParser<T2>();
			_t3Parser = context.GetRequiredSequenceParser<T3>();
			_t4Parser = context.GetRequiredSequenceParser<T4>();
			_t5Parser = context.GetRequiredSequenceParser<T5>();
			_t6Parser = context.GetRequiredSequenceParser<T6>();
			_t7Parser = context.GetRequiredSequenceParser<T7>();
			_t8Parser = context.GetRequiredSequenceParser<T8>();
		}

		public (T1, T2, T3, T4, T5, T6, T7, T8) Parse(ReadOnlySequence<byte> source, out int readSize)
		{
			var length = MsgPackSpec.ReadArrayHeader(source, out readSize);
			const uint expected = 8u;
			if (length != expected) throw ExceptionHelper.InvalidArrayLength(expected, length);

			// ReSharper disable once InlineOutVariableDeclaration
			int temp;
			var item1 = _t1Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			var item2 = _t2Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			var item3 = _t3Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			var item4 = _t4Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			var item5 = _t5Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			var item6 = _t6Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			var item7 = _t7Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			var item8 = _t8Parser.Parse(source.Slice(readSize), out temp); readSize += temp;
			return ValueTuple.Create(
				item1,
				item2,
				item3,
				item4,
				item5,
				item6,
				item7,
				item8
			);
		}
	}
}