const ArticlesSearchPageComponent = () => import('@/MediCare/Articles/Components/ArticlesSearchPageComponent.vue');
const PatientOrderArticlePageComponent = () => import('@/MediCare/Patients/Components/PatientOrderArticlePageComponent.vue');

export default [
    {
        path: '/Patient/:id/SearchArticle/',
        name: 'SearchArticle',
        component: ArticlesSearchPageComponent,
        meta: {
            title: 'Commander un autre produit',
            breadcrumb: {
                titleKey: 'Commander un autre produit',
                parentName: 'Patient'
            },
        }
    },
    {
        path: '/Orders/SearchPatient/Patient/:id/SearchArticle/',
        name: 'SearchArticleFromOrder',
        component: ArticlesSearchPageComponent,
        meta: {
            title: 'Commander un autre produit',
            breadcrumb: {
                titleKey: 'Commander un autre produit',
                parentName: 'SearchPatientFromOrder'
            },
        }
    },
    {
        path: '/Orders/SearchArticle/',
        name: 'SearchArticleForEMSFromOrder',
        component: ArticlesSearchPageComponent,
        meta: {
            title: 'Commander un autre produit',
            breadcrumb: {
                titleKey: 'Commander un autre produit',
                parentName: 'Orders'
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
				parentName: 'SearchPatientFromOrder'
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
]
