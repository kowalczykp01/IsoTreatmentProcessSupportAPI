﻿using AutoMapper;
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
        ReminderDto Add(int userId, CreateAndUpdateReminderDto dto);
        void Delete(int id);
        ReminderDto Update(int id, CreateAndUpdateReminderDto dto);
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
        public ReminderDto Add(int userId, CreateAndUpdateReminderDto dto)
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

            var addedReminder = _mapper.Map<ReminderDto>(reminderEntity);
            return addedReminder;
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

        public ReminderDto Update(int id, CreateAndUpdateReminderDto dto)
        {
            var reminder = _dbContext.Reminders.First(r => r.Id == id);

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
