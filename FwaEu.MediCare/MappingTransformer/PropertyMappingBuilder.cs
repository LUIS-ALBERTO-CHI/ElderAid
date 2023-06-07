using System.Reflection;

namespace FwaEu.MediCare.MappingTransformer
{
	public class PropertyMappingBuilder
	{
		public PropertyInfo Property { get; private set; }
		public string ColumnName { get; private set; }
		public PropertyMappingBuilder(PropertyInfo propertyInfo)
		{
			this.Property = propertyInfo;
		}

		public PropertyMappingBuilder Column(string columnName)
		{
			this.ColumnName = columnName;
			return this;
		}
	}
}