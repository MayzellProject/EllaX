﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cinder.Documents;

namespace Cinder.Data.Repositories
{
    public interface IAddressRepository : IRepository
    {
        Task UpsertAddress(CinderAddress address, CancellationToken cancellationToken = default);
        Task<CinderAddress> GetAddressByHash(string hash, CancellationToken cancellationToken = default);

        Task<IEnumerable<CinderAddress>> GetStaleAddresses(int age = 5, int limit = 100,
            CancellationToken cancellationToken = default);
    }
}
