using AutoMapper;
using SecondAPI.Domain.Model;
using SecondAPI.Domain.ViewModel;

namespace SecondAPI.Domain.Mapping;



public class Mapping : Profile
{
    public Mapping()
    {
        CreateMap<DadosUsuario, UsuarioViewModel>().ReverseMap();
    }
}