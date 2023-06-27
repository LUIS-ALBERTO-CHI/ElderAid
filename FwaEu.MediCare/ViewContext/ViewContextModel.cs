using System.Collections.Generic;

namespace FwaEu.MediCare.ViewContext
{
	public class ViewContextModel
	{
		public string DatabaseName { get; }

		public ViewContextModel(string databaseName)
		{
            this.DatabaseName = databaseName;

        }

		/// <summary>
		/// Used by logs and also to convert view context to reporting parameters.
		/// </summary>
		public Dictionary<string, object> ToDictionary()
		{
			var result = new Dictionary<string, object>();
			if (this.DatabaseName != null)
			{
				result = new Dictionary<string, object>()
				{
                    { "databaseName", this.DatabaseName }
                };
			}
			return result;
		}
	}
}