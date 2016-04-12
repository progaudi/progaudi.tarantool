using System;
using iproto.Data;
using iproto.Data.Packets;
using iproto.Data.UpdateOperations;

namespace iproto.Interfaces
{
    public interface IRequestFactory
    {
        UnifiedPacket CreateAuthentication(GreetingsPacket greetings, string username, string password);

        UnifiedPacket CreateSelect<T>(int spaceId, int indexId, int limit, int offset, Iterator iterator, SelectKey<T> selectKey);

        #region CreateInsert 

        UnifiedPacket CreateInsert<T1>(int spaceId, Tuple<T1> tuple);
        UnifiedPacket CreateInsert<T1, T2>(int spaceId, Tuple<T1, T2> tuple);
        UnifiedPacket CreateInsert<T1, T2, T3>(int spaceId, Tuple<T1, T2, T3> tuple);
        UnifiedPacket CreateInsert<T1, T2, T3, T4>(int spaceId, Tuple<T1, T2, T3, T4> tuple);
        UnifiedPacket CreateInsert<T1, T2, T3, T4, T5>(int spaceId, Tuple<T1, T2, T3, T4, T5> tuple);
        UnifiedPacket CreateInsert<T1, T2, T3, T4, T5, T6>(int spaceId, Tuple<T1, T2, T3, T4, T5, T6> tuple);
        UnifiedPacket CreateInsert<T1, T2, T3, T4, T5, T6, T7>(int spaceId, Tuple<T1, T2, T3, T4, T5, T6, T7> tuple);
        UnifiedPacket CreateInsert<T1, T2, T3, T4, T5, T6, T7, TRest>(int spaceId, Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple);

        #endregion

        #region CreateReplace 

        UnifiedPacket CreateReplace<T1>(int spaceId, Tuple<T1> tuple);
        UnifiedPacket CreateReplace<T1, T2>(int spaceId, Tuple<T1, T2> tuple);
        UnifiedPacket CreateReplace<T1, T2, T3>(int spaceId, Tuple<T1, T2, T3> tuple);
        UnifiedPacket CreateReplace<T1, T2, T3, T4>(int spaceId, Tuple<T1, T2, T3, T4> tuple);
        UnifiedPacket CreateReplace<T1, T2, T3, T4, T5>(int spaceId, Tuple<T1, T2, T3, T4, T5> tuple);
        UnifiedPacket CreateReplace<T1, T2, T3, T4, T5, T6>(int spaceId, Tuple<T1, T2, T3, T4, T5, T6> tuple);
        UnifiedPacket CreateReplace<T1, T2, T3, T4, T5, T6, T7>(int spaceId, Tuple<T1, T2, T3, T4, T5, T6, T7> tuple);
        UnifiedPacket CreateReplace<T1, T2, T3, T4, T5, T6, T7, TRest>(int spaceId, Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple);

        #endregion

        #region CreateCall 

        UnifiedPacket CreateCall<T1>(string functionName, Tuple<T1> tuple);
        UnifiedPacket CreateCall<T1, T2>(string functionName, Tuple<T1, T2> tuple);
        UnifiedPacket CreateCall<T1, T2, T3>(string functionName, Tuple<T1, T2, T3> tuple);
        UnifiedPacket CreateCall<T1, T2, T3, T4>(string functionName, Tuple<T1, T2, T3, T4> tuple);
        UnifiedPacket CreateCall<T1, T2, T3, T4, T5>(string functionName, Tuple<T1, T2, T3, T4, T5> tuple);
        UnifiedPacket CreateCall<T1, T2, T3, T4, T5, T6>(string functionName, Tuple<T1, T2, T3, T4, T5, T6> tuple);
        UnifiedPacket CreateCall<T1, T2, T3, T4, T5, T6, T7>(string functionName, Tuple<T1, T2, T3, T4, T5, T6, T7> tuple);
        UnifiedPacket CreateCall<T1, T2, T3, T4, T5, T6, T7, TRest>(string functionName, Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple);

        #endregion

        #region CreateEval 

        UnifiedPacket CreateEval<T1>(string expression, Tuple<T1> tuple);
        UnifiedPacket CreateEval<T1, T2>(string expression, Tuple<T1, T2> tuple);
        UnifiedPacket CreateEval<T1, T2, T3>(string expression, Tuple<T1, T2, T3> tuple);
        UnifiedPacket CreateEval<T1, T2, T3, T4>(string expression, Tuple<T1, T2, T3, T4> tuple);
        UnifiedPacket CreateEval<T1, T2, T3, T4, T5>(string expression, Tuple<T1, T2, T3, T4, T5> tuple);
        UnifiedPacket CreateEval<T1, T2, T3, T4, T5, T6>(string expression, Tuple<T1, T2, T3, T4, T5, T6> tuple);
        UnifiedPacket CreateEval<T1, T2, T3, T4, T5, T6, T7>(string expression, Tuple<T1, T2, T3, T4, T5, T6, T7> tuple);
        UnifiedPacket CreateEval<T1, T2, T3, T4, T5, T6, T7, TRest>(string expression, Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple);

        #endregion

        #region CreateUpsert 

        UnifiedPacket CreateUpsert<T1, TArg>(int spaceId, Tuple<T1> tuple, UpdateOperation<TArg> upsertOperation);
        UnifiedPacket CreateUpsert<T1, T2, TArg>(int spaceId, Tuple<T1, T2> tuple, UpdateOperation<TArg> upsertOperation);
        UnifiedPacket CreateUpsert<T1, T2, T3, TArg>(int spaceId, Tuple<T1, T2, T3> tuple, UpdateOperation<TArg> upsertOperation);
        UnifiedPacket CreateUpsert<T1, T2, T3, T4, TArg>(int spaceId, Tuple<T1, T2, T3, T4> tuple, UpdateOperation<TArg> upsertOperation);
        UnifiedPacket CreateUpsert<T1, T2, T3, T4, T5, TArg>(int spaceId, Tuple<T1, T2, T3, T4, T5> tuple, UpdateOperation<TArg> upsertOperation);
        UnifiedPacket CreateUpsert<T1, T2, T3, T4, T5, T6, TArg>(int spaceId, Tuple<T1, T2, T3, T4, T5, T6> tuple, UpdateOperation<TArg> upsertOperation);
        UnifiedPacket CreateUpsert<T1, T2, T3, T4, T5, T6, T7, TArg>(int spaceId, Tuple<T1, T2, T3, T4, T5, T6, T7> tuple, UpdateOperation<TArg> upsertOperation);
        UnifiedPacket CreateUpsert<T1, T2, T3, T4, T5, T6, T7, TRest, TArg>(int spaceId, Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple, UpdateOperation<TArg> upsertOperation);

        #endregion

        UnifiedPacket CreateUpdate<T, TArg>(int spaceId, int indexId, SelectKey<T> selectKey, UpdateOperation<TArg> updateOperation);
        UnifiedPacket CreateDelete<T>(int spaceId, int indexId, SelectKey<T> selectKey);
        UnifiedPacket CreateJoin(string serverUuid);
        UnifiedPacket CreateSubscribe(string serverUuid, string clusterUuid, int vclock);
    }
}