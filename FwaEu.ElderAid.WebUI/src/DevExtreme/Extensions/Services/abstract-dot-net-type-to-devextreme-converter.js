class AbstractDotNetTypeToDevExtremeConverter
{
	constructor()
	{
		if (this.constructor === AbstractDotNetTypeToDevExtremeConverter)
		{
			throw new TypeError('Abstract class "AbstractDotNetTypeToDevExtremeConverter" cannot be instantiated directly');
		}
		this.dotNetTypesConverters = null;
	}

	getDefaultValue(options)
	{
		return null;
	}

	getDotNetTypesHandled()
	{
		return this.dotNetTypesConverters.reduce(function (agg, item)
		{
			return agg.concat(item.getDotNetTypesHandled());
		}, []);
	}

	createDataGridColumn(options, getResource)
	{
		throw new Error('You must implement the function createDataGridColumn!');
	}

	createPivotGridColumn()
	{
		throw new Error('You must implement the function createPivotGridColumn!');
	}
}

export default AbstractDotNetTypeToDevExtremeConverter;