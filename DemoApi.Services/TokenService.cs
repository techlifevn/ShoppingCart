using DemoApi.Data.EF;
using System.IdentityModel.Tokens.Jwt;

namespace DemoApi.Services
{
    public interface ITokenSerivce
    {
        Task<bool> ValidToken(string token);
    }

    public class TokenService : ITokenSerivce
    {
        private readonly DataContext _context;

        public TokenService(DataContext context)
        {
            _context = context;
        }

        public Task<bool> ValidToken(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();

            var jwt = jwtHandler.ReadJwtToken(token);

            return Task.FromResult(true);
        }
    }
}
