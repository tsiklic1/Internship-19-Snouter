using Snouter.Application.Models;
using Snouter.Application.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Application.Services
{
    public class SpecService : ISpecService
    {
        private readonly ISpecRepository _specRepository;

        private readonly ICategoryRepository _categoryRepository;

        public SpecService(ISpecRepository specRepository, ICategoryRepository categoryRepository)
        {
            _specRepository = specRepository;
            _categoryRepository = categoryRepository;

        }
        public Task<bool> CreateAsync(Spec spec)
        {
            return _specRepository.CreateAsync(spec);
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Spec>> GetAllAsync()
        {
            return _specRepository.GetAllAsync();
        }

        public Task<Spec?> GetByIdAsync(Guid id)
        {
            return _specRepository.GetByIdAsync(id);
        }
    }
}
