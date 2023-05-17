using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using NHibernate.Linq;
using FwaEu.Fwamework.Data;
using System.Threading;
using FwaEu.Modules.Reports.WebApi;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.ReportsProvidersByEntities;
using FluentNHibernate.Data;
using FwaEu.Modules.ReportsUserViewsByEntities;
using FwaEu.Fwamework.Globalization;
using System.Collections;
using Microsoft.Extensions.DependencyInjection;

namespace FwaEu.Modules.Reports
{
	public interface IReportUserViewService
	{
		Task<IEnumerable<ReportUserViewModel>> GetAllAsync();
		Task DeleteAsync(int id);
		Task SaveAsync(ReportUserViewModel model);
	}

	public class DefaultReportUserViewService : IReportUserViewService
	{
		private readonly ISessionAdapterFactory _sessionAdapterFactory;
		private readonly IRepositoryFactory _repositoryFactory;


		public DefaultReportUserViewService(ISessionAdapterFactory sessionAdapterFactory,
			IRepositoryFactory repositoryFactory)
		{
			this._sessionAdapterFactory = sessionAdapterFactory
				?? throw new ArgumentNullException(nameof(sessionAdapterFactory));
			this._repositoryFactory = repositoryFactory
				?? throw new ArgumentNullException(nameof(repositoryFactory));
		}

		public async Task<IEnumerable<ReportUserViewModel>> GetAllAsync()
		{
			using (var session = this._sessionAdapterFactory.CreateStatefulSession())
			{
				var reportViewsModels = await this._repositoryFactory
					.Create<ReportUserViewEntityRepository>(session)
					.Query().Select(x => new ReportUserViewModel()
					{
						Id = x.Id,
						Name = x.Name,
						UserId = x.User.Id,
						ReportId = x.Report.Id
					}).ToListAsync();

				return reportViewsModels;
			}
		}

		public async Task DeleteAsync(int id)
		{
			using (var session = this._sessionAdapterFactory.CreateStatefulSession())
			{
				using (var transaction = session.BeginTransaction())
				{
					var repository = this._repositoryFactory
						.Create<ReportUserViewEntityRepository>(session);

					var entity = await repository.GetAsync(id);

					if (entity == null)
					{
						throw new NotFoundException($"View not found with id #{id}.");
					}

					await repository.DeleteAsync(entity);
					await session.FlushAsync();
					await transaction.CommitAsync();
				}
			}
		}

		public async Task SaveAsync(ReportUserViewModel model)
		{
			using (var session = this._sessionAdapterFactory.CreateStatefulSession())
			{
				var repository = this._repositoryFactory
					.Create<ReportUserViewEntityRepository>(session);

				var entity = model.Id == null ? 
					new ReportUserViewEntity() : await repository.GetAsync(model.Id.Value);

				if (entity == null)
				{
					throw new NotFoundException($"View not found with id #{model.Id}.");
				}

				entity.User = await this._repositoryFactory.Create<IUserEntityRepository>(session)
					.GetAsync(model.UserId);

				entity.Report = await this._repositoryFactory.Create<ReportEntityRepository>(session)
					.GetAsync(model.ReportId);

				entity.Name = model.Name;
				entity.Value = model.Value;

				await repository.SaveOrUpdateAsync(entity);
				await session.FlushAsync();

			}
		}
	}
}
