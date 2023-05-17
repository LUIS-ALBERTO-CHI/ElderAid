using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FwaEu.Modules.GenericAdmin.WebApi
{
	public class SaveResultsModel
	{
		public SaveResultModel[] Results { get; set; }

		public static SaveResultsModel FromSaveResults(SaveResult saveResult)
		{
			return new SaveResultsModel()
			{
				Results = saveResult.Results.Select(r => new SaveResultModel()
				{
					WasNew = r.WasNew,
					Keys = Helper.CreateDictionary(r.Keys),
				})
				.ToArray(),
			};
		}
	}

	public class SaveResultModel : ActionOnModelResultModel
	{
		public bool WasNew { get; set; }
	}
}