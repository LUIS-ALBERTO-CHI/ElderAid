using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Reflection
{
	public static class TypeHelper
	{
		private static Type[] NumericalWithDecimalPart = new[]
		{
			typeof(Decimal),
			typeof(Double),
			typeof(Single),
		};

		public static bool IsNumericalWithDecimalPart(this Type type)
		{
			return NumericalWithDecimalPart.Contains(type);
		}
	}
}
