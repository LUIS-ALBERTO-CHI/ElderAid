import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import PatientsMasterDataService from './Services/patients-master-data-service';
import TreatmentsMasterDataService from './Services/treatments-master-data-service';
import ProtectionMasterDataService from './Services/protections-master-data-service';
import IncontinenceLevelMasterDataService from './Services/incontinence-level-master-data-service';

export class PatientsModule extends AbstractModule {

	async onInitAsync() {
		await PatientsMasterDataService.configureAsync();
		await TreatmentsMasterDataService.configureAsync();
		await ProtectionMasterDataService.configureAsync();
		await IncontinenceLevelMasterDataService.configureAsync();
	}
}