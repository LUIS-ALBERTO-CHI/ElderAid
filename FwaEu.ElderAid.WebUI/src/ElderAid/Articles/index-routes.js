const ArticlesSearchPageComponent = () => import('@/ElderAid/Articles/Components/ArticlesSearchPageComponent.vue');
const PatientOrderArticlePageComponent = () => import('@/ElderAid/Patients/Components/PatientOrderArticlePageComponent.vue');

export default [
    {
        path: '/Patient/:id/SearchArticle/',
        name: 'SearchArticle',
        component: ArticlesSearchPageComponent,
        meta: {
            title: 'Commander un autre produit',
            breadcrumb: {
                titleKey: 'Pedir otro producto',
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
                titleKey: 'Rechercher un article',
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
                titleKey: 'Rechercher un article',
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
                titleKey: 'Rechercher un article',
                parentName: 'Orders'
            },
        }
    },
	{
		path: '/Patient/:id/Protection/SearchArticle/AddPosology/:articleId',
		name: 'AddPosology',
		component: PatientOrderArticlePageComponent,
		meta: {
			title: 'Ajout d\'une protection, posologie',
			breadcrumb: {
				titleKey: 'Ajout d\'une protection, posologie',
				parentName: 'SearchArticleFromProtection'
			},
		}
	},
    {
		path: '/Patient/:id/OrderArticle/:articleId',
		name: 'OrderArticle',
		component: PatientOrderArticlePageComponent,
		meta: {
			title: 'Commander un article',
			breadcrumb: {
				titleKey: 'Commander un article',
				parentName: 'SearchArticle'
			},
		}
	},
    {
		path: '/Patient/:id/OrderArticleFromOrder/:articleId',
		name: 'OrderArticleFromOrder',
		component: PatientOrderArticlePageComponent,
		meta: {
			title: 'Commander un article',
			breadcrumb: {
				titleKey: 'Commander un article',
				parentName: 'Orders'
			},
		}
	},
	{
		path: '/Patient/:id/OrderArticleFromOrder/:articleId',
		name: 'OrderArticleFromOrderWithArticleId',
		component: PatientOrderArticlePageComponent,
		meta: {
			title: 'Commander un article',
			breadcrumb: {
				titleKey: 'Commander un article',
				parentName: 'SearchPatientFromOrderWithArticleId'
			},
		}
	},
	{
		path: '/Orders/Patient/:id/SearchArticleForEms/:articleId',
		name: 'OrderArticleForEmsFromOrder',
		component: PatientOrderArticlePageComponent,
		meta: {
			title: 'Commander un article',
			breadcrumb: {
				titleKey: 'Commander un article',
				parentName: 'SearchArticleForEMSFromOrder'
			},
		}
	},
]
