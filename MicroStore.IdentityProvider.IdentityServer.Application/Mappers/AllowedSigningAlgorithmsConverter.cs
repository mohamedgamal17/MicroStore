using AutoMapper;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Mappers
{
    class AllowedSigningAlgorithmsConverter : IValueConverter<ICollection<string>, string>
    {
        public static AllowedSigningAlgorithmsConverter Converter = new AllowedSigningAlgorithmsConverter();

        public string Convert(ICollection<string> sourceMember, ResolutionContext context)
        {
            if (sourceMember == null || !sourceMember.Any())
            {
                return null;
            }
            return sourceMember.Aggregate((x, y) => $"{x},{y}");
        }
       
    }
}
