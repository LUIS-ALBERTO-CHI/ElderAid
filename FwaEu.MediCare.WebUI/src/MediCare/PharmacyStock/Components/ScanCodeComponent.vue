<template>
  <div v-show="showScanner">
    <div class="title" style="text-align: center;">
      Mettez le code QR à l'intérieur de la boîte.
    </div>
    <div class="center stream">
      <qr-stream @decode="onDecode" class="mb">
        <div style="color: red;" class="frame"></div>
      </qr-stream>
      Résultat du scan: {{ scannedCode }}
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

export default {
  components: {
    QrStream,
    Button,
  },
  data() {
    return {
      data: null,
      scannedCode: null,
      showScanner: true,
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
      this.$emit('cancelScan');
    },
    confirm() {
      this.$emit('codeScanned', { qrCodeText: this.scannedCode });
      this.showScanner = false;
    },
  },
};
</script>
<style type="text/css" scoped src="./Content/articles.css"></style>
