using AutoMapper;

namespace Eventos.IO.Application.AutoMapper
{
    // Para o Auto Mapper funcionar corretamente e necessario instalar ele desta forma:
    // install-package automapper
    // Sempre lembrar de instalar na camada certa que no caso e
    // 3 - Application\Eventos.IO.Application
    public class AutoMapperConfiguration
    {
        public static MapperConfiguration RegisterMappings()
        {
            // ps = profiles
            return new MapperConfiguration(ps =>
            {
                ps.AddProfile(new DomainToViewModelMappingProfile());
                ps.AddProfile(new ViewModelToDomainMappingProfile());
            });
        }
    }
}