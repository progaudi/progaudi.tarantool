using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Requests;
using ProGaudi.Tarantool.Client.Model.Responses;
using ProGaudi.Tarantool.Client.Model.UpdateOperations;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client
{
    public class Space : ISpace
    {
        private const int VIndex = 0x121;

        private const int IndexById = 0;

        private const int IndexByName = 2;

        private const uint PrimaryIndexId = 0;

        public ILogicalConnection LogicalConnection { get; set; }

        public Space(uint id, uint fieldCount, string name, IReadOnlyCollection<Index> indices, StorageEngine engine, IReadOnlyCollection<SpaceField> fields)
        {
            Id = id;
            FieldCount = fieldCount;
            Name = name;
            Indices = indices;
            Engine = engine;
            Fields = fields;
        }

        public uint Id { get; }

        public uint FieldCount { get; }

        public string Name { get; }

        public StorageEngine Engine { get; }

        public IReadOnlyCollection<Index> Indices { get; }

        public IReadOnlyCollection<SpaceField> Fields { get; }

        public Task CreateIndex()
        {
            throw new NotImplementedException();
        }

        public Task Drop()
        {
            throw new NotImplementedException();
        }

        public Task Rename(string newName)
        {
            throw new NotImplementedException();
        }

        public async Task<Index> GetIndex(string indexName)
        {
            var selectIndexRequest = new SelectRequest<TarantoolTuple<uint, string>>(VIndex, IndexByName, uint.MaxValue, 0, Iterator.Eq, TarantoolTuple.Create(Id, indexName));

            var response = await LogicalConnection.SendRequest<SelectRequest<TarantoolTuple<uint, string>>, Index>(selectIndexRequest);

            var result = response.Data.SingleOrDefault();

            if (result == null)
            {
                throw ExceptionHelper.InvalidIndexName(indexName, ToString());
            }

            result.LogicalConnection = LogicalConnection;

            return result;
        }

        public async Task<Index> GetIndex(uint indexId)
        {
            var selectIndexRequest = new SelectRequest<TarantoolTuple<uint, uint>>(VIndex, IndexById, uint.MaxValue, 0, Iterator.Eq, TarantoolTuple.Create(Id, indexId));

            var response = await LogicalConnection.SendRequest<SelectRequest<TarantoolTuple<uint, uint>>, Index>(selectIndexRequest);

            var result = response.Data.SingleOrDefault();

            if (result == null)
            {
                throw ExceptionHelper.InvalidIndexId(indexId, ToString());
            }

            result.LogicalConnection = LogicalConnection;

            return result;
        }

        public async Task<DataResponse<TTuple[]>> Insert<TTuple>(TTuple tuple)
            where TTuple : ITarantoolTuple
        {
            var insertRequest = new InsertRequest<TTuple>(Id, tuple);
            return await LogicalConnection.SendRequest<InsertReplaceRequest<TTuple>, TTuple>(insertRequest);
        }

        public async Task<DataResponse<TTuple[]>> Select<TKey, TTuple>(TKey selectKey)
          where TKey : ITarantoolTuple
          where TTuple : ITarantoolTuple
        {
            var selectRequest = new SelectRequest<TKey>(Id, PrimaryIndexId, uint.MaxValue, 0, Iterator.Eq, selectKey);
            return await LogicalConnection.SendRequest<SelectRequest<TKey>, TTuple>(selectRequest);
        }

        public async Task<TTuple> Get<TKey, TTuple>(TKey key)
            where TKey : ITarantoolTuple
          where TTuple : ITarantoolTuple
        {
            var selectRequest = new SelectRequest<TKey>(Id, PrimaryIndexId, 1, 0, Iterator.Eq, key);
            var response = await LogicalConnection.SendRequest<SelectRequest<TKey>, TTuple>(selectRequest);
            return response.Data.SingleOrDefault();
        }

        public async Task<DataResponse<TTuple[]>> Replace<TTuple>(TTuple tuple)
            where TTuple : ITarantoolTuple
        {
            var replaceRequest = new ReplaceRequest<TTuple>(Id, tuple);
            return await LogicalConnection.SendRequest<InsertReplaceRequest<TTuple>, TTuple>(replaceRequest);
        }

        public async Task<T> Put<T>(T tuple)
            where T : ITarantoolTuple
        {
            var response = await Replace(tuple);
            return response.Data.First();
        }

        public async Task<DataResponse<TTuple[]>> Update<TKey, TTuple>(TKey key, UpdateOperation[] updateOperations)
            where TKey : ITarantoolTuple
            where TTuple : ITarantoolTuple
        {
            var updateRequest = new UpdateRequest<TKey>(Id, PrimaryIndexId, key, updateOperations);
            return await LogicalConnection.SendRequest<UpdateRequest<TKey>, TTuple>(updateRequest);
        }

        public async Task Upsert<TTuple>(TTuple tuple, UpdateOperation[] updateOperations)
         where TTuple : ITarantoolTuple
        {
            var upsertRequest = new UpsertRequest<TTuple>(Id, tuple, updateOperations);
            await LogicalConnection.SendRequestWithEmptyResponse(upsertRequest);
        }

        public async Task<DataResponse<TTuple[]>> Delete<TKey, TTuple>(TKey key)
           where TTuple : ITarantoolTuple
           where TKey : ITarantoolTuple
        {
            var deleteRequest = new DeleteRequest<TKey>(Id, PrimaryIndexId, key);
            return await LogicalConnection.SendRequest<DeleteRequest<TKey>, TTuple>(deleteRequest);
        }

        public Task<uint> Count<TKey>(TKey key)
           where TKey : ITarantoolTuple
        {
            throw new NotImplementedException();
        }

        public Task<uint> Length()
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<TTuple[]>> Increment<TTuple, TKey>(TKey key)
            where TKey : ITarantoolTuple
            where TTuple : ITarantoolTuple
        {
            // Currently we can't impelment that method because Upsert returns void.
           throw new NotImplementedException();
        }

        public Task<DataResponse<TTuple[]>> Decrement<TTuple, TKey>(TKey key)
            where TKey : ITarantoolTuple
            where TTuple : ITarantoolTuple
        {
            // Currently we can't impelment that method because Upsert returns void.
            throw new NotImplementedException();
        }

        public TTuple AutoIncrement<TTuple, TRest>(TRest tupleRest)
            where TTuple : ITarantoolTuple
            where TRest : ITarantoolTuple
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<KeyValuePair<TKey, TValue>>> Pairs<TKey, TValue>()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"{Name}, id={Id}";
        }
    }
}