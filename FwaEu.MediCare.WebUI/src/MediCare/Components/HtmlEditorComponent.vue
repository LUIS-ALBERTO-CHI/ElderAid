<template>
    <DxHtmlEditor class="xdlol"
                  :value="editorValue"
                  @valueChanged="onValueChanged"
                  height="200px"
                  width="100%">
        <DxToolbar class="xdlol" :multiline="false">
            <dx-item name="undo" />
            <dx-item name="redo" />
            <dx-item name="separator" />
            <dx-item name="bold" />
            <dx-item name="italic" />
            <dx-item name="strike" />
            <dx-item name="underline" />
            <dx-item name="separator" />
            <dx-item name="alignLeft" />
            <dx-item name="alignCenter" />
            <dx-item name="alignRight" />
            <dx-item name="alignJustify" />
            <dx-item name="separator" />
            <dx-item name="orderedList" />
            <dx-item name="bulletList" />
            <dx-item name="separator" />
            <dx-item name="color" />
            <dx-item name="background" />
            <dx-item name="separator" />
            <dx-item name="link" />
            <dx-item name="separator" />
            <dx-item name="blockquote" />
            <dx-item name="separator" />
            <dx-item name="clear" />
            <dx-item :accepted-values="sizeValues"
                     name="size" />
            <dx-item :accepted-values="headerValues"
                     name="header" />
            <dx-item :accepted-values="fontValues"
                     name="font" />
        </DxToolbar>
    </DxHtmlEditor>

</template>

<script>
    import LocalizationMixin from '@/Fwamework/Culture/Services/single-file-component-localization-mixin';
    import { loadMessages } from 'devextreme/localization';
    import {
        DxHtmlEditor,
        DxToolbar,
        DxItem,
    } from 'devextreme-vue/html-editor';
    import translationFrJson from './Content/html-editor-translation.fr.json';
    import translationEnJson from './Content/html-editor-translation.en.json'



    export default {
        components: {
            DxHtmlEditor,
            DxToolbar,
            DxItem
        },
        props: {
            editorValue: {
                type: String,
                default: ""
            },
        },
        data() {
            return {
                sizeValues: ['8pt', '10pt', '12pt', '14pt', '18pt', '24pt', '36pt'],
                fontValues: ['Arial', 'Courier New', 'Georgia', 'Impact', 'Lucida Console', 'Tahoma', 'Times New Roman', 'Verdana'],
                headerValues: [false, 1, 2, 3, 4, 5],
            };
        },
        async created() {
            loadMessages({
                'fr': translationFrJson,
                'en': translationEnJson
            });
        },
        mixins: [LocalizationMixin],
        i18n: {
            messages: {
                getMessagesAsync(locale) {
                    return import(`./Content/actor-details-messages.${locale}.json`);
                }
            }
        },
        methods: {
            onValueChanged(e) {
                this.$emit('change-editor-value', e.value)
            },
        },

    }
</script>
<style>
    .dx-toolbar-menu-section .dx-toolbar-hidden-button .dx-button {
        padding: 4px 4px 8px 8px !important;
    }

    .dx-toolbar-items-container {
        padding: 0px !important;
    }

    .dx-htmleditor-toolbar-format.dx-size-format {
        width: 100% !important;
    }
</style>