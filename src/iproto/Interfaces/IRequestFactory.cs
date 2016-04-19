using System;
using iproto.Data;
using iproto.Data.Packets;
using iproto.Data.UpdateOperations;

namespace iproto.Interfaces
{
    public interface IRequestFactory
    {
        RequestPacket CreateAuthentication(GreetingsPacket greetings, string username, string password);

        RequestPacket CreateSelect<T>(int spaceId, int indexId, int limit, int offset, Iterator iterator, SelectKey<T> selectKey);

        #region CreateInsert 

        RequestPacket CreateInsert<T1>(int spaceId, Tuple<T1> tuple);
        RequestPacket CreateInsert<T1, T2>(int spaceId, Tuple<T1, T2> tuple);
        RequestPacket CreateInsert<T1, T2, T3>(int spaceId, Tuple<T1, T2, T3> tuple);
        RequestPacket CreateInsert<T1, T2, T3, T4>(int spaceId, Tuple<T1, T2, T3, T4> tuple);
        RequestPacket CreateInsert<T1, T2, T3, T4, T5>(int spaceId, Tuple<T1, T2, T3, T4, T5> tuple);
        RequestPacket CreateInsert<T1, T2, T3, T4, T5, T6>(int spaceId, Tuple<T1, T2, T3, T4, T5, T6> tuple);
        RequestPacket CreateInsert<T1, T2, T3, T4, T5, T6, T7>(int spaceId, Tuple<T1, T2, T3, T4, T5, T6, T7> tuple);
        RequestPacket CreateInsert<T1, T2, T3, T4, T5, T6, T7, TRest>(int spaceId, Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple);

        #endregion

        #region CreateReplace 

        RequestPacket CreateReplace<T1>(int spaceId, Tuple<T1> tuple);
        RequestPacket CreateReplace<T1, T2>(int spaceId, Tuple<T1, T2> tuple);
        RequestPacket CreateReplace<T1, T2, T3>(int spaceId, Tuple<T1, T2, T3> tuple);
        RequestPacket CreateReplace<T1, T2, T3, T4>(int spaceId, Tuple<T1, T2, T3, T4> tuple);
        RequestPacket CreateReplace<T1, T2, T3, T4, T5>(int spaceId, Tuple<T1, T2, T3, T4, T5> tuple);
        RequestPacket CreateReplace<T1, T2, T3, T4, T5, T6>(int spaceId, Tuple<T1, T2, T3, T4, T5, T6> tuple);
        RequestPacket CreateReplace<T1, T2, T3, T4, T5, T6, T7>(int spaceId, Tuple<T1, T2, T3, T4, T5, T6, T7> tuple);
        RequestPacket CreateReplace<T1, T2, T3, T4, T5, T6, T7, TRest>(int spaceId, Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple);

        #endregion

        #region CreateCall 

        RequestPacket CreateCall<T1>(string functionName, Tuple<T1> tuple);
        RequestPacket CreateCall<T1, T2>(string functionName, Tuple<T1, T2> tuple);
        RequestPacket CreateCall<T1, T2, T3>(string functionName, Tuple<T1, T2, T3> tuple);
        RequestPacket CreateCall<T1, T2, T3, T4>(string functionName, Tuple<T1, T2, T3, T4> tuple);
        RequestPacket CreateCall<T1, T2, T3, T4, T5>(string functionName, Tuple<T1, T2, T3, T4, T5> tuple);
        RequestPacket CreateCall<T1, T2, T3, T4, T5, T6>(string functionName, Tuple<T1, T2, T3, T4, T5, T6> tuple);
        RequestPacket CreateCall<T1, T2, T3, T4, T5, T6, T7>(string functionName, Tuple<T1, T2, T3, T4, T5, T6, T7> tuple);
        RequestPacket CreateCall<T1, T2, T3, T4, T5, T6, T7, TRest>(string functionName, Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple);

        #endregion

        #region CreateEval 

        RequestPacket CreateEval<T1>(string expression, Tuple<T1> tuple);
        RequestPacket CreateEval<T1, T2>(string expression, Tuple<T1, T2> tuple);
        RequestPacket CreateEval<T1, T2, T3>(string expression, Tuple<T1, T2, T3> tuple);
        RequestPacket CreateEval<T1, T2, T3, T4>(string expression, Tuple<T1, T2, T3, T4> tuple);
        RequestPacket CreateEval<T1, T2, T3, T4, T5>(string expression, Tuple<T1, T2, T3, T4, T5> tuple);
        RequestPacket CreateEval<T1, T2, T3, T4, T5, T6>(string expression, Tuple<T1, T2, T3, T4, T5, T6> tuple);
        RequestPacket CreateEval<T1, T2, T3, T4, T5, T6, T7>(string expression, Tuple<T1, T2, T3, T4, T5, T6, T7> tuple);
        RequestPacket CreateEval<T1, T2, T3, T4, T5, T6, T7, TRest>(string expression, Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple);

        #endregion

        #region CreateUpsert 

        RequestPacket CreateUpsert<T1, TArg>(int spaceId, Tuple<T1> tuple, UpdateOperation<TArg> upsertOperation);
        RequestPacket CreateUpsert<T1, T2, TArg>(int spaceId, Tuple<T1, T2> tuple, UpdateOperation<TArg> upsertOperation);
        RequestPacket CreateUpsert<T1, T2, T3, TArg>(int spaceId, Tuple<T1, T2, T3> tuple, UpdateOperation<TArg> upsertOperation);
        RequestPacket CreateUpsert<T1, T2, T3, T4, TArg>(int spaceId, Tuple<T1, T2, T3, T4> tuple, UpdateOperation<TArg> upsertOperation);
        RequestPacket CreateUpsert<T1, T2, T3, T4, T5, TArg>(int spaceId, Tuple<T1, T2, T3, T4, T5> tuple, UpdateOperation<TArg> upsertOperation);
        RequestPacket CreateUpsert<T1, T2, T3, T4, T5, T6, TArg>(int spaceId, Tuple<T1, T2, T3, T4, T5, T6> tuple, UpdateOperation<TArg> upsertOperation);
        RequestPacket CreateUpsert<T1, T2, T3, T4, T5, T6, T7, TArg>(int spaceId, Tuple<T1, T2, T3, T4, T5, T6, T7> tuple, UpdateOperation<TArg> upsertOperation);
        RequestPacket CreateUpsert<T1, T2, T3, T4, T5, T6, T7, TRest, TArg>(int spaceId, Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple, UpdateOperation<TArg> upsertOperation);

        #endregion

        RequestPacket CreateUpdate<T, TArg>(int spaceId, int indexId, SelectKey<T> selectKey, UpdateOperation<TArg> updateOperation);
        RequestPacket CreateDelete<T>(int spaceId, int indexId, SelectKey<T> selectKey);
        RequestPacket CreateJoin(string serverUuid);
        RequestPacket CreateSubscribe(string serverUuid, string clusterUuid, int vclock);
    }
}