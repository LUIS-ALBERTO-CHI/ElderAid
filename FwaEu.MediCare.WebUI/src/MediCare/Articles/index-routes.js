const ArticlesSearchPageComponent = () => import('@/MediCare/Articles/Components/ArticlesSearchPageComponent.vue');

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
]
