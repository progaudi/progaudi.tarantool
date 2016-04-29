namespace iproto
{
    public interface IMyTuple { }
    public class MyTuple<T1> : IMyTuple
    {
        public MyTuple(T1 item1)
        {
            Item1 = item1;
            Item1 = item1;
        }
        public T1 Item1 { get; }
    }
    public class MyTuple<T1, T2> : IMyTuple
    {
        public MyTuple(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item1 = item1; Item2 = item2;
        }
        public T1 Item1 { get; }
        public T2 Item2 { get; }
    }
    public class MyTuple<T1, T2, T3> : IMyTuple
    {
        public MyTuple(T1 item1, T2 item2, T3 item3)
        {
            Item1 = item1;
            Item1 = item1; Item2 = item2; Item3 = item3;
        }
        public T1 Item1 { get; }
        public T2 Item2 { get; }
        public T3 Item3 { get; }
    }
    public class MyTuple<T1, T2, T3, T4> : IMyTuple
    {
        public MyTuple(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            Item1 = item1;
            Item1 = item1; Item2 = item2; Item3 = item3; Item4 = item4;
        }
        public T1 Item1 { get; }
        public T2 Item2 { get; }
        public T3 Item3 { get; }
        public T4 Item4 { get; }
    }
    public class MyTuple<T1, T2, T3, T4, T5> : IMyTuple
    {
        public MyTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
        {
            Item1 = item1;
            Item1 = item1; Item2 = item2; Item3 = item3; Item4 = item4; Item5 = item5;
        }
        public T1 Item1 { get; }
        public T2 Item2 { get; }
        public T3 Item3 { get; }
        public T4 Item4 { get; }
        public T5 Item5 { get; }
    }
    public class MyTuple<T1, T2, T3, T4, T5, T6> : IMyTuple
    {
        public MyTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
        {
            Item1 = item1;
            Item1 = item1; Item2 = item2; Item3 = item3; Item4 = item4; Item5 = item5; Item6 = item6;
        }
        public T1 Item1 { get; }
        public T2 Item2 { get; }
        public T3 Item3 { get; }
        public T4 Item4 { get; }
        public T5 Item5 { get; }
        public T6 Item6 { get; }
    }
    public class MyTuple<T1, T2, T3, T4, T5, T6, T7> : IMyTuple
    {
        public MyTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
        {
            Item1 = item1;
            Item1 = item1; Item2 = item2; Item3 = item3; Item4 = item4; Item5 = item5; Item6 = item6; Item7 = item7;
        }
        public T1 Item1 { get; }
        public T2 Item2 { get; }
        public T3 Item3 { get; }
        public T4 Item4 { get; }
        public T5 Item5 { get; }
        public T6 Item6 { get; }
        public T7 Item7 { get; }
    }
    public class MyTuple<T1, T2, T3, T4, T5, T6, T7, T8> : IMyTuple
    {
        public MyTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
        {
            Item1 = item1;
            Item1 = item1; Item2 = item2; Item3 = item3; Item4 = item4; Item5 = item5; Item6 = item6; Item7 = item7; Item8 = item8;
        }
        public T1 Item1 { get; }
        public T2 Item2 { get; }
        public T3 Item3 { get; }
        public T4 Item4 { get; }
        public T5 Item5 { get; }
        public T6 Item6 { get; }
        public T7 Item7 { get; }
        public T8 Item8 { get; }
    }

    public class MyTuple
    {
        public static MyTuple<T1>
        Create<T1>(
        T1 item1)
        {
            return new MyTuple<T1>
           (item1);
        }

        public static MyTuple<T1, T2>
        Create<T1, T2>(
        T1 item1, T2 item2)
        {
            return new MyTuple<T1, T2>
           (item1, item2);
        }

        public static MyTuple<T1, T2, T3>
        Create<T1, T2, T3>(
        T1 item1, T2 item2, T3 item3)
        {
            return new MyTuple<T1, T2, T3>
           (item1, item2, item3);
        }

        public static MyTuple<T1, T2, T3, T4>
        Create<T1, T2, T3, T4>(
        T1 item1, T2 item2, T3 item3, T4 item4)
        {
            return new MyTuple<T1, T2, T3, T4>
           (item1, item2, item3, item4);
        }

        public static MyTuple<T1, T2, T3, T4, T5>
        Create<T1, T2, T3, T4, T5>(
        T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
        {
            return new MyTuple<T1, T2, T3, T4, T5>
           (item1, item2, item3, item4, item5);
        }

        public static MyTuple<T1, T2, T3, T4, T5, T6>
        Create<T1, T2, T3, T4, T5, T6>(
        T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
        {
            return new MyTuple<T1, T2, T3, T4, T5, T6>
           (item1, item2, item3, item4, item5, item6);
        }

        public static MyTuple<T1, T2, T3, T4, T5, T6, T7>
        Create<T1, T2, T3, T4, T5, T6, T7>(
        T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
        {
            return new MyTuple<T1, T2, T3, T4, T5, T6, T7>
           (item1, item2, item3, item4, item5, item6, item7);
        }

        public static MyTuple<T1, T2, T3, T4, T5, T6, T7, T8>
        Create<T1, T2, T3, T4, T5, T6, T7, T8>(
        T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
        {
            return new MyTuple<T1, T2, T3, T4, T5, T6, T7, T8>
           (item1, item2, item3, item4, item5, item6, item7, item8);
        }

    }

}