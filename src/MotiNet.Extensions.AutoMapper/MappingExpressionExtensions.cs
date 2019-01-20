using AutoMapper;

namespace MotiNet.Extensions.AutoMapper
{
    public static class MappingExpressionExtensions
    {
        public static IMappingExpression SwapMemberWithOrderedMember(
            this IMappingExpression mappingExpression,
            params string[] members)
        {
            foreach (var member in members)
            {
                mappingExpression.ForMember(member, options => options.Ignore())
                                 .AfterMap((source, destination) =>
                                 {
                                     SwapMember(source, destination, member, $"Ordered{member}", null);
                                 });
            }

            return mappingExpression;
        }

        public static IMappingExpression<TSource, TDestination> SwapMemberWithOrderedMember<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> mappingExpression,
            params string[] members)
        {
            foreach (var member in members)
            {
                mappingExpression.ForMember(member, options => options.Ignore())
                                 .AfterMap((source, destination, context) =>
                                 {
                                     SwapMember(source, destination, member, $"Ordered{member}", context.Mapper);
                                 });
            }

            return mappingExpression;
        }

        private static void SwapMember(
            object source, object destination,
            string originalMember, string newMember,
            IRuntimeMapper mapper)
        {
            var sourceValue = source.GetType().GetProperty(newMember).GetValue(source);
            var destinationValue =
                mapper?.Map(sourceValue, source.GetType().GetProperty(newMember).PropertyType, destination.GetType().GetProperty(originalMember).PropertyType) ??
                Mapper.Map(sourceValue, source.GetType().GetProperty(newMember).PropertyType, destination.GetType().GetProperty(originalMember).PropertyType);
            destination.GetType().GetProperty(originalMember).SetValue(destination, destinationValue);
        }
    }
}
