﻿using AutoMapper;
using IsoTreatmentProcessSupportAPI.Entities;
using IsoTreatmentProcessSupportAPI.Models;

namespace IsoTreatmentProcessSupportAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateReminderDto, Reminder>();

            CreateMap<CreateEntryDto, Entry>();

            CreateMap<Reminder, ReminderDto>();

            CreateMap<Entry, EntryDto>();
        }
    }
}
