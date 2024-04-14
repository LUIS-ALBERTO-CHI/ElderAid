const ArticlesSearchPageComponent = () => import('@/ElderAid/Articles/Components/ArticlesSearchPageComponent.vue');
const PatientOrderArticlePageComponent = () => import('@/ElderAid/Patients/Components/PatientOrderArticlePageComponent.vue');

export default [
    {
        path: '/Patient/:id/SearchArticle/',
        name: 'SearchArticle',
        component: ArticlesSearchPageComponent,
        meta: {
            title: 'Pedir otro producto',
            breadcrumb: {
                titleKey: 'Buscar un ar´ticulo',
                parentName: 'Patient'
            },
        }
    },
	{
        path: '/Patient/:id/Protection/SearchArticle/',
        name: 'SearchArticleFromProtection',
        component: ArticlesSearchPageComponent,
        meta: {
            title: 'Buscar un articulo',
            breadcrumb: {
                titleKey: 'Buscar un artículo',
                parentName: 'Protection'
            },
        }
    },
	{
        path: '/Orders/SearchPatient/Patient/:id/SearchArticle/',
        name: 'SearchArticleFromOrder',
        component: ArticlesSearchPageComponent,
        meta: {
            title: 'Buscar un articulo',
            breadcrumb: {
                titleKey: 'Buscar un artículo',
                parentName: 'SearchPatientFromOrder'
            },
        }
    },
	{
        path: '/Orders/Patient/:id/SearchArticleForEms/',
        name: 'SearchArticleForEMSFromOrder',
        component: ArticlesSearchPageComponent,
        meta: {
            title: 'Buscar un articulo',
            breadcrumb: {
                titleKey: 'Buscar un artículo',
                parentName: 'Orders'
            },
        }
    },
	{
		path: '/Patient/:id/Protection/SearchArticle/AddPosology/:articleId',
		name: 'AddPosology',
		component: PatientOrderArticlePageComponent,
		meta: {
			title: 'Agregar protección, dosis.',
			breadcrumb: {
				titleKey: 'Agregar protección, dosis.',
				parentName: 'SearchArticleFromProtection'
			},
		}
	},
    {
		path: '/Patient/:id/OrderArticle/:articleId',
		name: 'OrderArticle',
		component: PatientOrderArticlePageComponent,
		meta: {
			title: 'Ordenar un artículo',
			breadcrumb: {
				titleKey: 'Ordenar un artículo',
				parentName: 'SearchArticle'
			},
		}
	},
    {
		path: '/Patient/:id/OrderArticleFromOrder/:articleId',
		name: 'OrderArticleFromOrder',
		component: PatientOrderArticlePageComponent,
		meta: {
			title: 'Ordenar un artículo',
			breadcrumb: {
				titleKey: 'Ordenar un artículo',
				parentName: 'Orders'
			},
		}
	},
	{
		path: '/Patient/:id/OrderArticleFromOrder/:articleId',
		name: 'OrderArticleFromOrderWithArticleId',
		component: PatientOrderArticlePageComponent,
		meta: {
			title: 'Ordenar un artículo',
			breadcrumb: {
				titleKey: 'Ordenar un artículo',
				parentName: 'SearchPatientFromOrderWithArticleId'
			},
		}
	},
	{
		path: '/Orders/Patient/:id/SearchArticleForEms/:articleId',
		name: 'OrderArticleForEmsFromOrder',
		component: PatientOrderArticlePageComponent,
		meta: {
			title: 'Ordenar un artículo',
			breadcrumb: {
				titleKey: 'Ordenar un artículo',
				parentName: 'SearchArticleForEMSFromOrder'
			},
		}
	},
]
