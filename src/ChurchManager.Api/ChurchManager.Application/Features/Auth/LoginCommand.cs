using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.ViewModels;
using ChurchManager.Infrastructure.Abstractions.Security;
using ChurchManager.Infrastructure.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Application.Features.Auth
{
    public record LoginCommand([Required] string Username, [Required] string Password) : IRequest<TokenViewModel>
    {
    }

    public class LoginHandler : IRequestHandler<LoginCommand, TokenViewModel>
    {
        private readonly ChurchManagerDbContext _dbContext;
        private readonly ITokenService _tokens;

        public LoginHandler(ChurchManagerDbContext dbContext, ITokenService tokens)
        {
            _dbContext = dbContext;
            _tokens = tokens;
        }

        public async Task<TokenViewModel> Handle(LoginCommand request, CancellationToken ct)
        {
            var user =  await _dbContext.UserLogin
                .FirstOrDefaultAsync(u => 
                    u.Username == request.Username &&
                    u.Password == request.Password, 
                    ct);

            if(user is not null)
            {
                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, request.Username),
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new("PersonId", user.PersonId.ToString()),
                    new(ClaimTypes.Role, "Manager")
                };

                var accessToken = _tokens.GenerateAccessToken(claims);
                var refreshToken = _tokens.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

                await _dbContext.SaveChangesAsync(ct);

                return new TokenViewModel(IsAuthenticated:true, accessToken, refreshToken);
            }

            return new TokenViewModel(IsAuthenticated: false);
        }
    }
}
