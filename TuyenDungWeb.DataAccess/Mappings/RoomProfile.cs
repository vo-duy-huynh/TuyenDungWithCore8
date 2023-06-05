using AutoMapper;
using TuyenDungWeb.Models;
using TuyenDungWeb.Models.ViewModels;

namespace TuyenDungWeb.DataAccess.Mappings
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<Room, RoomViewModel>()
                .ForMember(dst => dst.Admin, opt => opt.MapFrom(x => x.Admin.UserName));

            CreateMap<RoomViewModel, Room>();
        }
    }
}
