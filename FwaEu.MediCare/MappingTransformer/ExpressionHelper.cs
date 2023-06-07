using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FwaEu.MediCare.MappingTransformer
{
	public static class ExpressionHelper
	{
		public static string GetPropertyNameFromAction<TIn, TOut>(Expression<Func<TIn, TOut>> exp)
		{
			return GetPropertyInfoFromAction(exp).Name;
		}
		public static PropertyInfo GetPropertyInfoFromAction<TIn, TOut>(Expression<Func<TIn, TOut>> exp)
		{
			var body = exp.Body as MemberExpression;
			if (body == null)
			{
				var ubody = (UnaryExpression)exp.Body;
				body = ubody.Operand as MemberExpression;
			}
			return (PropertyInfo)body.Member;
		}
		public static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> exp)
		{
			return GetPropertyNameFromAction(exp);
		}
		public static string GetPropertyName<T>(Expression<Func<T, object>> exp)
		{
			return GetPropertyNameFromAction(exp);
		}
		public static PropertyInfo GetPropertyInfo<T>(Expression<Func<T, object>> exp)
		{
			return GetPropertyInfoFromAction(exp);
		}

		public static MemberInfo GetMemberInfoFromAction<TIn, TOut>(Expression<Func<TIn, TOut>> expression)
		{
			var body = expression.Body as MemberExpression;

			if (body == null)
			{
				var ubody = (UnaryExpression)expression.Body;
				body = ubody.Operand as MemberExpression;
			}
			return body?.Member;
		}
	}
}
