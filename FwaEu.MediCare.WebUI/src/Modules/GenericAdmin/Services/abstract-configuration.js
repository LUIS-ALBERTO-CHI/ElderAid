import HttpService from '@/Fwamework/Core/Services/http-service';
import ColumnsCustomizerService from '@/Modules/GenericAdmin/Services/columns-customizer-service';
import ResourcesService from '@/Modules/GenericAdmin/Services/resources-service';
import StringHelperService from '@/Modules/GenericAdmin/Services/string-helper-service';
import DataGridHelperService from '@/Modules/GenericAdmin/Services/data-grid-helper-service';
import DotNetTypesToDevExtremeConverterService from '@/Fwamework/DevExtreme/Services/dot-net-types-to-devextreme-converter-service.js';
import LocalizationService from '@/Fwamework/Culture/Services/localization-service';
import merge from 'lodash/merge';

class AbstractConfiguration
{
	constructor()
	{
		if (this.constructor === AbstractConfiguration)
			throw new TypeError('Abstract class "AbstractConfiguration" cannot be instantiated directly');
		this.configuration = null;
		this.baseModel = null;
		this.newModelWithDefaultValues = null;
		this.baseModelWithOnlyKeys = null;
		this.columnsCustomizer = ColumnsCustomizerService.getCustomizer();
		this.customize();
	}

	getResources(locale)
	{
		return [];
	}

	async getOrInitResourcesManagerAsync(locale)
	{
		if (!this.resourcesManager)
		{
			let resourcesManager = ResourcesService.getNewResourcesManager(locale);
			await resourcesManager.loadDefaultResourcesAsync();
			resourcesManager.pushTopSpecificResources(await Promise.all(this.getResources(locale)));
			this.resourcesManager = resourcesManager;
		}
		return this.resourcesManager;
	}

	initLocalizableStringsColumnsCustomization()
	{
		let addedValue = 0;
		this.columnsCustomizer.addCustomization('text',
			{ index: 100 });
		this.columnsCustomizer.addCustomization('name',
			{ index: 100 });
		this.columnsCustomizer.addCustomization('description',
			{ index: 200 });

		const defaultLanguageCode = LocalizationService.getDefaultLanguageCode();

		LocalizationService.getSupportedLanguagesCode().forEach(languageCode =>
		{
			let valueToAdd = languageCode === defaultLanguageCode ? 0 : ++addedValue;

			this.columnsCustomizer.addCustomization(`text.${languageCode}`,
				{ index: 100 + valueToAdd });
			this.columnsCustomizer.addCustomization(`name.${languageCode}`,
				{ index: 100 + valueToAdd });
			this.columnsCustomizer.addCustomization(`description.${languageCode}`,
				{ index: 200 + valueToAdd });
		});
	}

	customize()
	{
		this.columnsCustomizer.addCustomization('id', { index: 20, width: 60 });
		this.columnsCustomizer.addCustomization('code', { index: 21 });
		this.columnsCustomizer.addCustomization('invariantId', { index: 21 });
		this.columnsCustomizer.addCustomization('value', { index: 21 });

		this.initLocalizableStringsColumnsCustomization();

		this.columnsCustomizer.addCustomization('isActive', { index: 2000 });

		this.columnsCustomizer.addCustomization('updatedOn', { index: 2100 });
		this.columnsCustomizer.addCustomization('updatedById', { index: 2101, width: 200 });
		this.columnsCustomizer.addCustomization('createdOn', { index: 2200 });
		this.columnsCustomizer.addCustomization('createdById', { index: 2201, width: 200 });
	}

	getApiViewContext()//NOTE: CRUD operations customizer, check wiki for more details
	{
		return null;
	}

	onComponentCreated(component)
	{
		//TODO: Implement me https://dev.azure.com/fwaeu/TemplateWebApplication/_workitems/edit/2111
	}

	initBaseModels()
	{
		this.baseModel = {};
		this.baseModelWithOnlyKeys = {};
		this.newModelWithDefaultValues = {};

		this.configuration.properties.forEach(p =>
		{
			const propertyNameWithFirstCharacterLowered = StringHelperService.lowerFirstCharacter(p.name);
			this.baseModel[propertyNameWithFirstCharacterLowered] = null;
			if (p.isKey)
			{
				this.baseModelWithOnlyKeys[propertyNameWithFirstCharacterLowered] = null;
			}
			let dotNetTypeToDevExtremeConverter = DotNetTypesToDevExtremeConverterService.getConverter(p.type);

			this.newModelWithDefaultValues[propertyNameWithFirstCharacterLowered]
				= dotNetTypeToDevExtremeConverter ? dotNetTypeToDevExtremeConverter.getDefaultValue(p) : null;
		});
	}

