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
]
