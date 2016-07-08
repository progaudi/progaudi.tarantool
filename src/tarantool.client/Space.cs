using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Tarantool.Client.Model;
using Tarantool.Client.Model.Enums;
using Tarantool.Client.Model.Requests;
using Tarantool.Client.Model.Responses;
using Tarantool.Client.Model.UpdateOperations;
using Tarantool.Client.Utils;

using Tuple = Tarantool.Client.Model.Tuple;

namespace Tarantool.Client
{
    public class Space
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

        public void CreateIndex()
        {
            throw new NotImplementedException();
        }

        public void Drop()
        {
            throw new NotImplementedException();
        }

        public void Rename(string newName)
        {
            throw new NotImplementedException();
        }

        public async Task<Index> GetIndex(string indexName)
        {
            var selectIndexRequest = new SelectRequest<Model.Tuple<uint, string>>(VIndex, IndexByName, uint.MaxValue, 0, Iterator.Eq, Tuple.Create(Id, indexName));

            var response = await LogicalConnection.SendRequest<SelectRequest<Model.Tuple<uint, string>>, Index>(selectIndexRequest);

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
            var selectIndexRequest = new SelectRequest<Model.Tuple<uint, uint>>(VIndex, IndexById, uint.MaxValue, 0, Iterator.Eq, Tuple.Create(Id, indexId));

            var response = await LogicalConnection.SendRequest<SelectRequest<Model.Tuple<uint, uint>>, Index>(selectIndexRequest);

            var result = response.Data.SingleOrDefault();

            if (result == null)
            {
                throw ExceptionHelper.InvalidIndexId(indexId, ToString());
            }

            result.LogicalConnection = LogicalConnection;

            return result;
        }
        
        public async Task<DataResponse<TTuple[]>> Insert<TTuple>(TTuple tuple)
            where TTuple : ITuple
        {
            var insertRequest = new InsertRequest<TTuple>(Id, tuple);
            return await LogicalConnection.SendRequest<InsertReplaceRequest<TTuple>, TTuple>(insertRequest);
        }

        public async Task<DataResponse<TTuple[]>> Select<TKey, TTuple>(TKey selectKey)
          where TKey : ITuple
          where TTuple : ITuple
        {
            var selectRequest = new SelectRequest<TKey>(Id, PrimaryIndexId, uint.MaxValue, 0, Iterator.Eq, selectKey);
            return await LogicalConnection.SendRequest<SelectRequest<TKey>, TTuple>(selectRequest);
        }

        public async Task<TTuple> Get<TKey, TTuple>(TKey key)
            where TKey : ITuple
          where TTuple : ITuple
        {
            var selectRequest = new SelectRequest<TKey>(Id, PrimaryIndexId, 1, 0, Iterator.Eq, key);
            var response = await LogicalConnection.SendRequest<SelectRequest<TKey>, TTuple>(selectRequest);
            return response.Data.SingleOrDefault();
        }

        public async Task<DataResponse<TTuple[]>> Replace<TTuple>(TTuple tuple)
            where TTuple : ITuple
        {
            var replaceRequest = new ReplaceRequest<TTuple>(Id, tuple);
            return await LogicalConnection.SendRequest<InsertReplaceRequest<TTuple>, TTuple>(replaceRequest);
        }

        public async Task<T> Put<T>(T tuple)
            where T : ITuple
        {
            var response = await Replace(tuple);
            return response.Data.First();
        }

        public async Task<DataResponse<TTuple[]>> Update<TKey, TTuple>(TKey key, UpdateOperation[] updateOperations)
            where TKey : ITuple
            where TTuple : ITuple
        {
            var updateRequest = new UpdateRequest<TKey>(Id, PrimaryIndexId, key, updateOperations);
            return await LogicalConnection.SendRequest<UpdateRequest<TKey>, TTuple>(updateRequest);
        }

        public async Task Upsert<TTuple>(TTuple tuple, UpdateOperation[] updateOperations)
         where TTuple : ITuple
        {
            var upsertRequest = new UpsertRequest<TTuple>(Id, tuple, updateOperations);
            await LogicalConnection.SendRequestWithEmptyResponse(upsertRequest);
        }

        public async Task<DataResponse<TTuple[]>> Delete<TKey, TTuple>(TKey key)
           where TTuple : ITuple
           where TKey : ITuple
        {
            var deleteRequest = new DeleteRequest<TKey>(Id, PrimaryIndexId, key);
            return await LogicalConnection.SendRequest<DeleteRequest<TKey>, TTuple>(deleteRequest);
        }

        public uint Count<TKey>(TKey key)
           where TKey : ITuple
        {
            throw new NotImplementedException();
        }

        public uint Length()
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<TTuple[]>> Increment<TTuple, TKey>(TKey key)
            where TKey : ITuple
            where TTuple : ITuple
        {
            // Currently we can't impelment that method because Upsert returns void.
           throw new NotImplementedException();
        }

        public Task<DataResponse<TTuple[]>> Decrement<TTuple, TKey>(TKey key)
            where TKey : ITuple
            where TTuple : ITuple
        {
            // Currently we can't impelment that method because Upsert returns void.
            throw new NotImplementedException();
        }

        public TTuple AutoIncrement<TTuple, TRest>(TRest tupleRest)
            where TTuple : ITuple
            where TRest : ITuple
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> Pairs<TKey, TValue>()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"{Name}, id={Id}";
        }
    }
}