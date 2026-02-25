using AutoMapper;
using ShamsAlShamoos03.Shared.Entities;
using ShamsAlShamoos03.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShamsAlShamoos03.Application.Mapping
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            AllowNullCollections = true;
            CreateMap<HistoryRegisterKala01, HistoryRegisterKala01ViewModelcat>().ReverseMap();
            CreateMap<HistoryRegisterKala01, HistoryRegisterKala01ViewModel_Update>().ReverseMap();


        }
    }

}
