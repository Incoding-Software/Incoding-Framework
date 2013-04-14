"use strict";

var incodingEngine;
$(document).ready(function () {
    incodingEngine = new IncodingEngine();
    incodingEngine.parse(document);
    incodingEngine.domInspect();
});