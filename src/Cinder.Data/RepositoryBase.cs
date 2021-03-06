﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cinder.Documents;
using Cinder.Extensions;
using MongoDB.Driver;

namespace Cinder.Data
{
    public abstract class RepositoryBase<TDocument> : IRepository where TDocument : IDocument
    {
        protected readonly IMongoClient Client;
        protected readonly IMongoCollection<TDocument> Collection;
        protected readonly IMongoDatabase Database;

        protected RepositoryBase(IMongoClient client, string databaseName, CollectionName collectionName)
        {
            Client = client;
            Database = Client.GetDatabase(databaseName);
            Collection = Database.GetCollection<TDocument>(collectionName.ToCollectionName());
        }

        protected virtual FilterDefinition<TDocument> CreateDocumentFilter(string id)
        {
            id = id.ToLowerInvariant();

            return Builders<TDocument>.Filter.Eq(document => document.Id, id);
        }

        protected virtual FilterDefinition<TDocument> CreateDocumentFilter(TDocument entity)
        {
            return Builders<TDocument>.Filter.Eq(document => document.Id, entity.Id);
        }

        protected async Task UpsertDocumentAsync(TDocument updatedDocument, CancellationToken cancellationToken = default)
        {
            await Collection.ReplaceOneAsync(CreateDocumentFilter(updatedDocument), updatedDocument,
                    new UpdateOptions {IsUpsert = true}, cancellationToken)
                .ConfigureAwait(false);
        }

        protected async Task<int> BulkUpsertDocumentAsync(IEnumerable<TDocument> updatedDocuments,
            CancellationToken cancellationToken = default)
        {
            List<WriteModel<TDocument>> requests = updatedDocuments
                .Select(document =>
                    new ReplaceOneModel<TDocument>(Builders<TDocument>.Filter.Where(x => x.Id == document.Id), document)
                    {
                        IsUpsert = true
                    })
                .Cast<WriteModel<TDocument>>()
                .ToList();

            BulkWriteResult<TDocument> result = await Collection.BulkWriteAsync(requests, cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return result.ProcessedRequests.Count;
        }
    }
}
