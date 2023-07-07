<template>
  <div>
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
  methods: {
    onDecode(data) {
      this.scannedCode = data;
    },
    goBack() {
      this.$emit('cancelScan');
    },
    confirm() {
      this.$emit('codeScanned', { qrCodeText: this.scannedCode });
    },
  },
};
</script>
<style scoped>
.stream {
  max-height: 500px;
  max-width: 500px;
  margin: auto;
}
.frame {
  border-style: solid;
  border-width: 2px;
  border-color: red;
  height: 200px;
  width: 200px;
  position: absolute;
  top: 0px;
  bottom: 0px;
  right: 0px;
  left: 0px;
  margin: auto;
}
.buttons {
    display: flex;
    justify-content: center;
    gap: 10px;
    margin-top: 50px;
}
.confirm-button {
    width: 150px;
}

.cancel-button {
    width: 150px;
}
</style>