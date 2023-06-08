using FwaEu.Fwamework.Data.Database.Sessions;
using MySqlX.XDevAPI;
using NHibernate;
using NHibernate.Transform;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ubiety.Dns.Core.Common;

namespace FwaEu.MediCare.MappingTransformer
{
    public interface IDatabaseMappingTransformer
    {
        Task<List<TModelTo>> GetGenericModelAsync<TModelFrom, TModelTo>(ISession session, string query, Dictionary<string, object> parameters)
        where TModelFrom : IMappingTransformer, new();

        Task<int> GetGenericModelCountAsync(ISession session, string query, Dictionary<string, object> parameters);
    }

    public class DatabaseMappingTransformer : IDatabaseMappingTransformer
    {
        public async Task<List<TModelTo>> GetGenericModelAsync<TModelFrom, TModelTo>(ISession session, string query, Dictionary<string, object> parameters) where TModelFrom : IMappingTransformer, new()
        {
            var stockedProcedure = session.CreateSQLQuery(query);
            foreach (var item in parameters)
            {
                stockedProcedure.SetParameter(item.Key, item.Value);
            }
            var model = await stockedProcedure.SetResultTransformer(new MappingTransformerPropertyColumn<TModelFrom>()).ListAsync<TModelTo>();
            return model.ToList();
        }

        public Task<int> GetGenericModelCountAsync(ISession session, string query, Dictionary<string, object> parameters)
        {
            var stockedProcedure = session.CreateSQLQuery(query);
            foreach (var item in parameters)
            {
                stockedProcedure.SetParameter(item.Key, item.Value);
            }
            var model = await stockedProcedure.resul SetResultTransformer(Transformers.AliasToBean<OperationModel>()).ListAsync<OperationModel>();
            return 
        }
    }
}
