using FluentValidation;
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
        private readonly IValidator<User> _userValidator;

        public UserService(IUserRepository userRepository, IValidator<User> userValidator)
        {
            _userRepository = userRepository;
            _userValidator = userValidator;

        }
        public async Task<bool> CreateAsync(User user, CancellationToken token = default)
        {
            await _userValidator.ValidateAndThrowAsync(user, cancellationToken: token);
            return await _userRepository.CreateAsync(user, token);
        }

        public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            return _userRepository.DeleteByIdAsync(id, token);
        }

        public Task<IEnumerable<User>> GetAllAsync(CancellationToken token = default)
        {
            return _userRepository.GetAllAsync( token);
        }

        public Task<User?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return _userRepository.GetByIdAsync(id, token);
        }

        public async Task<User?> UpdateAsync(User user, CancellationToken token = default)
        {
            await _userValidator.ValidateAndThrowAsync(user, cancellationToken: token);
            var userExists = await _userRepository.ExistsByIdAsync(user.Id, token);
            if (!userExists) { return null; }

            await _userRepository.UpdateAsync(user, token);
            return user;
        }
    }
}
