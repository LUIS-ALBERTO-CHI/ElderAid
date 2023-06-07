using System.Collections.Generic;

namespace FwaEu.MediCare.MappingTransformer
{
	public interface IMappingTransformer
	{
		List<PropertyMappingBuilder> PropertiesMapping { get; }
	}
}
