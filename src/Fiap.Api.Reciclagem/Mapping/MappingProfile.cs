using AutoMapper;
using Fiap.Api.Reciclagem.Models;
using Fiap.Api.Reciclagem.ViewModels;

namespace Fiap.Api.Reciclagem.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PontoColeta, PontoColetaViewModel>();
            CreateMap<PontoColetaCreateViewModel, PontoColeta>();

            CreateMap<Descarte, DescarteViewModel>();
            CreateMap<DescarteCreateViewModel, Descarte>();

            CreateMap<Alerta, AlertaViewModel>();
        }
    }
}
