using AutoMapper;
using IsoTreatmentProcessSupportAPI.Entities;
using IsoTreatmentProcessSupportAPI.Exceptions;
using IsoTreatmentProcessSupportAPI.Models;

namespace IsoTreatmentProcessSupportAPI.Services
{
    public interface IEntryService
    {
        IEnumerable<EntryDto> GetAll(string token);
        EntryDto Add(string token, CreateEntryDto dto);
        EntryDto Update(int id, UpdateEntryDto dto);
        void Delete(int id);
    }
    public class EntryService : IEntryService
    {
        private readonly IsoSupportDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        public EntryService(IsoSupportDbContext dbContext, IMapper mapper, ITokenService tokenService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _tokenService = tokenService;
        }
        public EntryDto Add(string token, CreateEntryDto dto)
        {
            int userId = _tokenService.GetUserIdFromToken(token);

            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            var entryEntity = _mapper.Map<Entry>(dto);

            entryEntity.UserId = userId;

            _dbContext.Entries.Add(entryEntity);
            _dbContext.SaveChanges();

            var addedEntry = _mapper.Map<EntryDto>(entryEntity);

            return addedEntry;
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

        public IEnumerable<EntryDto> GetAll(string token)
        {
            int userId = _tokenService.GetUserIdFromToken(token);

            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            var entries = _dbContext.Entries
                .Where(e => e.UserId == userId) 
                .AsEnumerable();

            if (entries.Count() == 0)
            {
                throw new NotFoundException("Entries not found");
            }

            var entryDtos = _mapper.Map<IEnumerable<EntryDto>>(entries);

            return entryDtos;
        }

        public EntryDto Update(int id, UpdateEntryDto dto)
        {
            var entry = _dbContext.Entries.FirstOrDefault(e => e.Id == id);

            if (entry is null)
            {
                throw new NotFoundException("Entry not found");
            }

            entry.Content = dto.Content;

            var updatedEntry = _mapper.Map<EntryDto>(entry);

            _dbContext.SaveChanges();

            return updatedEntry;
        }
    }
}
