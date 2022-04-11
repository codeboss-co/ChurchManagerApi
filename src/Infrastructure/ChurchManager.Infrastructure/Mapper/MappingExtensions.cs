namespace ChurchManager.Infrastructure.Mapper
{
    public static class MappingExtensions
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return AutoMapperConfig.Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return AutoMapperConfig.Mapper.Map(source, destination);
        }

        public static IQueryable<TDestination> MapTo<TSource, TDestination>(this IQueryable<TSource> source)
        {
            return AutoMapperConfig.Mapper.ProjectTo<TDestination>(source);
        }
    }
}
