(function($) {
  "use strict";

  if ($(".form_datetime").length) {
    $(".form_datetime").datetimepicker({
      format: "dd MM yyyy - hh:ii"
    });
  }
})(jQuery);
