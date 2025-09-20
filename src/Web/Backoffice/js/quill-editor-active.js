(function($) {
  "use strict";

  if ($("#quillExample").length) {
    var quill = new Quill('#quillExample', {
      modules: {
        toolbar: [
          [{
            header: [1, 2, false]
          }],
          ['bold', 'italic', 'underline'],
          ['image', 'code-block']
        ]
      },
      placeholder: 'Compose an epic...',
      theme: 'snow' // or 'bubble'
    });
  }

})(jQuery);
