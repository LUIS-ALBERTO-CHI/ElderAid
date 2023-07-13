<template>
    <div class="empty-list-container">
        <i class="fa-solid fa-heart-pulse icon-not-found"></i>
        <span>Aucun élément récent trouvé</span>
    </div>

</template>
<!-- eslint-disable @fwaeu/custom-rules/no-local-storage -->
<script>



    export default {
        data() {
            return {
            };
        },
        props: {

        },
        async created() {
        },
        methods: {
        },
        computed: {

        },

    }
</script>
<style type="text/css">
    .empty-list-container {
        display: flex;
        width: 100%;
        flex-direction: column;
        align-items: center;
        row-gap: 10px;
    }

    .icon-not-found {
        font-size: 48px;
        color: #484349
    }
</style>