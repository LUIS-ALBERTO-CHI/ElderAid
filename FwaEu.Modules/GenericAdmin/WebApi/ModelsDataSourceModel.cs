using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FwaEu.Modules.GenericAdmin.WebApi
{
	public class ModelsDataSourceModel
	{
		public DataSourceModel DataSource { get; set; }
	}

	public abstract class DataSourceModel
	{
		protected DataSourceModel(object[] items)
		{
			this.Items = items;
		}

		public object[] Items { get; }
	}

	public interface IDataSourceModelFactory
	{
		/// <summary>
		/// Returns null if result type is not handled.
		/// </summary>
		DataSourceModel Create(IDataSource dataSource);
	}

	public class ArrayDataSourceModel : DataSourceModel
	{
		public ArrayDataSourceModel(object[] items) : base(items)
		{
		}
	}

	public class ArrayDataSourceModelFactory : IDataSourceModelFactory
	{
		public DataSourceModel Create(IDataSource dataSource)
		{
			if (dataSource is ArrayDataSource arrayDataSource)
			{
				return new ArrayDataSourceModel(arrayDataSource.Items.ToArray());
			}

			return null;
		}
	}

	public class EnumDataSourceModelFactory : IDataSourceModelFactory
	{
		public DataSourceModel Create(IDataSource dataSource)
		{
			if (dataSource is IEnumDataSource enumDataSource)
			{
				return new ArrayDataSourceModel(enumDataSource.Items.ToArray());
			}

			return null;
		}
	}
}