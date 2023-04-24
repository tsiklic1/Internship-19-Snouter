using Snouter.Application.Models;
using Snouter.Application.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;


        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;

        }
        public Task<bool> CreateAsync(User user)
        {
            return _userRepository.CreateAsync(user);
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            return _userRepository.DeleteByIdAsync(id);
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            return _userRepository.GetAllAsync();
        }

        public Task<User?> GetByIdAsync(Guid id)
        {
            return _userRepository.GetByIdAsync(id);
        }

        public async Task<User?> UpdateAsync(User user)
        {
            var userExists = await _userRepository.ExistsByIdAsync(user.Id);
            if (!userExists) { return null; }

            await _userRepository.UpdateAsync(user);
            return user;
        }
    }
}
