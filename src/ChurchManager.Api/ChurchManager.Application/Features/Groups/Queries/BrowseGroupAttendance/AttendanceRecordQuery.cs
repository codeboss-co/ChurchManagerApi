﻿using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChurchManager.Core.Shared;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Infrastructure.Persistence.Specifications;
using ChurchManager.Persistence.Models.Groups;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Application.Features.Groups.Queries.BrowseGroupAttendance
{
    public record AttendanceRecordQuery(int AttendanceRecordId) : IRequest<GroupAttendanceDetailViewModel>
    {
    }

    public class AttendanceRecordHandler : IRequestHandler<AttendanceRecordQuery, GroupAttendanceDetailViewModel>
    {
        private readonly IGroupAttendanceDbRepository _dbRepository;
        private readonly IMapper _mapper;

        public AttendanceRecordHandler(IGroupAttendanceDbRepository dbRepository, IMapper mapper)
        {
            _dbRepository = dbRepository;
            _mapper = mapper;
        }

        public async Task<GroupAttendanceDetailViewModel> Handle(AttendanceRecordQuery query, CancellationToken ct)
        {
            GroupAttendance entity = await _dbRepository
                .Queryable(new GroupAttendanceSpecification(query.AttendanceRecordId))
                .FirstOrDefaultAsync(ct);

            try
            {
                var vm = _mapper.Map<GroupAttendanceDetailViewModel>(entity);
                return vm;
            }
            catch(Exception ex)
            {

                throw;

            }
            
        }
    }
}