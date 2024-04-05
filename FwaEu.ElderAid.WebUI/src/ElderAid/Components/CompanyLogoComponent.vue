<template>
	<img :src="companyLogo" :height="height" :width="width" />
</template>

<script>
	import CompanyLogo from '@/ElderAid/Content/elder-aid-logo.png';

	export default {
		props: {
			width: {
				type: Number,
				default: null
			},
			height: {
				type: Number,
				default: 30
			}
		},
		data: function () {
			return {
				companyLogo: CompanyLogo
			}
		}
	}
</script>
