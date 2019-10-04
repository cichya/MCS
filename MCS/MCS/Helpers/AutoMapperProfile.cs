using AutoMapper;
using MCS.DTO;
using MCS.Models;
using MCS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Helpers
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			IDateTimeService dateTimeService = new DateTimeService();

			this.CreateMap<Person, PersonForListDto>()
				.ForMember(member => member.Age, memberOpt =>
				{
					memberOpt.MapFrom(value => dateTimeService.CalculateAge(value.BirthDate));
				});
		}
	}

	public class AutoMapperConfiguration
	{
		public MapperConfiguration Configure()
		{
			var config = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<AutoMapperProfile>();
			});

			return config;
		}
	}
}
