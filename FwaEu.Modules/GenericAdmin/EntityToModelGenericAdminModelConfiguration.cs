using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.ComponentModel;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using NHibernate.Linq;
using NHibernate.Exceptions;
using FwaEu.Modules.Data.Database;
using FwaEu.Fwamework.Globalization;

namespace FwaEu.Modules.GenericAdmin
{
	public abstract class EntityToModelGenericAdminModelConfiguration<TEntity, TIdentifier, TModel, TSessionContext>
		: ModelAttributeGenericAdminModelConfiguration<TModel>
		where TEntity : class, IEntity, new()
		where TModel : new()
		where TSessionContext : BaseSessionContext<IStatefulSessionAdapter>
	{
		private Lazy<TSessionContext> _sessionContext;

		protected EntityToModelGenericAdminModelConfiguration(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this._sessionContext = new Lazy<TSessionContext>(
				() => serviceProvider.GetRequiredService<TSessionContext>());

			var keyResolver = serviceProvider.GetRequiredService<IEntityKeyResolver<TEntity>>();
			this._keyLazy = new Lazy<string>(keyResolver.ResolveKey);
		}

		private readonly Lazy<string> _keyLazy;
		public override string Key => this._keyLazy.Value;
		protected RepositorySession<IStatefulSessionAdapter> RepositorySession => this._sessionContext.Value.RepositorySession;

		protected virtual IRepository<TEntity, TIdentifier> GetRepository()
		{
			return (IRepository<TEntity, TIdentifier>)this.RepositorySession.CreateByEntityType(typeof(TEntity));
		}

		protected abstract Expression<Func<TEntity, TModel>> GetSelectExpression();

		protected virtual IQueryable<TEntity> Query()
		{
			return this.GetRepository().QueryNoPerimeter(); //NOTE: Perimeter discussion https://dev.azure.com/fwaeu/TemplateCore/_workitems/edit/4508
		}

		private async Task<TModel[]> LoadEntitiesAsync()
		{
			return (await this.Query()
				.Select(this.GetSelectExpression())
				.ToListAsync())
				.ToArray();
		}

		public override async Task<LoadDataResult<TModel>> GetModelsAsync()
		{
			return new LoadDataResult<TModel>(
				new ArrayDataSource<TModel>(await this.LoadEntitiesAsync())
				);
		}

		protected abstract void SetIdToModel(TModel model, TEntity entity);
		protected abstract Task<TEntity> GetEntityAsync(TModel model);
		protected abstract Task FillEntityAsync(TEntity entity, TModel model);

		private async Task<TEntity> GetEntityOrFailWhenNotFoundAsync(TModel model)
		{
			var entity = await this.GetEntityAsync(model);

			if (entity == null)
			{
				throw new NotSupportedException("Entity not found.");
			}

			return entity;
		}

		private async Task<TEntity> GetEntityOrNewOrFailWhenNotFoundAsync(TModel model)
		{
			return this.IsNew(model) ? new TEntity()
				: await this.GetEntityOrFailWhenNotFoundAsync(model);
		}

		protected override async Task<SimpleSaveModelResult> SaveModelAsync(TModel model)
		{
			var entity = await this.GetEntityOrNewOrFailWhenNotFoundAsync(model);

			await this.FillEntityAsync(entity, model);

			await this.GetRepository().SaveOrUpdateAsync(entity);
			this.SetIdToModel(model, entity);
			return new SimpleSaveModelResult();
		}

		public override async Task<SaveResult> SaveAsync(IEnumerable<TModel> models)
		{
			var session = this.RepositorySession.Session;
			using (var transaction = session.BeginTransaction())
			{
				try
				{
					var saveResult = default(SaveResult);
					try
					{
						saveResult = await base.SaveAsync(models);

						await session.FlushAsync();
						await transaction.CommitAsync();
					}
					catch (GenericADOException e)
					{
						DatabaseExceptionHelper.CheckForDbConstraints(e);
						throw;
					}
					return saveResult;
				}
				catch
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
		}

		protected override async Task<SimpleDeleteModelResult> DeleteModelAsync(TModel model)
		{
			var entity = await this.GetEntityOrNewOrFailWhenNotFoundAsync(model);
			await this.GetRepository().DeleteAsync(entity);
			return new SimpleDeleteModelResult();
		}

		public override async Task<DeleteResult> DeleteAsync(IEnumerable<TModel> models, PropertyDescriptor[] keyProperties)
		{
			var session = this.RepositorySession.Session;
			using (var transaction = session.BeginTransaction())
			{
				try
				{
					var deleteResult = default(DeleteResult);

					try
					{
						deleteResult = await base.DeleteAsync(models, keyProperties);

						await session.FlushAsync();
						await transaction.CommitAsync();
					}
					catch (GenericADOException e)
					{
						DatabaseExceptionHelper.CheckForDbConstraints(e);
						throw;
					}
					return deleteResult;
				}
				catch
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
		}
		protected override Property CreateProperty(PropertyDescriptor propertyDescriptor)
		{
			var localizableStringAttributte = propertyDescriptor.Attributes.OfType<LocalizableStringCustomTypeAttribute>().FirstOrDefault();
			if (localizableStringAttributte != null)
			{
				var mappedCultureCodes = RepositorySession.Session.GetMappedCultureCodes<TEntity>(propertyDescriptor.Name);
				localizableStringAttributte.SupportedCultureCodes = mappedCultureCodes;
			}
			return base.CreateProperty(propertyDescriptor);
		}



	}

}