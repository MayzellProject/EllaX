﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Cinder.Api.Infrastructure.Features.Transaction;
using Cinder.Core.Paging;
using Cinder.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cinder.Api.Controllers
{
    public class TransactionController : BaseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(GetTransactions.Model), StatusCodes.Status200OK)]
        public async Task<IPage<GetTransactions.Model>> GetTransactions(int? page, int? size,
            SortOrder sort = SortOrder.Ascending)
        {
            return await Mediator.Send(new GetTransactions.Query {Page = page, Size = size, Sort = sort}).ConfigureAwait(false);
        }

        [HttpGet("{hash}")]
        [ProducesResponseType(typeof(GetTransactionByHash.Model), StatusCodes.Status200OK)]
        public async Task<GetTransactionByHash.Model> GetTransactionByHash(string hash)
        {
            return await Mediator.Send(new GetTransactionByHash.Query {Hash = hash}).ConfigureAwait(false);
        }

        [HttpGet("block/{hash}")]
        [ProducesResponseType(typeof(GetTransactionsByBlockHash.Model), StatusCodes.Status200OK)]
        public async Task<IEnumerable<GetTransactionsByBlockHash.Model>> GetTransactionsByBlockHash(string hash)
        {
            return await Mediator.Send(new GetTransactionsByBlockHash.Query {BlockHash = hash}).ConfigureAwait(false);
        }

        [HttpGet("address/{hash}")]
        [ProducesResponseType(typeof(GetTransactionsByAddressHash.Model), StatusCodes.Status200OK)]
        public async Task<IPage<GetTransactionsByAddressHash.Model>> GetTransactionsByAddressHash(string hash, int? page,
            int? size, SortOrder sort = SortOrder.Ascending)
        {
            return await Mediator.Send(
                    new GetTransactionsByAddressHash.Query {AddressHash = hash, Page = page, Size = size, Sort = sort})
                .ConfigureAwait(false);
        }

        [HttpGet("address/{hash}/recent")]
        [ProducesResponseType(typeof(GetRecentTransactionsByAddressHash.Model), StatusCodes.Status200OK)]
        public async Task<IEnumerable<GetRecentTransactionsByAddressHash.Model>> GetRecentTransactionsByAddressHash(string hash,
            int? size)
        {
            return await Mediator.Send(new GetRecentTransactionsByAddressHash.Query {AddressHash = hash, Size = size})
                .ConfigureAwait(false);
        }
    }
}
