using NHibernate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.MediCare.MappingTransformer
{
	public interface IMappingTransformer
	{
        Task<List<TTarget>> GetGenericModelAsync<TSource, TTarget>(ISession session, string query, Dictionary<string, object> parameters = null);
    }
}
