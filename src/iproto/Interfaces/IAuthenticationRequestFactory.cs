using iproto.Data.Packets;

namespace iproto.Interfaces
{
    public interface IRequestFactory
    {
        AuthenticationPacket CreateAuthentication(GreetingsPacket greetings, string username, string password);
    }
}