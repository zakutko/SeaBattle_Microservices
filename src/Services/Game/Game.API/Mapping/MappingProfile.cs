using AutoMapper;
using Game.DAL.Models;
using SeaBattle.Contracts.Dtos;

namespace Game.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Cell, CellListResponse>();
            CreateMap<Cell, CellListResponseForSecondPlayer>();
        }
    }
}