﻿using System.Numerics;
using System.Threading.Tasks;
using Cinder.Documents;
using MongoDB.Driver;

namespace Cinder.Data.Repositories
{
    public class BlockProgressRepository : RepositoryBase<CinderBlockProgress>, IBlockProgressRepository
    {
        public BlockProgressRepository(IMongoClient client, string databaseName) : base(client, databaseName,
            CollectionName.BlockProgress) { }

        public async Task UpsertProgressAsync(BigInteger blockNumber)
        {
            CinderBlockProgress block = new CinderBlockProgress {LastBlockProcessed = blockNumber.ToString()};
            block.UpdateRowDates();
            await UpsertDocumentAsync(block).ConfigureAwait(false);
        }

        public async Task<BigInteger?> GetLastBlockNumberProcessedAsync()
        {
            long count = await Collection.CountDocumentsAsync(FilterDefinition<CinderBlockProgress>.Empty).ConfigureAwait(false);

            if (count == 0)
            {
                return null;
            }

            string max = await Collection.Find(FilterDefinition<CinderBlockProgress>.Empty)
                .Limit(1)
                .Sort(new SortDefinitionBuilder<CinderBlockProgress>().Descending(block => block.LastBlockProcessed))
                .Project(block => block.LastBlockProcessed)
                .SingleOrDefaultAsync()
                .ConfigureAwait(false);

            return BigInteger.Parse(max);
        }
    }
}
