using MsgPack.Light;
using Shouldly;
using Tuple = iproto.Tuple;

namespace tarantool_client.Converters
{
    public class TupleConverter<T1> : IMsgPackConverter<iproto.Tuple<T1>>
    {
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }

        public void Write(iproto.Tuple<T1> value, IMsgPackWriter writer)
        {
            if (value == null)
            {
                _context.NullConverter.Write(null, writer);
                return;
            }

            var t1Converter = _context.GetConverter<T1>();
            writer.WriteArrayHeader(1);
            t1Converter.Write(value.Item1, writer);
        }

        public iproto.Tuple<T1> Read(IMsgPackReader reader)
        {
            var t1Converter = _context.GetConverter<T1>();

            reader.ReadArrayLength().ShouldBe(1u);
            var item1 = t1Converter.Read(reader);

            return Tuple.Create(item1);
        }
    }

    public class TupleConverter<T1, T2> : IMsgPackConverter<iproto.Tuple<T1, T2>>
    {
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }

        public void Write(iproto.Tuple<T1, T2> value, IMsgPackWriter writer)
        {
            if (value == null)
            {
                _context.NullConverter.Write(null, writer);
                return;
            }

            var t1Converter = _context.GetConverter<T1>();
            var t2Converter = _context.GetConverter<T2>();

            writer.WriteArrayHeader(2);
            t1Converter.Write(value.Item1, writer);
            t2Converter.Write(value.Item2, writer);
        }

        public iproto.Tuple<T1, T2> Read(IMsgPackReader reader)
        {
            var t1Converter = _context.GetConverter<T1>();
            var t2Converter = _context.GetConverter<T2>();

            reader.ReadArrayLength().ShouldBe(2u);
            var item1 = t1Converter.Read(reader);
            var item2 = t2Converter.Read(reader);

            return Tuple.Create(item1, item2);
        }
    }

    public class TupleConverter<T1, T2, T3> : IMsgPackConverter<iproto.Tuple<T1, T2, T3>>
    {
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }

        public void Write(iproto.Tuple<T1, T2, T3> value, IMsgPackWriter writer)
        {
            if (value == null)
            {
                _context.NullConverter.Write(null, writer);
                return;
            }

            var t1Converter = _context.GetConverter<T1>();
            var t2Converter = _context.GetConverter<T2>();
            var t3Converter = _context.GetConverter<T3>();

            writer.WriteArrayHeader(3);
            t1Converter.Write(value.Item1, writer);
            t2Converter.Write(value.Item2, writer);
            t3Converter.Write(value.Item3, writer);
        }

        public iproto.Tuple<T1, T2, T3> Read(IMsgPackReader reader)
        {
            var t1Converter = _context.GetConverter<T1>();
            var t2Converter = _context.GetConverter<T2>();
            var t3Converter = _context.GetConverter<T3>();

            reader.ReadArrayLength().ShouldBe(3u);
            var item1 = t1Converter.Read(reader);
            var item2 = t2Converter.Read(reader);
            var item3 = t3Converter.Read(reader);

            return Tuple.Create(item1, item2, item3);
        }
    }

    public class TupleConverter<T1, T2, T3, T4> : IMsgPackConverter<iproto.Tuple<T1, T2, T3, T4>>
    {
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }

        public void Write(iproto.Tuple<T1, T2, T3, T4> value, IMsgPackWriter writer)
        {
            if (value == null)
            {
                _context.NullConverter.Write(null, writer);
                return;
            }

            var t1Converter = _context.GetConverter<T1>();
            var t2Converter = _context.GetConverter<T2>();
            var t3Converter = _context.GetConverter<T3>();
            var t4Converter = _context.GetConverter<T4>();

            writer.WriteArrayHeader(4);
            t1Converter.Write(value.Item1, writer);
            t2Converter.Write(value.Item2, writer);
            t3Converter.Write(value.Item3, writer);
            t4Converter.Write(value.Item4, writer);
        }

        public iproto.Tuple<T1, T2, T3, T4> Read(IMsgPackReader reader)
        {
            var t1Converter = _context.GetConverter<T1>();
            var t2Converter = _context.GetConverter<T2>();
            var t3Converter = _context.GetConverter<T3>();
            var t4Converter = _context.GetConverter<T4>();

            reader.ReadArrayLength().ShouldBe(4u);
            var item1 = t1Converter.Read(reader);
            var item2 = t2Converter.Read(reader);
            var item3 = t3Converter.Read(reader);
            var item4 = t4Converter.Read(reader);

            return Tuple.Create(item1, item2, item3, item4);
        }
    }

    public class TupleConverter<T1, T2, T3, T4, T5> : IMsgPackConverter<iproto.Tuple<T1, T2, T3, T4, T5>>
    {
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }

        public void Write(iproto.Tuple<T1, T2, T3, T4, T5> value, IMsgPackWriter writer)
        {
            if (value == null)
            {
                _context.NullConverter.Write(null, writer);
                return;
            }

            var t1Converter = _context.GetConverter<T1>();
            var t2Converter = _context.GetConverter<T2>();
            var t3Converter = _context.GetConverter<T3>();
            var t4Converter = _context.GetConverter<T4>();
            var t5Converter = _context.GetConverter<T5>();

            writer.WriteArrayHeader(5);
            t1Converter.Write(value.Item1, writer);
            t2Converter.Write(value.Item2, writer);
            t3Converter.Write(value.Item3, writer);
            t4Converter.Write(value.Item4, writer);
            t5Converter.Write(value.Item5, writer);
        }

        public iproto.Tuple<T1, T2, T3, T4, T5> Read(IMsgPackReader reader)
        {
            var t1Converter = _context.GetConverter<T1>();
            var t2Converter = _context.GetConverter<T2>();
            var t3Converter = _context.GetConverter<T3>();
            var t4Converter = _context.GetConverter<T4>();
            var t5Converter = _context.GetConverter<T5>();

            reader.ReadArrayLength().ShouldBe(5u);
            var item1 = t1Converter.Read(reader);
            var item2 = t2Converter.Read(reader);
            var item3 = t3Converter.Read(reader);
            var item4 = t4Converter.Read(reader);
            var item5 = t5Converter.Read(reader);

            return Tuple.Create(item1, item2, item3, item4, item5);
        }
    }

    public class TupleConverter<T1, T2, T3, T4, T5, T6> : IMsgPackConverter<iproto.Tuple<T1, T2, T3, T4, T5, T6>>
    {
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }

        public void Write(iproto.Tuple<T1, T2, T3, T4, T5, T6> value, IMsgPackWriter writer)
        {
            if (value == null)
            {
                _context.NullConverter.Write(null, writer);
                return;
            }

            var t1Converter = _context.GetConverter<T1>();
            var t2Converter = _context.GetConverter<T2>();
            var t3Converter = _context.GetConverter<T3>();
            var t4Converter = _context.GetConverter<T4>();
            var t5Converter = _context.GetConverter<T5>();
            var t6Converter = _context.GetConverter<T6>();

            writer.WriteArrayHeader(6);
            t1Converter.Write(value.Item1, writer);
            t2Converter.Write(value.Item2, writer);
            t3Converter.Write(value.Item3, writer);
            t4Converter.Write(value.Item4, writer);
            t5Converter.Write(value.Item5, writer);
            t6Converter.Write(value.Item6, writer);
        }

        public iproto.Tuple<T1, T2, T3, T4, T5, T6> Read(
            IMsgPackReader reader)
        {
            var t1Converter = _context.GetConverter<T1>();
            var t2Converter = _context.GetConverter<T2>();
            var t3Converter = _context.GetConverter<T3>();
            var t4Converter = _context.GetConverter<T4>();
            var t5Converter = _context.GetConverter<T5>();
            var t6Converter = _context.GetConverter<T6>();

            reader.ReadArrayLength().ShouldBe(6u);
            var item1 = t1Converter.Read(reader);
            var item2 = t2Converter.Read(reader);
            var item3 = t3Converter.Read(reader);
            var item4 = t4Converter.Read(reader);
            var item5 = t5Converter.Read(reader);
            var item6 = t6Converter.Read(reader);

            return Tuple.Create(item1, item2, item3, item4, item5, item6);
        }
    }

    public class TupleConverter<T1, T2, T3, T4, T5, T6, T7> : IMsgPackConverter<iproto.Tuple<T1, T2, T3, T4, T5, T6, T7>>
    {
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }

        public void Write(iproto.Tuple<T1, T2, T3, T4, T5, T6, T7> value, IMsgPackWriter writer)
        {
            if (value == null)
            {
                _context.NullConverter.Write(null, writer);
                return;
            }

            var t1Converter = _context.GetConverter<T1>();
            var t2Converter = _context.GetConverter<T2>();
            var t3Converter = _context.GetConverter<T3>();
            var t4Converter = _context.GetConverter<T4>();
            var t5Converter = _context.GetConverter<T5>();
            var t6Converter = _context.GetConverter<T6>();
            var t7Converter = _context.GetConverter<T7>();

            writer.WriteArrayHeader(7);
            t1Converter.Write(value.Item1, writer);
            t2Converter.Write(value.Item2, writer);
            t3Converter.Write(value.Item3, writer);
            t4Converter.Write(value.Item4, writer);
            t5Converter.Write(value.Item5, writer);
            t6Converter.Write(value.Item6, writer);
            t7Converter.Write(value.Item7, writer);
        }

        public iproto.Tuple<T1, T2, T3, T4, T5, T6, T7> Read(
            IMsgPackReader reader)
        {
            var t1Converter = _context.GetConverter<T1>();
            var t2Converter = _context.GetConverter<T2>();
            var t3Converter = _context.GetConverter<T3>();
            var t4Converter = _context.GetConverter<T4>();
            var t5Converter = _context.GetConverter<T5>();
            var t6Converter = _context.GetConverter<T6>();
            var t7Converter = _context.GetConverter<T7>();

            reader.ReadArrayLength().ShouldBe(7u);
            var item1 = t1Converter.Read(reader);
            var item2 = t2Converter.Read(reader);
            var item3 = t3Converter.Read(reader);
            var item4 = t4Converter.Read(reader);
            var item5 = t5Converter.Read(reader);
            var item6 = t6Converter.Read(reader);
            var item7 = t7Converter.Read(reader);

            return Tuple.Create(item1, item2, item3, item4, item5, item6, item7);
        }
    }

    public class TupleConverter<T1, T2, T3, T4, T5, T6, T7, TRest> : IMsgPackConverter<iproto.Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>>
    {
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }

        public void Write(iproto.Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> value, IMsgPackWriter writer)
        {
            if (value == null)
            {
                _context.NullConverter.Write(null, writer);
                return;
            }

            var t1Converter = _context.GetConverter<T1>();
            var t2Converter = _context.GetConverter<T2>();
            var t3Converter = _context.GetConverter<T3>();
            var t4Converter = _context.GetConverter<T4>();
            var t5Converter = _context.GetConverter<T5>();
            var t6Converter = _context.GetConverter<T6>();
            var t7Converter = _context.GetConverter<T7>();
            var t8Converter = _context.GetConverter<TRest>();

            writer.WriteArrayHeader(8);
            t1Converter.Write(value.Item1, writer);
            t2Converter.Write(value.Item2, writer);
            t3Converter.Write(value.Item3, writer);
            t4Converter.Write(value.Item4, writer);
            t5Converter.Write(value.Item5, writer);
            t6Converter.Write(value.Item6, writer);
            t7Converter.Write(value.Item7, writer);
            t8Converter.Write(value.Item8, writer);
        }

        public iproto.Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> Read(
            IMsgPackReader reader)
        {
            var t1Converter = _context.GetConverter<T1>();
            var t2Converter = _context.GetConverter<T2>();
            var t3Converter = _context.GetConverter<T3>();
            var t4Converter = _context.GetConverter<T4>();
            var t5Converter = _context.GetConverter<T5>();
            var t6Converter = _context.GetConverter<T6>();
            var t7Converter = _context.GetConverter<T7>();
            var t8Converter = _context.GetConverter<TRest>();

            reader.ReadArrayLength().ShouldBe(8u);
            var item1 = t1Converter.Read(reader);
            var item2 = t2Converter.Read(reader);
            var item3 = t3Converter.Read(reader);
            var item4 = t4Converter.Read(reader);
            var item5 = t5Converter.Read(reader);
            var item6 = t6Converter.Read(reader);
            var item7 = t7Converter.Read(reader);
            var item8 = t8Converter.Read(reader);

            return new iproto.Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>(item1, item2, item3, item4, item5, item6, item7, item8);
        }
    }
}