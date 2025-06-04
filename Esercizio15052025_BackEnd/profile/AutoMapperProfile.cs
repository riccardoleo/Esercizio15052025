using AutoMapper;
using Esercizio15052025.DTO.PlantComponent_DTO;
using Esercizio15052025.DTO.Tool_DTO;
using Esercizio15052025.DTO.ToolCategory_DTO;
using Esercizio15052025.Models;
using Esercizio20052025.DTO.Users_DTO;

namespace Esercizio15052025.profile
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Mapper PlantComponent - IGNORA le proprietà di navigazione
            CreateMap<PlantComponent, PC_DTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => src.CreatedByUserId));

            CreateMap<PC_DTO, PlantComponent>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => src.CreatedByUserId))
                .ForMember(dest => dest.ComponentId, opt => opt.Ignore())
                .ForMember(dest => dest.Tools, opt => opt.Ignore());

            CreateMap<PC_DTO_Update, PlantComponent>()
                .ForMember(dest => dest.ComponentId, opt => opt.MapFrom(src => src.ComponentId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => src.CreatedByUserId))
                .ForMember(dest => dest.Tools, opt => opt.Ignore());

            CreateMap<PC_DTO_Delete, PlantComponent>()
                .ForMember(dest => dest.ComponentId, opt => opt.MapFrom(src => src.componentId))
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Description, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.Tools, opt => opt.Ignore());

            // Mapper Tool - IGNORA le proprietà di navigazione
            CreateMap<Tool, T_DTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate))
                .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => src.CreatedByUserId))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.PlantComponentId, opt => opt.MapFrom(src => src.PlantComponentId));

            CreateMap<T_DTO, Tool>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate))
                .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => src.CreatedByUserId))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.PlantComponentId, opt => opt.MapFrom(src => src.PlantComponentId))
                .ForMember(dest => dest.ToolId, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.PlantComponent, opt => opt.Ignore());

            CreateMap<T_DTO_Update, Tool>()
                .ForMember(dest => dest.ToolId, opt => opt.MapFrom(src => src.ToolId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate))
                .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => src.CreatedByUserId))
                .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
                .ForMember(dest => dest.PlantComponentId, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.PlantComponent, opt => opt.Ignore());

            CreateMap<T_DTO_Delete, Tool>()
                .ForMember(dest => dest.ToolId, opt => opt.MapFrom(src => src.ToolId))
                .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Description, opt => opt.Ignore())
                .ForMember(dest => dest.PlantComponentId, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.PlantComponent, opt => opt.Ignore());

            // Mapper ToolCategory - IGNORA le proprietà di navigazione
            CreateMap<ToolCategory, TC_DTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => src.CreatedByUserId));

            CreateMap<TC_DTO, ToolCategory>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => src.CreatedByUserId))
                .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
                .ForMember(dest => dest.Tools, opt => opt.Ignore());

            CreateMap<TC_DTO_Update, ToolCategory>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => src.CreatedByUserId))
                .ForMember(dest => dest.Tools, opt => opt.Ignore());

            CreateMap<TC_DTO_Delete, ToolCategory>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.Tools, opt => opt.Ignore());

            // Mapper User - IGNORA le proprietà di navigazione e collezioni
            CreateMap<User, User_DTO>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.passwdRole, opt => opt.Ignore());

            CreateMap<User_DTO, User>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.PlantComponents, opt => opt.Ignore())
                .ForMember(dest => dest.ToolCategories, opt => opt.Ignore())
                .ForMember(dest => dest.Tools, opt => opt.Ignore());

            CreateMap<UserCredential_DTO, User>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash))
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.PlantComponents, opt => opt.Ignore())
                .ForMember(dest => dest.ToolCategories, opt => opt.Ignore())
                .ForMember(dest => dest.Tools, opt => opt.Ignore());
        }
    }
}