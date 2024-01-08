using AutoMapper;
using Smart_Library.Entities;
using Smart_Library.Models;

namespace Smart_Library.Config.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Book, BookViewModel>()
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name))
            .ForMember(dest => dest.AuthorAddress, opt => opt.MapFrom(src => src.Author.Address))
            .ForMember(dest => dest.AuthorImageURL, opt => opt.MapFrom(src => src.Author.ImageURL))
            .ForMember(dest => dest.PublisherName, opt => opt.MapFrom(src => src.Publisher.Name))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.AddedByName, opt => opt.MapFrom(src => src.AddedBy.FirstName + " " + src.AddedBy.LastName));
        }
    }
}