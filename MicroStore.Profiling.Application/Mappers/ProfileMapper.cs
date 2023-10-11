using MicroStore.Profiling.Application.Domain;
using MicroStore.Profiling.Application.Dtos;
using MicroStore.Profiling.Application.Models;

namespace MicroStore.Profiling.Application.Mappers
{
    public class ProfileMapper : AutoMapper.Profile
    {
        public ProfileMapper()
        {
            CreateMap<Profile, ProfileDto>()
                .ForMember(x => x.Phone, opt => opt.MapFrom(src => src.Phone.Number))
                .ForMember(x => x.Addresses, opt => opt.MapFrom(src => src.Addresses));

            CreateMap<ProfileModel, Profile>()
                .ForMember(x => x.Phone, opt =>
                {
                    opt.MapFrom(src => src.Phone);
                    opt.ConvertUsing(PhoneConverter.Converter);
                })
                .ForMember(x=> x.Gender, opt=> opt.MapFrom(src=> Enum.Parse<Gender>(src.Gender.ToLower(),true)))
                .ForMember(x => x.Addresses, opt => opt.MapFrom(src => src.Addresses));


            CreateMap<CreateProfileModel, Profile>()
                .ForMember(x => x.Phone, opt =>
                {
                    opt.MapFrom(src => src.Phone);
                    opt.ConvertUsing(PhoneConverter.Converter);
                });

            CreateMap<ProfileModel, CreateProfileModel>();
        }
    }

    public class PhoneConverter : AutoMapper.IValueConverter<string, Phone>
    {
        public static PhoneConverter Converter = new PhoneConverter();
        public Phone Convert(string sourceMember, AutoMapper.ResolutionContext context)
        {
            return Phone.Create(sourceMember);

        }
    }
}
