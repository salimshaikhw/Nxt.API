using AutoMapper;
using Nxt.Entities.Dtos.Customer;
using Nxt.Entities.Models;
using System;
using System.Linq.Expressions;

namespace Nxt.Services.AutoMapperProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CustomerInput, Customer>();
            CreateMap<Customer, CustomerDetails>();
        }
    }

    public static class MapperExtensions
    {
        public static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination>(this IMappingExpression<TSource, TDestination> map, Expression<Func<TDestination, object>> selector)
        {
            map.ForMember(selector, config => config.Ignore());
            return map;
        }

        public static IMappingExpression<TSource, TDest> IgnoreAllUnmapped<TSource, TDest>(this IMappingExpression<TSource, TDest> expression)
        {
            expression.ForAllMembers(opt => opt.Ignore());
            return expression;
        }
    }
}
