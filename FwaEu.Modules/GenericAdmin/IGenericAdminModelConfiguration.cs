using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericAdmin
{
	public interface IGenericAdminModelConfiguration
	{
		string Key { get; }
		Type ModelType { get; }

		IEnumerable<Property> GetProperties();
		Task<LoadDataResult> GetModelsAsync();
		Task<SaveResult> SaveAsync(IEnumerable<object> models);
		Task<DeleteResult> DeleteAsync(IEnumerable<IDictionary<string, object>> modelsKeys);
		IAuthorizedActions GetAuthorizedActions();
		Task<bool> IsAccessibleAsync();
	}

	public class ActionOnModelResult
	{
		public ActionOnModelResult(IDictionary<string, object> keys)
		{
			this.Keys = keys;
		}

		public IDictionary<string, object> Keys { get; }
	}

	public class SaveModelResult : ActionOnModelResult
	{
		public SaveModelResult(IDictionary<string, object> keys, bool wasNew) : base(keys)
		{
			this.WasNew = wasNew;
		}

		public bool WasNew { get; }
	}

	public class SaveResult
	{
		public SaveResult(SaveModelResult[] results)
		{
			this.Results = results;
		}

		public SaveModelResult[] Results { get; }
	}

	public class DeleteResult
	{
		public DeleteResult(DeleteModelResult[] results)
		{
			this.Results = results;
		}

		public DeleteModelResult[] Results { get; }
	}

	public class DeleteModelResult : ActionOnModelResult
	{
		public DeleteModelResult(IDictionary<string, object> keys) : base(keys)
		{
		}
	}
}
