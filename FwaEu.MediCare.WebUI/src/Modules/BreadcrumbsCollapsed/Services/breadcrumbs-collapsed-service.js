import { ref, onMounted } from "vue";
import { onIntersect } from "./breadcrumbs-intersect";


export function useBreacrumbsCollapsed(crumbs) {
    const observer = ref({});
    const crumbsRef = ref(null);
    const isCollapsed = ref(false);

    onMounted((instance) => {
        console.log(instance)
        observer.value = onIntersect(crumbsRef.value, onCollapse, onExitCollapse);
    });

    function onCollapse() {
        isCollapsed.value = false;
    }

    function onExitCollapse() {
        isCollapsed.value = true;
    }

    return {
        crumbsRef,
        isCollapsed
    };
}