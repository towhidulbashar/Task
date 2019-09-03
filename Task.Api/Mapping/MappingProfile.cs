using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task.Api.Controllers.Resources;
using Task.Api.Core.Domain;

namespace Task.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<WorkItem, GetWorkItemResource>()
                .ForMember(g => g.ApplicationUserName, operation => operation.MapFrom(w => $"{w.AssignedTo.FirstName} {w.AssignedTo.LastName}"));
        }
    }
}
