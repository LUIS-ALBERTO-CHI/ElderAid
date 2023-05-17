const fallBackLocale = 'en';

class ResourcesManager
{
	constructor(locale)
	{
		this.locale = locale || fallBackLocale;
	}

	async loadDefaultResourcesAsync()
	{
		this.resourcesManagers = await Promise.all([
			import(`@/Modules/GenericAdmin/Content/generic-admin-common.${this.locale}.json`)
		]);
	}

	pushTopSpecificResources(specificResources)
	{
		this.resourcesManagers = specificResources.concat(this.resourcesManagers);
	}

	getResource(keys)
	{
		for (let resources of this.resourcesManagers)
		{
			if (!resources)
			{
				continue;
			}
			for (let key of keys)
			{
				if (resources[key])
				{
					return resources[key];
				}
			}
		}
		return `{Missing ressource ${keys.reduce((agg, key) => agg + (agg.length ? " or " + key : key), "")}}`;
	}
}

export default {
	getNewResourcesManager(userCulture)
	{
		return new ResourcesManager(userCulture);
	}
};