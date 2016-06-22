using System.Collections.Generic;

namespace Tarantool.Client.Model
{
    public interface ITuple
    {
    }

    public class Tuple<T1> : ITuple
    {
        public Tuple(T1 item1)
        {
            Item1 = item1;
        }

        public T1 Item1 { get; }

        protected bool Equals(Tuple<T1> other)
        {
            return EqualityComparer<T1>.Default.Equals(Item1, other.Item1);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Tuple<T1>) obj);
        }

        public override int GetHashCode()
        {
            var hashCode = EqualityComparer<T1>.Default.GetHashCode(Item1);
            return hashCode;
        }
    }

    public class Tuple<T1, T2> : ITuple
    {
        public Tuple(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        public T1 Item1 { get; }

        public T2 Item2 { get; }

        protected bool Equals(Tuple<T1, T2> other)
        {
            return EqualityComparer<T1>.Default.Equals(Item1, other.Item1) && EqualityComparer<T2>.Default.Equals(Item2, other.Item2);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Tuple<T1, T2>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = EqualityComparer<T1>.Default.GetHashCode(Item1);
                hashCode = (hashCode * 397) ^ EqualityComparer<T2>.Default.GetHashCode(Item2);
                return hashCode;
            }
        }
    }

    public class Tuple<T1, T2, T3> : ITuple
    {
        public Tuple(T1 item1, T2 item2, T3 item3)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
        }

        public T1 Item1 { get; }

        public T2 Item2 { get; }

        public T3 Item3 { get; }

        protected bool Equals(Tuple<T1, T2, T3> other)
        {
            return EqualityComparer<T1>.Default.Equals(Item1, other.Item1) && EqualityComparer<T2>.Default.Equals(Item2, other.Item2)
                && EqualityComparer<T3>.Default.Equals(Item3, other.Item3);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Tuple<T1, T2, T3>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = EqualityComparer<T1>.Default.GetHashCode(Item1);
                hashCode = (hashCode * 397) ^ EqualityComparer<T2>.Default.GetHashCode(Item2);
                hashCode = (hashCode * 397) ^ EqualityComparer<T3>.Default.GetHashCode(Item3);
                return hashCode;
            }
        }
    }

    public class Tuple<T1, T2, T3, T4> : ITuple
    {
        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
        }

        public T1 Item1 { get; }

        public T2 Item2 { get; }

        public T3 Item3 { get; }

        public T4 Item4 { get; }

        protected bool Equals(Tuple<T1, T2, T3, T4> other)
        {
            return EqualityComparer<T1>.Default.Equals(Item1, other.Item1) && EqualityComparer<T2>.Default.Equals(Item2, other.Item2)
                && EqualityComparer<T3>.Default.Equals(Item3, other.Item3) && EqualityComparer<T4>.Default.Equals(Item4, other.Item4);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Tuple<T1, T2, T3, T4>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = EqualityComparer<T1>.Default.GetHashCode(Item1);
                hashCode = (hashCode * 397) ^ EqualityComparer<T2>.Default.GetHashCode(Item2);
                hashCode = (hashCode * 397) ^ EqualityComparer<T3>.Default.GetHashCode(Item3);
                hashCode = (hashCode * 397) ^ EqualityComparer<T4>.Default.GetHashCode(Item4);
                return hashCode;
            }
        }
    }

    public class Tuple<T1, T2, T3, T4, T5> : ITuple
    {
        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
        }

        public T1 Item1 { get; }

        public T2 Item2 { get; }

        public T3 Item3 { get; }

        public T4 Item4 { get; }

        public T5 Item5 { get; }

        protected bool Equals(Tuple<T1, T2, T3, T4, T5> other)
        {
            return EqualityComparer<T1>.Default.Equals(Item1, other.Item1) && EqualityComparer<T2>.Default.Equals(Item2, other.Item2)
                && EqualityComparer<T3>.Default.Equals(Item3, other.Item3) && EqualityComparer<T4>.Default.Equals(Item4, other.Item4)
                && EqualityComparer<T5>.Default.Equals(Item5, other.Item5);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Tuple<T1, T2, T3, T4, T5>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = EqualityComparer<T1>.Default.GetHashCode(Item1);
                hashCode = (hashCode * 397) ^ EqualityComparer<T2>.Default.GetHashCode(Item2);
                hashCode = (hashCode * 397) ^ EqualityComparer<T3>.Default.GetHashCode(Item3);
                hashCode = (hashCode * 397) ^ EqualityComparer<T4>.Default.GetHashCode(Item4);
                hashCode = (hashCode * 397) ^ EqualityComparer<T5>.Default.GetHashCode(Item5);
                return hashCode;
            }
        }
    }

    public class Tuple<T1, T2, T3, T4, T5, T6> : ITuple
    {
        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
        }

        public T1 Item1 { get; }

        public T2 Item2 { get; }

        public T3 Item3 { get; }

        public T4 Item4 { get; }

        public T5 Item5 { get; }

        public T6 Item6 { get; }

        protected bool Equals(Tuple<T1, T2, T3, T4, T5, T6> other)
        {
            return EqualityComparer<T1>.Default.Equals(Item1, other.Item1) && EqualityComparer<T2>.Default.Equals(Item2, other.Item2)
                && EqualityComparer<T3>.Default.Equals(Item3, other.Item3) && EqualityComparer<T4>.Default.Equals(Item4, other.Item4)
                && EqualityComparer<T5>.Default.Equals(Item5, other.Item5) && EqualityComparer<T6>.Default.Equals(Item6, other.Item6);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Tuple<T1, T2, T3, T4, T5, T6>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = EqualityComparer<T1>.Default.GetHashCode(Item1);
                hashCode = (hashCode * 397) ^ EqualityComparer<T2>.Default.GetHashCode(Item2);
                hashCode = (hashCode * 397) ^ EqualityComparer<T3>.Default.GetHashCode(Item3);
                hashCode = (hashCode * 397) ^ EqualityComparer<T4>.Default.GetHashCode(Item4);
                hashCode = (hashCode * 397) ^ EqualityComparer<T5>.Default.GetHashCode(Item5);
                hashCode = (hashCode * 397) ^ EqualityComparer<T6>.Default.GetHashCode(Item6);
                return hashCode;
            }
        }
    }

    public class Tuple<T1, T2, T3, T4, T5, T6, T7> : ITuple
    {
        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Item7 = item7;
        }

        public T1 Item1 { get; }

        public T2 Item2 { get; }

        public T3 Item3 { get; }

        public T4 Item4 { get; }

        public T5 Item5 { get; }

        public T6 Item6 { get; }

        public T7 Item7 { get; }

        protected bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7> other)
        {
            return EqualityComparer<T1>.Default.Equals(Item1, other.Item1) && EqualityComparer<T2>.Default.Equals(Item2, other.Item2)
                && EqualityComparer<T3>.Default.Equals(Item3, other.Item3) && EqualityComparer<T4>.Default.Equals(Item4, other.Item4)
                && EqualityComparer<T5>.Default.Equals(Item5, other.Item5) && EqualityComparer<T6>.Default.Equals(Item6, other.Item6)
                && EqualityComparer<T7>.Default.Equals(Item7, other.Item7);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Tuple<T1, T2, T3, T4, T5, T6, T7>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = EqualityComparer<T1>.Default.GetHashCode(Item1);
                hashCode = (hashCode * 397) ^ EqualityComparer<T2>.Default.GetHashCode(Item2);
                hashCode = (hashCode * 397) ^ EqualityComparer<T3>.Default.GetHashCode(Item3);
                hashCode = (hashCode * 397) ^ EqualityComparer<T4>.Default.GetHashCode(Item4);
                hashCode = (hashCode * 397) ^ EqualityComparer<T5>.Default.GetHashCode(Item5);
                hashCode = (hashCode * 397) ^ EqualityComparer<T6>.Default.GetHashCode(Item6);
                hashCode = (hashCode * 397) ^ EqualityComparer<T7>.Default.GetHashCode(Item7);
                return hashCode;
            }
        }
    }

    public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8> : ITuple
    {
        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Item7 = item7;
            Item8 = item8;
        }

        public T1 Item1 { get; }

        public T2 Item2 { get; }

        public T3 Item3 { get; }

        public T4 Item4 { get; }

        public T5 Item5 { get; }

        public T6 Item6 { get; }

        public T7 Item7 { get; }

        public T8 Item8 { get; }

        protected bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8> other)
        {
            return EqualityComparer<T1>.Default.Equals(Item1, other.Item1) && EqualityComparer<T2>.Default.Equals(Item2, other.Item2)
                && EqualityComparer<T3>.Default.Equals(Item3, other.Item3) && EqualityComparer<T4>.Default.Equals(Item4, other.Item4)
                && EqualityComparer<T5>.Default.Equals(Item5, other.Item5) && EqualityComparer<T6>.Default.Equals(Item6, other.Item6)
                && EqualityComparer<T7>.Default.Equals(Item7, other.Item7) && EqualityComparer<T8>.Default.Equals(Item8, other.Item8);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Tuple<T1, T2, T3, T4, T5, T6, T7, T8>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = EqualityComparer<T1>.Default.GetHashCode(Item1);
                hashCode = (hashCode * 397) ^ EqualityComparer<T2>.Default.GetHashCode(Item2);
                hashCode = (hashCode * 397) ^ EqualityComparer<T3>.Default.GetHashCode(Item3);
                hashCode = (hashCode * 397) ^ EqualityComparer<T4>.Default.GetHashCode(Item4);
                hashCode = (hashCode * 397) ^ EqualityComparer<T5>.Default.GetHashCode(Item5);
                hashCode = (hashCode * 397) ^ EqualityComparer<T6>.Default.GetHashCode(Item6);
                hashCode = (hashCode * 397) ^ EqualityComparer<T7>.Default.GetHashCode(Item7);
                hashCode = (hashCode * 397) ^ EqualityComparer<T8>.Default.GetHashCode(Item8);
                return hashCode;
            }
        }
    }

    public class Tuple : ITuple
    {

        protected bool Equals(Tuple other)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Tuple)obj);
        }

        public override int GetHashCode()
        {
            return string.Empty.GetHashCode();
        }

        public static Tuple Create()
        {
            return new Tuple();
        }

        public static Tuple<T1>
            Create<T1>(T1 item1)
        {
            return new Tuple<T1>
                (item1);
        }

        public static Tuple<T1, T2>
            Create<T1, T2>(T1 item1, T2 item2)
        {
            return new Tuple<T1, T2>
                (item1, item2);
        }

        public static Tuple<T1, T2, T3>
            Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
        {
            return new Tuple<T1, T2, T3>
                (item1, item2, item3);
        }

        public static Tuple<T1, T2, T3, T4>
            Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            return new Tuple<T1, T2, T3, T4>
                (item1, item2, item3, item4);
        }

        public static Tuple<T1, T2, T3, T4, T5>
            Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
        {
            return new Tuple<T1, T2, T3, T4, T5>
                (item1, item2, item3, item4, item5);
        }

        public static Tuple<T1, T2, T3, T4, T5, T6>
            Create<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
        {
            return new Tuple<T1, T2, T3, T4, T5, T6>
                (item1, item2, item3, item4, item5, item6);
        }

        public static Tuple<T1, T2, T3, T4, T5, T6, T7>
            Create<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
        {
            return new Tuple<T1, T2, T3, T4, T5, T6, T7>
                (item1, item2, item3, item4, item5, item6, item7);
        }

        public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8>
            Create<T1, T2, T3, T4, T5, T6, T7, T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
        {
            return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8>
                (item1, item2, item3, item4, item5, item6, item7, item8);
        }
    }
}