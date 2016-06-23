namespace Tarantool.Client.Model.Enums
{
    public enum Iterator : uint
    {
        Eq = 0,
        Req = 1,
        All = 2,
        Lt = 3,
        Le = 4,
        Ge = 5,
        Gt = 6,
        BitsAllSet = 7,
        BitsAnySet = 8,
        BitsAllNotSet = 9,
        Overlaps = 10,
        Neighbour = 11,
    }
}