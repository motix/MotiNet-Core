using AutoMapper;

namespace MotiNet.AutoMapper
{
    public static class MappingExpressionExtensions
    {
        public static IMappingExpression SwapMemberWithOrderedMember(this IMappingExpression mappingExpression, params string[] members)
        {
            foreach (var member in members)
            {
                mappingExpression.ForMember(member, options => options.MapFrom($"Ordered{member}"));
            }

            return mappingExpression;
        }
    }
}
