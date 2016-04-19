using System;
using iproto.Data;
using iproto.Data.Bodies;
using iproto.Data.Packets;
using iproto.Data.UpdateOperations;
using iproto.Interfaces;

namespace iproto.Services
{
    public class RequestFactory : IRequestFactory
    {
        public RequestPacket CreateAuthentication(GreetingsPacket greetings, string username, string password)
        {
            var scrable = GetScrable(greetings, password);
            var header = new Header(CommandCode.Auth, 0, 0);
            var body = new AuthenticationBody(username, scrable);
            var authenticationPacket = new RequestPacket(header, body);
            return authenticationPacket;
        }


        public RequestPacket CreateSelect<T>(int spaceId, int indexId, int limit, int offset, Iterator iterator, SelectKey<T> selectKey)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateInsert<T1>(int spaceId, Tuple<T1> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateInsert<T1, T2>(int spaceId, Tuple<T1, T2> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateInsert<T1, T2, T3>(int spaceId, Tuple<T1, T2, T3> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateInsert<T1, T2, T3, T4>(int spaceId, Tuple<T1, T2, T3, T4> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateInsert<T1, T2, T3, T4, T5>(int spaceId, Tuple<T1, T2, T3, T4, T5> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateInsert<T1, T2, T3, T4, T5, T6>(int spaceId, Tuple<T1, T2, T3, T4, T5, T6> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateInsert<T1, T2, T3, T4, T5, T6, T7>(int spaceId, Tuple<T1, T2, T3, T4, T5, T6, T7> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateInsert<T1, T2, T3, T4, T5, T6, T7, TRest>(int spaceId, Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateReplace<T1>(int spaceId, Tuple<T1> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateReplace<T1, T2>(int spaceId, Tuple<T1, T2> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateReplace<T1, T2, T3>(int spaceId, Tuple<T1, T2, T3> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateReplace<T1, T2, T3, T4>(int spaceId, Tuple<T1, T2, T3, T4> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateReplace<T1, T2, T3, T4, T5>(int spaceId, Tuple<T1, T2, T3, T4, T5> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateReplace<T1, T2, T3, T4, T5, T6>(int spaceId, Tuple<T1, T2, T3, T4, T5, T6> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateReplace<T1, T2, T3, T4, T5, T6, T7>(int spaceId, Tuple<T1, T2, T3, T4, T5, T6, T7> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateReplace<T1, T2, T3, T4, T5, T6, T7, TRest>(int spaceId, Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateCall<T1>(string functionName, Tuple<T1> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateCall<T1, T2>(string functionName, Tuple<T1, T2> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateCall<T1, T2, T3>(string functionName, Tuple<T1, T2, T3> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateCall<T1, T2, T3, T4>(string functionName, Tuple<T1, T2, T3, T4> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateCall<T1, T2, T3, T4, T5>(string functionName, Tuple<T1, T2, T3, T4, T5> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateCall<T1, T2, T3, T4, T5, T6>(string functionName, Tuple<T1, T2, T3, T4, T5, T6> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateCall<T1, T2, T3, T4, T5, T6, T7>(string functionName, Tuple<T1, T2, T3, T4, T5, T6, T7> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateCall<T1, T2, T3, T4, T5, T6, T7, TRest>(string functionName, Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateEval<T1>(string expression, Tuple<T1> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateEval<T1, T2>(string expression, Tuple<T1, T2> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateEval<T1, T2, T3>(string expression, Tuple<T1, T2, T3> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateEval<T1, T2, T3, T4>(string expression, Tuple<T1, T2, T3, T4> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateEval<T1, T2, T3, T4, T5>(string expression, Tuple<T1, T2, T3, T4, T5> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateEval<T1, T2, T3, T4, T5, T6>(string expression, Tuple<T1, T2, T3, T4, T5, T6> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateEval<T1, T2, T3, T4, T5, T6, T7>(string expression, Tuple<T1, T2, T3, T4, T5, T6, T7> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateEval<T1, T2, T3, T4, T5, T6, T7, TRest>(string expression, Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateUpsert<T1, TArg>(int spaceId, Tuple<T1> tuple, UpdateOperation<TArg> upsertOperation)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateUpsert<T1, T2, TArg>(int spaceId, Tuple<T1, T2> tuple, UpdateOperation<TArg> upsertOperation)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateUpsert<T1, T2, T3, TArg>(int spaceId, Tuple<T1, T2, T3> tuple, UpdateOperation<TArg> upsertOperation)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateUpsert<T1, T2, T3, T4, TArg>(int spaceId, Tuple<T1, T2, T3, T4> tuple, UpdateOperation<TArg> upsertOperation)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateUpsert<T1, T2, T3, T4, T5, TArg>(int spaceId, Tuple<T1, T2, T3, T4, T5> tuple, UpdateOperation<TArg> upsertOperation)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateUpsert<T1, T2, T3, T4, T5, T6, TArg>(int spaceId, Tuple<T1, T2, T3, T4, T5, T6> tuple, UpdateOperation<TArg> upsertOperation)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateUpsert<T1, T2, T3, T4, T5, T6, T7, TArg>(int spaceId, Tuple<T1, T2, T3, T4, T5, T6, T7> tuple, UpdateOperation<TArg> upsertOperation)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateUpsert<T1, T2, T3, T4, T5, T6, T7, TRest, TArg>(int spaceId, Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple, UpdateOperation<TArg> upsertOperation)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateUpdate<T, TArg>(int spaceId, int indexId, SelectKey<T> selectKey, UpdateOperation<TArg> updateOperation)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateDelete<T>(int spaceId, int indexId, SelectKey<T> selectKey)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateJoin(string serverUuid)
        {
            throw new NotImplementedException();
        }

        public RequestPacket CreateSubscribe(string serverUuid, string clusterUuid, int vclock)
        {
            throw new NotImplementedException();
        }


        private static byte[] GetScrable(GreetingsPacket greetings, string password)
        {
            var decodedSalt = greetings.Salt;
            var first20SaltBytes = new byte[20];
            Array.Copy(decodedSalt, first20SaltBytes, 20);

            var step1 = Sha1Utils.Hash(password);
            var step2 = Sha1Utils.Hash(step1);
            var step3 = Sha1Utils.Hash(first20SaltBytes, step2);
            var scrambleBytes = Sha1Utils.Xor(step1, step3);

            return scrambleBytes;
        }
    }
}