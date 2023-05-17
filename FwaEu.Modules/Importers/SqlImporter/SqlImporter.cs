using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Nhibernate;
using FwaEu.Fwamework.Imports;
using FwaEu.Fwamework.ProcessResults;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FwaEu.Modules.Importers.SqlImporter
{
	public class SqlImporter
	{
		public SqlImporter(INhibernateSessionAdapterFactory nhibernateSessionAdapterFactory)
		{
			this._nhibernateSessionAdapterFactory = nhibernateSessionAdapterFactory
				?? throw new ArgumentNullException(nameof(nhibernateSessionAdapterFactory));
		}

		private readonly INhibernateSessionAdapterFactory _nhibernateSessionAdapterFactory;

		public async Task ImportAsync(ImportContext context, IImportFile[] orderedFiles)
		{
			using (var session = this._nhibernateSessionAdapterFactory.CreateStatelessSession())
			{
				foreach (var file in orderedFiles)
				{
					await this.ImportSqlFileAsync(context, file, session);
				}
			}
		}

		private static async Task<string> ReadSqlAsync(IImportFile file)
		{
			using (var stream = file.OpenReadStream())
			{
				using (var reader = new StreamReader(stream))
				{
					return await reader.ReadToEndAsync();
				}
			}
		}

		private async Task ImportSqlFileAsync(ImportContext context, IImportFile file, INhibernateStatelessSessionAdapter session)
		{
			var resultContext = context.ProcessResult.CreateContext($"Import of file {file.Name}", "SqlImport");
			var content = await ReadSqlAsync(file);

			//HACK: Pierre was sorry about this :(
			var queries = Regex.Split(content, @"[\r\n\s]GO[\r\n\s]", RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

			foreach (var query in queries)
			{
				resultContext.Add(new SqlProcessResultEntry(query));
				try
				{ 
					await session.NhibernateSession.CreateSQLQuery(query).ExecuteUpdateAsync();
				}
				catch(Exception ex)
				{
					resultContext.Add(ErrorProcessResultEntry.FromException(ex));
					continue;
				}
			}
		}
	}
}
