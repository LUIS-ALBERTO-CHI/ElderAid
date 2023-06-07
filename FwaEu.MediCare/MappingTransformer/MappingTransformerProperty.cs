using NHibernate.Transform;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FwaEu.MediCare.MappingTransformer
{
	public class MappingTransformerPropertyColumn<TModelMapping> : IResultTransformer
	where TModelMapping : IMappingTransformer, new()
	{
		private TModelMapping modelMapping = new();

		public IList TransformList(IList collection)
		{
			return collection;
		}

		public object TransformTuple(object[] tuple, string[] aliases)
		{
			var model = Activator.CreateInstance(typeof(TModelMapping).BaseType.GetGenericArguments()[0]);
			for (int i = 0; i < aliases.Length; i++)
			{
				var aliasValue = aliases[i];
				var tupleValue = tuple[i];
				var propertyMappingInfo = modelMapping.PropertiesMapping.FirstOrDefault(x => x.ColumnName == aliasValue);
				if (propertyMappingInfo != null)
					propertyMappingInfo.Property.SetValue(model, tupleValue);
			}
			return model;
		}
	}
}