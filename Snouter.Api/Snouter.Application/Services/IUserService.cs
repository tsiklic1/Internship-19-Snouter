﻿using Snouter.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Application.Services
{
    public interface IUserService
    {
        Task<bool> CreateAsync(User user);
        Task<User?> GetByIdAsync(Guid id);
        Task<IEnumerable<User>> GetAllAsync();

        //Task<Category?> UpdateAsync(Category category);
        Task<User?> DeleteAsync(Guid id);
    }
}
