import { ref, onMounted } from "vue";
import { onIntersect } from "./breadcrumbs-intersect";


export function useBreacrumbsCollapsed() {
    const observer = ref({});
    const crumbsRef = ref(null);
    const isCollapsed = ref(false);

    onMounted(() => {
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