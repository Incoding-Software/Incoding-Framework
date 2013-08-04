"use strict";

//#region class IncodingInstrumentation

function DefaultIncodingInstrumentation() {

}

DefaultIncodingInstrumentation.prototype =
    {
        fireParse : function(context) {
            console.log('Found element {0} with count meta {1}'.f(context.element, context.countMeta));
        },

        fireInvoke : function(context) {
            /*console.log('Invoke {0} for {1}'.f(context.event, context.element));*/
        },

        fireInitIncoding : function(context) {
            /*console.log('Init incoding for {0}'.f(context.element));*/
        },

        fireUnHandleException : function(e) {
            /*console.error('Ajax un handle exception {0}'.f(e));*/
        },

        fireAjaxBeforeSend : function(jqXhr, settings) {
            console.log('Ajax before send:');
            console.log('xhr {0}'.f(jqXhr));
            console.log('settings {0}'.f(settings));
        },

        fireAjaxComplete : function(jqXhr, textStatus) {
            console.log('Ajax complete');
            console.log('xhr {0}'.f(jqXhr));
            console.log('status {0}'.f(textStatus));
        },

        fireAjaxSuccess : function(data) {
            console.log('Ajax Success:');
            console.log(data);
        },

        fireAjaxAfterParse : function(parseData) {
            console.log('Ajax parse data:');
            console.log(parseData);
        },

        fireAjaxError : function(event, jqXhr, ajaxSettings, thrownError) {
            console.log('Ajax error:');
            console.log(event);
            console.log(jqXhr);
            console.log(ajaxSettings);
            console.log(thrownError);
        },

        fireAjaxSubmitSuccess : function(responseText, statusText, xhr, $form) {
            console.log('Ajax submit success:');
            console.log(responseText);
            console.log(statusText);
            console.log(xhr);
            console.log($form);
        },

        fireAjaxSubmitBefore : function(formData, jqForm, options) {
            console.log('Ajax submit before:');
            console.log(formData);
            console.log(jqForm);
            console.log(options);
        }
    };

DefaultIncodingInstrumentation.Instance = new DefaultIncodingInstrumentation();

//#endregion