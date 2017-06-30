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

        public Space(uint id, uint fieldCount, string name, IReadOnlyCollection<IIndex> indices, StorageEngine engine, IReadOnlyCollection<SpaceField> fields)
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

        public IReadOnlyCollection<IIndex> Indices { get; }

        public IReadOnlyCollection<SpaceField> Fields { get; }

        public Task<IIndex> CreateIndex()
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

        public async Task<IIndex> GetIndex(string indexName)
        {
            var selectIndexRequest = new SelectRequest<TarantoolTuple<uint, string>>(VIndex, IndexByName, uint.MaxValue, 0, Iterator.Eq, TarantoolTuple.Create(Id, indexName));

            var response = await LogicalConnection.SendRequest<SelectRequest<TarantoolTuple<uint, string>>, Index>(selectIndexRequest).ConfigureAwait(false);

            var result = response.Data.SingleOrDefault();

            if (result == null)
            {
                throw ExceptionHelper.InvalidIndexName(indexName, ToString());
            }

            result.LogicalConnection = LogicalConnection;

            return result;
        }

        public async Task<IIndex> GetIndex(uint indexId)
        {
            var selectIndexRequest = new SelectRequest<TarantoolTuple<uint, uint>>(VIndex, IndexById, uint.MaxValue, 0, Iterator.Eq, TarantoolTuple.Create(Id, indexId));

            var response = await LogicalConnection.SendRequest<SelectRequest<TarantoolTuple<uint, uint>>, Index>(selectIndexRequest).ConfigureAwait(false);

            var result = response.Data.SingleOrDefault();

            if (result == null)
            {
                throw ExceptionHelper.InvalidIndexId(indexId, ToString());
            }

            result.LogicalConnection = LogicalConnection;

            return result;
        }

        public async Task<DataResponse<TTuple[]>> Insert<TTuple>(TTuple tuple)
        {
            var insertRequest = new InsertRequest<TTuple>(Id, tuple);
            return await LogicalConnection.SendRequest<InsertReplaceRequest<TTuple>, TTuple>(insertRequest).ConfigureAwait(false);
        }

        public async Task<DataResponse<TTuple[]>> Select<TKey, TTuple>(TKey selectKey)
        {
            var selectRequest = new SelectRequest<TKey>(Id, PrimaryIndexId, uint.MaxValue, 0, Iterator.Eq, selectKey);
            return await LogicalConnection.SendRequest<SelectRequest<TKey>, TTuple>(selectRequest).ConfigureAwait(false);
        }

        public async Task<TTuple> Get<TKey, TTuple>(TKey key)
        {
            var selectRequest = new SelectRequest<TKey>(Id, PrimaryIndexId, 1, 0, Iterator.Eq, key);
            var response = await LogicalConnection.SendRequest<SelectRequest<TKey>, TTuple>(selectRequest).ConfigureAwait(false);
            return response.Data.SingleOrDefault();
        }

        public async Task<DataResponse<TTuple[]>> Replace<TTuple>(TTuple tuple)
        {
            var replaceRequest = new ReplaceRequest<TTuple>(Id, tuple);
            return await LogicalConnection.SendRequest<InsertReplaceRequest<TTuple>, TTuple>(replaceRequest).ConfigureAwait(false);
        }

        public async Task<T> Put<T>(T tuple)
        {
            var response = await Replace(tuple).ConfigureAwait(false);
            return response.Data.First();
        }

        public async Task<DataResponse<TTuple[]>> Update<TKey, TTuple>(TKey key, UpdateOperation[] updateOperations)
        {
            var updateRequest = new UpdateRequest<TKey>(Id, PrimaryIndexId, key, updateOperations);
            return await LogicalConnection.SendRequest<UpdateRequest<TKey>, TTuple>(updateRequest).ConfigureAwait(false);
        }

        public async Task Upsert<TTuple>(TTuple tuple, UpdateOperation[] updateOperations)
        {
            var upsertRequest = new UpsertRequest<TTuple>(Id, tuple, updateOperations);
            await LogicalConnection.SendRequestWithEmptyResponse(upsertRequest).ConfigureAwait(false);
        }

        public async Task<DataResponse<TTuple[]>> Delete<TKey, TTuple>(TKey key)
        {
            var deleteRequest = new DeleteRequest<TKey>(Id, PrimaryIndexId, key);
            return await LogicalConnection.SendRequest<DeleteRequest<TKey>, TTuple>(deleteRequest).ConfigureAwait(false);
        }

        public Task<uint> Count<TKey>(TKey key)
        {
            throw new NotImplementedException();
        }

        public Task<uint> Length()
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<TTuple[]>> Increment<TTuple, TKey>(TKey key)
        {
            // Currently we can't impelment that method because Upsert returns void.
           throw new NotImplementedException();
        }

        public Task<DataResponse<TTuple[]>> Decrement<TTuple, TKey>(TKey key)
        {
            // Currently we can't impelment that method because Upsert returns void.
            throw new NotImplementedException();
        }

        public TTuple AutoIncrement<TTuple, TRest>(TRest tupleRest)
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