using System.Reflection;

namespace MicroStore.TestBase.Extensions
{
    public static class TypeExtensions
    {
        public static bool HasSetter(this PropertyInfo property)
        {
            return property.DeclaringType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                                         .Any(m => m.Name == "set_" + property.Name);
        }

        public static bool HasGetter(this PropertyInfo property)
        {
            return property.DeclaringType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                                         .Any(m => m.Name == "get_" + property.Name);
        }
    }
}
