<template>
  <div class="page-cabinet">
    <div class="vignette-list">
      <div @click="goToArticlesInStock(cabinet.id)" class="vignette-item" v-for="cabinet in cabinets" :key="cabinet.id">
        <div>
          <i class="fa-regular fa-inbox vignette-icon fa-fw" style="color: #d9c4b3" />
          <span class="vignette-text">{{ cabinet.name }}</span>
        </div>
        <i class="fa-regular fa-angle-right chevron-icon" />
      </div>
    </div>
  </div>
</template>
<script>
import CabinetsMasterDataService from "@/MediCare/Referencials/Services/cabinets-master-data-service";

export default {
  data() {
    return {
      cabinets: [],
    };
  },
  async created() {
    this.cabinets = await CabinetsMasterDataService.getAllAsync();
  },
  methods: {
    goToArticlesInStock(cabinetId) {
      this.$router.push({ name: "Cabinet", params: { id: cabinetId } });
    },
  },
};
</script>