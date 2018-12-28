﻿using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Data;
using FluentValidation;
using MediatR;
using static EllaX.Core.Constants;

namespace EllaX.Application.Features.Peer
{
    public class Create
    {
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(command => command.Id).NotEmpty().Length(Entities.Peer.IdMaxLength);
                RuleFor(command => command.Name).MaximumLength(Entities.Peer.NameMaxLength);
                RuleFor(command => command.LocalAddress).NotEmpty().MaximumLength(Entities.Peer.AddressMaxLength);
                RuleFor(command => command.RemoteAddress).NotEmpty().MaximumLength(Entities.Peer.AddressMaxLength);
                RuleFor(command => command.City).MaximumLength(Entities.Peer.CityMaxLength);
                RuleFor(command => command.Country).MaximumLength(Entities.Peer.CountryMaxLength);
            }
        }

        public class Command : IRequest<Model>
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string LocalAddress { get; set; }
            public string RemoteAddress { get; set; }
            public decimal? Latitude { get; set; }
            public decimal? Longitude { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
        }

        public class Model
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string LocalAddress { get; set; }
            public string RemoteAddress { get; set; }
            public decimal? Latitude { get; set; }
            public decimal? Longitude { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
            public DateTimeOffset FirstSeenDate { get; set; }
            public DateTimeOffset LastSeenDate { get; set; }
        }

        public class Handler : IRequestHandler<Command, Model>
        {
            private readonly ApplicationDbContext _db;
            private readonly IMapper _mapper;

            public Handler(ApplicationDbContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<Model> Handle(Command request, CancellationToken cancellationToken)
            {
                Core.Entities.Peer peer = _mapper.Map<Command, Core.Entities.Peer>(request);
                await _db.Peers.AddAsync(peer, cancellationToken);
                Model model = _mapper.Map<Core.Entities.Peer, Model>(peer);
                await _db.SaveChangesAsync(cancellationToken);

                return model;
            }
        }
    }
}