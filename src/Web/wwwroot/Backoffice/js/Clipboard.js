$(function() {

    var clipboard = new ClipboardJS(".btn-clip");
        clipboard.on("success", function(e) {
        console.log(e);
    });
        clipboard.on("error", function(e) {
        console.log(e);
    });

});
