using AutoMapper;
using IsoTreatmentProcessSupportAPI.Entities;
using IsoTreatmentProcessSupportAPI.Exceptions;
using IsoTreatmentProcessSupportAPI.Models;

namespace IsoTreatmentProcessSupportAPI.Services
{
    public interface IEntryService
    {
        IEnumerable<EntryDto> GetByDate(int userId, GetEntryByDateDto dto);
        void Add(int userId, CreateEntryDto dto);
        void Update(int id, UpdateEntryDto dto);
        void Delete(int id);
    }
    public class EntryService : IEntryService
    {
        private readonly IsoSupportDbContext _dbContext;
        private readonly IMapper _mapper;
        public EntryService(IsoSupportDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public void Add(int userId, CreateEntryDto dto)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            var entryEntity = _mapper.Map<Entry>(dto);

            entryEntity.UserId = userId;

            _dbContext.Entries.Add(entryEntity);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var entry = _dbContext.Entries.FirstOrDefault(e => e.Id == id);

            if (entry is null)
            {
                throw new NotFoundException("Entry not found");
            }

            _dbContext.Entries.Remove(entry);
            _dbContext.SaveChanges();
        }

        public IEnumerable<EntryDto> GetByDate(int userId, GetEntryByDateDto dto)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            var entries = _dbContext.Entries
                .Where(e => e.Date.Date == dto.Date.Date)
                .AsEnumerable();

            if (entries.Count() == 0)
            {
                throw new NotFoundException("Entry(s) not found for the given date");
            }

            var entryDtos = _mapper.Map<IEnumerable<EntryDto>>(entries);

            return entryDtos;
        }

        public void Update(int id, UpdateEntryDto dto)
        {
            var entry = _dbContext.Entries.FirstOrDefault(e => e.Id == id);

            if (entry is null)
            {
                throw new NotFoundException("Entry not found");
            }

            entry.Content = dto.Content;

            _dbContext.SaveChanges();
        }
    }
}
