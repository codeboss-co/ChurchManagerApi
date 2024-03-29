﻿using AutoMapper;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Groups.Queries.GroupMembers
{
    public record GroupMembersQuery(int GroupId) : IRequest<ApiResponse>
    {
        public string RecordStatus { get; set; }
    }

    public class GroupMembersHandler : IRequestHandler<GroupMembersQuery, ApiResponse>
    {
        private readonly IGroupDbRepository _groupDbRepository;
        private readonly IMapper _mapper;

        public GroupMembersHandler(IGroupDbRepository groupDbRepository, IMapper mapper)
        {
            _groupDbRepository = groupDbRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse> Handle(GroupMembersQuery query, CancellationToken ct)
        {
            var members = await _groupDbRepository.GroupMembersAsync(query.GroupId, query.RecordStatus, ct);

            return new ApiResponse(members);
        }
    }
}