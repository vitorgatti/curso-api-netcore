using System.Threading.Tasks;
using Api.Domain.Dtos;
using Api.Domain.Interfaces.Services.User;
using Api.Domain.Repository;

namespace Api.Service.Services
{
    public class LoginService : ILoginService
    {
        private IUserRepository _repository;

        public LoginService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<object> FindByLogin(LoginDTO loginDto)
        {
            if (loginDto != null && !string.IsNullOrEmpty(loginDto.Email))
            {
                return await _repository.FindByLogin(loginDto.Email);
            }
            else
            {
                return null;
            }
        }
    }
}
