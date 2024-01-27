using AutoMapper;
using IsoTreatmentProcessSupportAPI.Entities;
using IsoTreatmentProcessSupportAPI.Exceptions;
using IsoTreatmentProcessSupportAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IsoTreatmentProcessSupportAPI.Services
{
    public interface IReminderService
    {
        IEnumerable<ReminderDto> GetAll(string token);
        ReminderDto GetById(string token, int id);
        ReminderDto Add(string token, CreateAndUpdateReminderDto dto);
        void Delete(string token, int id);
        ReminderDto Update(string token, int id, CreateAndUpdateReminderDto dto);
    }
    public class ReminderService : IReminderService
    {
        private readonly IsoSupportDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        public ReminderService(IsoSupportDbContext dbContext, IMapper mapper, ITokenService tokenService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _tokenService = tokenService;
        }
        public ReminderDto Add(string token, CreateAndUpdateReminderDto dto)
        {
            int userId = _tokenService.GetUserIdFromToken(token);

            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            var reminderEntity = _mapper.Map<Reminder>(dto);

            reminderEntity.UserId = userId;

            _dbContext.Reminders.Add(reminderEntity);
            _dbContext.SaveChanges();

            var addedReminder = _mapper.Map<ReminderDto>(reminderEntity);
            return addedReminder;
        }

        public void Delete(string token, int id)
        {
            int userId = _tokenService.GetUserIdFromToken(token);

            var user = _dbContext.Users
                .Include(u => u.Reminders)
                .FirstOrDefault(u => u.Id == userId);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            var reminder = user.Reminders.FirstOrDefault(r => r.Id == id);


            if (reminder is null)
            {
                throw new NotFoundException("Reminder not found");
            }

            _dbContext.Reminders.Remove(reminder);
            _dbContext.SaveChanges();
        }

        public IEnumerable<ReminderDto> GetAll(string token)
        {
            int userId = _tokenService.GetUserIdFromToken(token);

            var user = _dbContext.Users
                .Include(u => u.Reminders)
                .FirstOrDefault(u => u.Id == userId);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            var reminderDtos = _mapper.Map<IEnumerable<ReminderDto>>(user.Reminders);

            return reminderDtos;
        }

        public ReminderDto GetById(string token, int id)
        {
            int userId = _tokenService.GetUserIdFromToken(token);

            var user = _dbContext.Users
                .Include(u => u.Reminders)
                .FirstOrDefault(u => u.Id == userId);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            var reminder = user.Reminders.FirstOrDefault(r => r.Id == id);

            if (reminder is null)
            {
                throw new NotFoundException("Reminder not found");
            }

            var reminderDto = _mapper.Map<ReminderDto>(reminder);

            return reminderDto;
        }

        public ReminderDto Update(string token, int id, CreateAndUpdateReminderDto dto)
        {
            int userId = _tokenService.GetUserIdFromToken(token);

            var user = _dbContext.Users
                .Include(u => u.Reminders)
                .FirstOrDefault(u => u.Id == userId);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            var reminder = user.Reminders.FirstOrDefault(r => r.Id == id);

            if (reminder is null)
            {
                throw new NotFoundException("Reminder not found");
            }

            reminder.Time = dto.Time;

            _dbContext.SaveChanges();

            var updatedReminder = _mapper.Map<ReminderDto>(reminder);
            return updatedReminder;
        }
    }
}
