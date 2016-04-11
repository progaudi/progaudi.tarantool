using iproto.Data;
using iproto.Data.Packets;
using iproto.Data.UpdateOperations;

namespace iproto.Interfaces
{
    public interface IRequestFactory
    {
        UnifiedPacket CreateAuthentication(GreetingsPacket greetings, string username, string password);
        UnifiedPacket CreateSelect<T>(int spaceId, int indexId, int limit, int offset, Iterator iterator, SelectKey<T> selectKey);
        UnifiedPacket CreateInsert<T>(int spaceId, T tuple);
        UnifiedPacket CreateReplace<T>(int spaceId, T tuple);
        UnifiedPacket CreateUpdate<T, TArg>(int spaceId, int indexId, SelectKey<T> selectKey, UpdateOperation<TArg> updateOperation);
        UnifiedPacket CreateDelete<T>(int spaceId, int indexId, SelectKey<T> selectKey);
        UnifiedPacket CreateCall<T>(string functionName, T tuple);
        UnifiedPacket CreateEval<T>(string expression, T tuple);
        UnifiedPacket CreateUpsert<T, TArg>(int spaceId, T tiple, UpdateOperation<TArg> upsertOperation);
        UnifiedPacket CreateJoin(string serverUuid);
        UnifiedPacket CreateSubscribe(string serverUuid, string clusterUuid, int vclock);
    }
}