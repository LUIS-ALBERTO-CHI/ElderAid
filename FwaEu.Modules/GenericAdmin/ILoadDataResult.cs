using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FwaEu.Modules.GenericAdmin
{
	public interface IDataSource
	{
		IEnumerable<object> Items { get; }
		bool CouldHaveMoreItems { get; }
	}

	public interface IDataSource<out TModel> : IDataSource
	{
		new IEnumerable<TModel> Items { get; }
	}

	public class LoadDataResult
	{
		public LoadDataResult(IDataSource value)
		{
			this.Value = value;
		}

		public IDataSource Value { get; private set; } //NOTE: Ready for later implementation of paginated load
	}

	public class LoadDataResult<TModel> : LoadDataResult
	{
		public LoadDataResult(IDataSource<TModel> value)
			: base(value)
		{
		}

		public new IDataSource<TModel> Value
		{
			get { return (IDataSource<TModel>)base.Value; }
		}
	}

	public class ArrayDataSource : IDataSource
	{
		public bool CouldHaveMoreItems => false;

		public ArrayDataSource(object[] items)
		{
			this.Items = items;
		}

		public object[] Items { get; }

		IEnumerable<object> IDataSource.Items => this.Items.Cast<object>();
	}

	public class ArrayDataSource<TModel> : ArrayDataSource, IDataSource<TModel>
	{
		public ArrayDataSource(TModel[] items) : base(items.Cast<object>().ToArray())
		{
		}

		public new TModel[] Items
		{
			get { return base.Items.Cast<TModel>().ToArray(); }
		}

		IEnumerable<TModel> IDataSource<TModel>.Items => this.Items;
	}

	public class EnumValue
	{
		public EnumValue(object value)
		{
			this.Value = value;
		}

		public object Value { get; }
	}

	public interface IEnumDataSource : IDataSource
	{
		new IEnumerable<EnumValue> Items { get; }
	}

	public class EnumDataSource<TEnum> : IDataSource<EnumValue>, IEnumDataSource
		where TEnum : struct
	{
		static EnumDataSource()
		{
			if (!typeof(TEnum).IsEnum)
			{
				throw new NotSupportedException($"The '{nameof(TEnum)}' generic argument must be an enum.");
			}
		}

		public bool CouldHaveMoreItems => false;

		public virtual IEnumerable<EnumValue> Items => Enum.GetValues(typeof(TEnum))
			.Cast<TEnum>().Select(value => new EnumValue(value));

		IEnumerable<object> IDataSource.Items => this.Items.Cast<object>();
	}
}