using System;
using System.Security.Principal;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Domain.Dtos;
using Api.Domain.Entities;
using Api.Domain.Interfaces.Services.User;
using Api.Domain.Repository;
using Api.Domain.Security;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Api.Service.Services
{
    public class LoginService : ILoginService
    {
        private IUserRepository _repository;
        private SigningConfigurations _signConfigurations;
        private TokenConfigurations _tokenConfiguration;
        private IConfiguration _configuration;

        public LoginService(IUserRepository repository,
                            SigningConfigurations signConfigurations,
                            TokenConfigurations tokenConfiguration,
                            IConfiguration configuration)
        {
            _repository = repository;
            _signConfigurations = signConfigurations;
            _tokenConfiguration = tokenConfiguration;
            _configuration = configuration;
        }

        public async Task<object> FindByLogin(LoginDto loginDto)
        {
            var baseUser = new UserEntity();

            if (loginDto != null && !string.IsNullOrEmpty(loginDto.Email))
            {
                baseUser = await _repository.FindByLogin(loginDto.Email);
                if (baseUser == null)
                {
                    return new
                    {
                        authenticated = false,
                        message = "falha ao autenticar."
                    };
                }
                else
                {
                    var identity = new ClaimsIdentity(
                        new GenericIdentity(baseUser.Email),
                        new[]
                        {
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                new Claim(JwtRegisteredClaimNames.UniqueName, baseUser.Email)
                        }
                    );

                    var createDate = DateTime.Now;
                    var expirationDate = createDate + TimeSpan.FromSeconds(_tokenConfiguration.Seconds);
                    var handler = new JwtSecurityTokenHandler();

                    string token = CreateToken(handler, identity, createDate, expirationDate);
                    return SuccessObject(createDate, expirationDate, token, loginDto);
                }
            }
            else
            {
                return new
                {
                    authenticated = false,
                    message = "falha ao autenticar."
                };
            }
        }

        private string CreateToken(JwtSecurityTokenHandler handler, ClaimsIdentity identity, DateTime createDate, DateTime expirationDate)
        {
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfiguration.Issuer,
                Audience = _tokenConfiguration.Audience,
                SigningCredentials = _signConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = createDate,
                Expires = expirationDate
            });

            var token = handler.WriteToken(securityToken);
            return token;
        }

        private object SuccessObject(DateTime createDate, DateTime expirationDate, string token, LoginDto loginDto)
        {
            return new
            {
                authenticate = true,
                created = createDate.ToString("yyyy-MM-dd HH:mm:ss"),
                expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                acessToken = token,
                username = loginDto.Email,
                message = "Usuário logado com sucesso."
            };
        }
    }
}
