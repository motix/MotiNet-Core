using AutoMapper;

namespace MotiNet.Extensions.AutoMapper
{
    public static class MappingExpressionExtensions
    {
        public static IMappingExpression<TSource, TDestination> SwapMemberWithOrderedMember<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> mappingExpression,
            params string[] members)
        {
            foreach (var member in members)
            {
                mappingExpression.ForMember(member, options => options.Ignore())
                                 .AfterMap((source, destination, context) =>
                                 {
                                     SwapMember(source, destination, member, $"Ordered{member}", context);
                                 });
            }

            return mappingExpression;
        }

        private static void SwapMember(
            object source, object destination,
            string originalMember, string newMember,
            ResolutionContext context)
        {
            var sourceValue = source.GetType().GetProperty(newMember).GetValue(source);
            var destinationValue = context.Mapper.Map(sourceValue, source.GetType().GetProperty(newMember).PropertyType, destination.GetType().GetProperty(originalMember).PropertyType);
            destination.GetType().GetProperty(originalMember).SetValue(destination, destinationValue);
        }
    }
}
