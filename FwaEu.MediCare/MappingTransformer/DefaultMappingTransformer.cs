using NHibernate;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FwaEu.MediCare.MappingTransformer
{

    public class DefaultMappingTransformer : IMappingTransformer
    {
        public async Task<List<TTarget>> GetGenericModelAsync<TSource, TTarget>(ISession session, string query, Dictionary<string, object> parameters = null)
        {
            var storedProcedure = session.CreateSQLQuery(query);

            parameters = parameters ?? new Dictionary<string, object>();
            
            foreach (var item in parameters)
            {
                storedProcedure.SetParameter(item.Key, item.Value);
            }

            var models = await storedProcedure
                .SetResultTransformer(Transformers.AliasToBean<TSource>())
                .ListAsync<TSource>();

            return models.Select(x => MapProperties<TSource, TTarget>(x)).ToList();
        }


        private static TTarget MapProperties<TSource, TTarget>(TSource source)
        {
            var destination = Activator.CreateInstance<TTarget>();

            var sourceProperties = typeof(TSource).GetProperties();
            foreach (var sourceProperty in sourceProperties)
            {
                var mapToAttribute = sourceProperty.GetCustomAttribute<MapToAttribute>();
                if (mapToAttribute != null)
                {
                    var destinationPropertyName = mapToAttribute.SourcePropertyName;
                    var destinationProperty = typeof(TTarget).GetProperty(destinationPropertyName);
                    if (destinationProperty != null)
                    {
                        var sourceValue = sourceProperty.GetValue(source);
                        destinationProperty.SetValue(destination, sourceValue);
                    }
                }
            }

            return destination;
        }
    }
}
