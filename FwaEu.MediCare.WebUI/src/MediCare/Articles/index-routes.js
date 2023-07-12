const ArticlesSearchPageComponent = () => import('@/MediCare/Articles/Components/ArticlesSearchPageComponent.vue');

export default [
    {
        path: '/SearchArticle',
        name: 'SearchArticle',
        component: ArticlesSearchPageComponent,
        meta: {
            breadcrumb: {
                titleKey: 'Commander un autre produit',
                parentName: 'Patient'
            },
        }
    },
]