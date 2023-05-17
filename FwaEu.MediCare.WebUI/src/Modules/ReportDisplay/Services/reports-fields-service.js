import DotNetTypesToDevExtremeConverterService from '@/Fwamework/DevExtreme/Services/dot-net-types-to-devextreme-converter-service.js';
import ReportDataSourceService from "@/Modules/Reports/Services/report-data-source-service";
import StringHelperService from '@/Modules/GenericAdmin/Services/string-helper-service';
import ReportFieldMasterDataService from "@/Modules/ReportMasterData/Services/report-field-master-data-service";

export default {
	async getPropertiesAsync(report) {
		let properties = await Promise.all(report.properties
			.map(async p => {
				let fieldName = StringHelperService.lowerFirstCharacter(p.name);//NOTE: We suppose that the name will be in PascalCasing
				let reportField = p.fieldInvariantId ? (await ReportFieldMasterDataService.getAsync(p.fieldInvariantId))
					:
					{
						name: fieldName,
						dotNetTypeName: 'String'
					};
				reportField.fieldName = fieldName;
				return reportField;
			}));
		return properties;
	},
	createFieldsGrid(property, viewContext) {
		let column = { allowHiding: true };
		const field = { name: property.fieldName, label: property.name, cellTemplate: property.templateName, urlFormat: property.urlFormat, fieldName: property.fieldName };
		let dataSource = { type: property.dataSourceType, argument: property.dataSourceArgument };

		if (dataSource.type) {
			column.dataField = field.name;
			column.caption = field.label;
			if (!column.lookup) {
				column.lookup = {};
			}
			column.lookup.dataSource = { load: async () => await ReportDataSourceService.getFieldDataSourceAsync(property.invariantId, dataSource, {}, viewContext) };
			// TODO:Récupérer les displayExpr et valueExpr selon le editorOptions.dataSource ?  https://fwaeu.visualstudio.com/MediCare/_workitems/edit/7352				
			column.lookup.displayExpr = 'name';
			column.lookup.valueExpr = 'id';
		}
		else {
			let dotNetTypeToDevExtremeConverter = DotNetTypesToDevExtremeConverterService.getConverter(property.dotNetTypeName);
			column = Object.assign(column, dotNetTypeToDevExtremeConverter.createDataGridColumn(field));
		}

		return column;
	},

	createFieldsPivot(property, viewContext) {
		let column = [];
		const field = { name: property.fieldName, label: property.name, summaryType: property.summaryType?.toLowerCase(), format: property.displayFormat };
		let dotNetTypeToDevExtremeConverter = DotNetTypesToDevExtremeConverterService.getConverter(property.dotNetTypeName);
		column = dotNetTypeToDevExtremeConverter.createPivotGridColumn(field);
		return column;
	}
}
