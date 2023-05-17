import { mount, createLocalVue } from '@vue/test-utils';
import VueRouter from 'vue-router';
import Breadcrumbs from '@/Fwamework/Breadcrumbs/Components/BreadcrumbsComponent.vue';
const routes = [
    {
        name: 'path1',
        path: '/path1',
        meta: {
            breadcrumb: {
                title: "Node1"
            }
        }
    },
    {
        name: 'path2',
        path: '/path2',
        meta: {
            breadcrumb: {
                title: "Node2",
                parentName: 'path1'
            }
        }
    },
    {
        name: 'path3',
        path: '/path3',
        meta: {
            breadcrumb: {
                title: "Node3",
                parentName: 'path2'
            }
        }
    },
    {
        name: 'pathMissingMeta',
        path: '/pathMissingMeta'
    },
    {
        name: 'pathMissingParent',
        path: '/pathMissingParent',
        meta: {
            breadcrumb: {
                title: 'pathMissingParent',
                parentName: 'parentDoesNotExist'
            }
        }
    }
];
const $i18n = { t: (textKey) => textKey };
const localVue = createLocalVue();
localVue.use(VueRouter);

describe("Breadcrumbs", () => {
    const router = new VueRouter({
        routes
    });
    const wrapper = mount(Breadcrumbs, {
        localVue,
        router,
        mocks: {
            $i18n
        }
    });

    it("Testing parent node resolution", async () => {

        await router.push("/path3");
        await wrapper.vm.resolveBreadcrumb(wrapper.vm.$route);

        expect(wrapper.vm.breadcrumbs[0].text).toBe('Node1');
        expect(wrapper.vm.breadcrumbs[1].text).toBe('Node2');
        expect(wrapper.vm.breadcrumbs[2].text).toBe('Node3');

        expect(wrapper.findAll('.breadcrumb-node').length).toBe(3);
    });

    it("Testing 'missing meta.breadcrumb'", async () => {
       
        await router.push("/pathMissingMeta");
        await wrapper.vm.resolveBreadcrumb(wrapper.vm.$route);

        expect(wrapper.vm.breadcrumbs[0].text).toBe('Missing "meta.breadcrumb"');
    });

    it("Testing 'node not found'", async () => {

        await router.push("/pathMissingParent");
        await wrapper.vm.resolveBreadcrumb(wrapper.vm.$route);

        expect(wrapper.vm.breadcrumbs[0].text).toEqual(expect.stringContaining('Not found node'));
    });
});