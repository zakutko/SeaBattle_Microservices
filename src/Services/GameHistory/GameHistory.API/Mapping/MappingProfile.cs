using AutoMapper;
using SeaBattle.Contracts.Dtos;

namespace GameHistory.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DAL.Models.GameHistory, GameHistoryResponse>();
        }
    }
}
