using AutoMapper;
using ShamsAlShamoos01.Shared.Entities;
using ShamsAlShamoos01.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShamsAlShamoos01.Application.Mapping
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            //Mapper.Configuration.AllowNullCollections = true;
            AllowNullCollections = true;
            //AllowNullCollections = false;

            CreateMap<HistoryRegisterKala01, HistoryRegisterKala01ViewModelcat>().ReverseMap();
            CreateMap<HistoryRegisterKala01, HistoryRegisterKala01ViewModel_Update>().ReverseMap();


        }
    }

}
