using AutoMapper;

namespace MotiNet.AutoMapper
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
                                     var newMember = $"Ordered{member}";
                                     var sourceValue = source.GetType().GetProperty(newMember).GetValue(source);
                                     var destinationValue = context.Mapper.Map(sourceValue, source.GetType().GetProperty(newMember).PropertyType, destination.GetType().GetProperty(member).PropertyType);
                                     destination.GetType().GetProperty(member).SetValue(destination, destinationValue);
                                 });
            }

            return mappingExpression;
        }
    }
}
