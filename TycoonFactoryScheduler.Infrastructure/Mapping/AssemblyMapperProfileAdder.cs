using AutoMapper;
using System.Reflection;
using TycoonFactoryScheduler.Infrastructure.Exceptions;

namespace TycoonFactoryScheduler.Infrastructure.Mapping
{
    public static class AssemblyMapperProfileAdder
    {
        public static void AddProfilesFromAssemblyOfType<T>(this IMapperConfigurationExpression mapperConfigurationExpression) =>
           mapperConfigurationExpression.AddProfilesFromAssemblyOfType(typeof(T));

        public static void AddProfilesFromAssemblyOfType(this IMapperConfigurationExpression mapperConfigurationExpression, Type typeInsideTargetAssembly) =>
            mapperConfigurationExpression.AddProfilesFromAssembly(typeInsideTargetAssembly.Assembly);

        public static void AddProfilesFromAssembly(this IMapperConfigurationExpression mapperConfigurationExpression, string assemblyName)
        {
            Assembly? assembly = AppDomain.CurrentDomain.GetAssemblies().
                    SingleOrDefault(assembly => assembly.GetName().Name == assemblyName)
                    ?? throw new AssemblyNotFoundException(assemblyName);

            mapperConfigurationExpression.AddProfilesFromAssembly(assembly);
        }

        public static void AddProfilesFromAssembly(this IMapperConfigurationExpression mapperConfigurationExpression, Assembly assembly)
        {
            assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(Profile)))
                .ToList()
                .ForEach(type => AddProfile(type, mapperConfigurationExpression));
        }

        private static void AddProfile(Type type, IMapperConfigurationExpression mapperConfigurationExpression)
        {
            Profile? instance = type.GetConstructor(Array.Empty<Type>())?.Invoke(Array.Empty<object>()) as Profile
                ?? throw new AssemblyMapperProfileAdderException(type);

            mapperConfigurationExpression.AddProfile(instance);
        }
    }
}
