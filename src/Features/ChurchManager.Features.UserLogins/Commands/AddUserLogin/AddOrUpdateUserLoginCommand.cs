using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using DotLiquid.Util;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Application.Features.UserLogins.Commands.AddUserLogin
{
    public record AddOrUpdateUserLoginCommand : IRequest<ApiResponse>
    {
        public int PersonId { get; set; }
        public string Tenant { get; set; }
        public List<string> Roles { get; set; } = new(0);
    }

    public class AddUserLoginHandler : IRequestHandler<AddOrUpdateUserLoginCommand, ApiResponse>
    {
        private readonly IGenericDbRepository<UserLogin> _dbRepository;
        private readonly IPersonDbRepository _personDbRepository;

        public AddUserLoginHandler(
            IGenericDbRepository<UserLogin> dbRepository,
            IPersonDbRepository personDbRepository)
        {
            _dbRepository = dbRepository;
            _personDbRepository = personDbRepository;
        }

        public async Task<ApiResponse> Handle(AddOrUpdateUserLoginCommand command, CancellationToken ct)
        {
            var userLogin =  await _dbRepository
                .Queryable()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PersonId == command.PersonId, ct);

            if (userLogin is not null)
            {
                userLogin.Tenant = command.Tenant;
                userLogin.Roles = command.Roles;
            }
            else
            {
                var person = await _personDbRepository.GetByIdAsync(command.PersonId, ct) ?? throw new ArgumentNullException(nameof(command.PersonId));
                userLogin = new UserLogin
                {
                    PersonId = command.PersonId,
                    Tenant = command.Tenant,
                    Roles = command.Roles,
                    Username = person.Email.IsTruthy() && person.Email.IsActive.IsTruthy() ? person.Email.Address : $"{person.FullName.FirstName}.{person.FullName.LastName}",
                    Password = BCrypt.Net.BCrypt.HashPassword("pancake"),
                };

                await _dbRepository.AddAsync(userLogin, ct);
            }

            return new ApiResponse();
        }
    }
}