	onConfigurationLoaded(component)
	{
		this.configuration = component.configuration;
		this.initBaseModels();
	}

	onInitDataSource(dataSource)
	{
		return dataSource;
	}

	createDataSource(component)
	{
		const viewContext = this.getApiViewContext();
		async function saveModel(event)
		{
			component.genericAdminConfiguration.onBeforeCreateOrUpdateRequest(component, event);
			return HttpService.post('GenericAdmin/Save/' + component.configurationKey,
				{
					models: [event.data],
					viewContext: { value: viewContext }
				});
		}

		return DataGridHelperService.createCustomStore({
			key: component.configuration.properties.filter(p => p.isKey).reduce((agg, p) =>
			{
				agg.push(StringHelperService.lowerFirstCharacter(p.name));
				return agg;
			}, []),
			load: function (loadOptions)
			{
				return component.dataSource; //HACK: Implement later for server side computing https://dev.azure.com/fwaeu/TemplateWebApplication/_workitems/edit/3253
			},
			insert: function (newData)
			{
				return saveModel({ data: newData }).then(result => //NOTE: Reassignment of id returned by server after creation, may be useless after server side load implementation
				{
					if (result.data.results[0].wasNew) //NOTE: There will always be one key returned in data-grid case, it will also be always true except on bug
					{
						Object.entries(result.data.results[0].keys) //NOTE: There will always be one key returned in data-grid case
							.forEach(([k, v]) => newData[k] = v);
					}
					component.dataSource.push(newData); //HACK: Implement later for server side computing https://dev.azure.com/fwaeu/TemplateWebApplication/_workitems/edit/3253
				});
			},
			update: function (key, data)
			{
				return saveModel({ data: data });
			},
			remove: async function (keys)
			{
				return HttpService.post('GenericAdmin/Delete/' + component.configurationKey, {
					keys: [keys],
					viewContext: { value: viewContext }
				}).then(result => //HACK: Will be removed after server side computing implementation https://dev.azure.com/fwaeu/TemplateWebApplication/_workitems/edit/3253
				{
					component.dataSource = component.dataSource.filter(i => result.data.results.find(r =>
					{
						return !Object.entries(r.keys).some(([k, v]) => i[k] === v);
					}));
					return result;
				});
			}
		});
	}

	onBeforeCreateOrUpdateRequest(component, event)
	{
		event.data = merge(this.baseModel, event.data);
	}

	async onColumnsCreatingAsync(component, columns)
	{
		await this.columnsCustomizer.customizeAsync(columns, this.configuration.properties);
	}

	onInitNewRow(component, event)
	{
		event.data = merge(this.newModelWithDefaultValues, event.data);
	}

	onRowInserting(component, event)
	{
	}

	onRowInserted(component, event)
	{
	}
	onInitialized(component, event) {

	}
	onRowUpdating(component, event)
	{
		event.newData = merge(event.oldData, event.newData);
	}

	onRowUpdated(component, event)
	{
	}

	onRowRemoving(component, event)
	{
	}

	onRowRemoved(component, event)
	{
	}

	getExportImplementation(component)
	{
		//NOTE: For custom implementation, check https://supportcenter.devexpress.com/Ticket/Details/T315513/dxdatagrid-how-to-get-filtered-data
		return null;
	}

	getExportFileName(resourcesManager)
	{
		return this.getPageTitle(resourcesManager);
	}

	getPageTitle(resourcesManager)
	{
		throw new Error('You have to implement the method getPageTitle!');
	}

	getPageContainerCustomClass()
	{
		return '';//NOTE: PageContainerComponent does not handle null or undefined value, would add "null" as class
	}

	getDescription(resourcesManager)
	{
		return null;
	}

	getGroupText(resourcesManager)
	{
		return resourcesManager.getResource(['referentials']);
	}

	getGroupIndex() {
		return 100;
	}

	getVisibleIndex() {
		return 0;
	}
}

export default AbstractConfiguration;