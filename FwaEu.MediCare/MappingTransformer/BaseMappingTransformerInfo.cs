using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FwaEu.MediCare.MappingTransformer
{
	public class BaseMappingTransformerInfo<TModel> : IMappingTransformer where TModel : class
	{
		public List<PropertyMappingBuilder> PropertiesMapping { get; set; } = new();

		protected PropertyMappingBuilder Map(Expression<Func<TModel, object>> exp)
		{
			var propertyMappingBuilder = new PropertyMappingBuilder(ExpressionHelper.GetPropertyInfo(exp));
			PropertiesMapping.Add(propertyMappingBuilder);
			return propertyMappingBuilder;
		}
	}
}