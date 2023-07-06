<template>
  <div v-if="showScanner">
    <div class="title" style="text-align: center;">
      Mettez le code QR à l'intérieur de la boîte.
    </div>
    <div class="center stream">
      <qr-stream @decode="onDecode" class="mb">
        <div style="color: red;" class="frame"></div>
      </qr-stream>
    </div>
    <div class="buttons">
      <Button @click="confirm" label="Confirmer"></Button>
      <Button @click="goBack" label="Annuler"></Button>
    </div>
  </div>
</template>

<script>
import { QrStream } from 'vue3-qr-reader';
import Button from 'primevue/button';
import CabinetsMasterDataService from "@/MediCare/Referencials/Services/cabinets-master-data-service";

export default {
  components: {
    QrStream,
    Button,
  },
  data() {
    return {
      data: null,
      cabinetName: '',
      scannedCode: null,
      showScanner: true
    };
  },
  props: {
    searchValue: {
      type: String,
      default: ""
    }
  },
  created() {
    this.getCurrentCabinetAsync();
  },
  methods: {
    onDecode(data) {
      this.scannedCode = data;
    },
    goBack() {
      this.showScanner = false;
    },
    async getCurrentCabinetAsync() {
      const cabinetId = this.$route.params.id;
      const cabinet = await CabinetsMasterDataService.getAsync(cabinetId);
      this.cabinetName = cabinet.name;
      return cabinet;
    },
    confirm() {
      this.$emit('codeScanned', { qrCodeText: this.scannedCode });
      this.showScanner = false;
    },
  },
};
</script>
<style type="text/css" scoped src="./Content/articles.css"></style>
