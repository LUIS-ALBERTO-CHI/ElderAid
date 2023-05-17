import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import ReportDisplayService from "@/Modules/ReportDisplay/Services/report-display-service";
import ReportGridDisplayType from "@/Modules/ReportDisplay/Components/report-grid-display-type";
import ReportPivotDisplayType from "@/Modules/ReportDisplay/Components/report-pivot-display-type";

export class ReportDisplayModule extends AbstractModule {
	onInitAsync() {
		ReportDisplayService.addReportDisplay(ReportGridDisplayType);
		ReportDisplayService.addReportDisplay(ReportPivotDisplayType);
	}
}
