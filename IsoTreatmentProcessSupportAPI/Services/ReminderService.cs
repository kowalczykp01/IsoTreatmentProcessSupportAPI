using AutoMapper;
using IsoTreatmentProcessSupportAPI.Entities;
using IsoTreatmentProcessSupportAPI.Exceptions;
using IsoTreatmentProcessSupportAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IsoTreatmentProcessSupportAPI.Services
{
    public interface IReminderService
    {
        IEnumerable<ReminderDto> GetAll(int userId);
        ReminderDto GetById(int id);
        void Add(int userId, CreateReminderDto dto);
        void Delete(int id);
        void Update(int id, ReminderDto dto);
    }
    public class ReminderService : IReminderService
    {
        private readonly IsoSupportDbContext _dbContext;
        private readonly IMapper _mapper;
        public ReminderService(IsoSupportDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public void Add(int userId, CreateReminderDto dto)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            var reminderEntity = _mapper.Map<Reminder>(dto);

            reminderEntity.UserId = userId;

            _dbContext.Reminders.Add(reminderEntity);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var reminder = _dbContext.Reminders.FirstOrDefault(r => r.Id == id);

            if (reminder is null)
            {
                throw new NotFoundException("Reminder not found");
            }

            _dbContext.Reminders.Remove(reminder);
            _dbContext.SaveChanges();
        }

        public IEnumerable<ReminderDto> GetAll(int userId)
        {
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

        public ReminderDto GetById(int id)
        {
            var reminder = _dbContext.Reminders.FirstOrDefault(r => r.Id == id);

            if (reminder is null)
            {
                throw new NotFoundException("Reminder not found");
            }

            var reminderDto = _mapper.Map<ReminderDto>(reminder);

            return reminderDto;
        }

        public void Update(int id, ReminderDto dto)
        {
            var reminder = _dbContext.Reminders.First(r => r.Id == id);

            if (reminder is null)
            {
                throw new NotFoundException("ReminderNotFound");
            }

            reminder.Time = dto.Time;

            _dbContext.SaveChanges();
        }
    }
}
