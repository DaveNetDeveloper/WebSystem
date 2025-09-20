// Bootstrap SweetAlert
$(function() {

  if ($("#sweetalert-example-1").length) {
    $("#sweetalert-example-1").click(function() {
      swal("Here's a message!", "It's pretty, isn't it?");
    });
  }

    if ($("#sweetalert-example-2").length) {
      $("#sweetalert-example-2").click(function () {
          swal(
              {
                  title: "Are you sure?",
                  text: "You will not be able to recover this imaginary file!",
                  type: "warning",
                  showCancelButton: true,
                  confirmButtonClass: "btn-danger",
                  confirmButtonText: "Yes, delete it!",
                  cancelButtonText: "No, cancel plx!",
                  closeOnConfirm: false,
                  closeOnCancel: false
              },
              function (isConfirm) {
                  if (isConfirm) {
                      swal("Deleted!", "Your imaginary file has been deleted.", "success");
                  } else {
                      swal("Cancelled", "Your imaginary file is safe :)", "error");
                  }
              }
          );
      });
  }


    if ($("#sweetalert-example-3").length) {
      $("#sweetalert-example-3").click(function () {
          swal(
              {
                  title: "Ajax request example",
                  text: "Submit to run ajax request",
                  type: "info",
                  showCancelButton: true,
                  closeOnConfirm: false,
                  showLoaderOnConfirm: true
              },
              function () {
                  setTimeout(function () {
                      swal("Ajax request finished!");
                  }, 2000);
              }
          );
      });
  }


    if ($("#sweetalert-example-4").length) {
      $("#sweetalert-example-4").click(function () {
          swal({
              title: "Are you sure?",
              text: "You will not be able to recover this imaginary file!",
              type: "info",
              showCancelButton: true,
              confirmButtonClass: "btn-info",
              confirmButtonText: "Info!"
          });
      });
  }


    if ($("#sweetalert-example-5").length) {
      $("#sweetalert-example-5").click(function () {
          swal({
              title: "Are you sure?",
              text: "You will not be able to recover this imaginary file!",
              type: "success",
              showCancelButton: true,
              confirmButtonClass: "btn-success",
              confirmButtonText: "Success!"
          });
      });
  }


    if ($("#sweetalert-example-6").length) {
      $("#sweetalert-example-6").click(function () {
          swal({
              title: "Are you sure?",
              text: "You will not be able to recover this imaginary file!",
              type: "warning",
              showCancelButton: true,
              confirmButtonClass: "btn-warning",
              confirmButtonText: "Warning!"
          });
      });
  }


    if ($("#sweetalert-example-7").length) {
      $("#sweetalert-example-7").click(function () {
          swal({
              title: "Are you sure?",
              text: "You will not be able to recover this imaginary file!",
              type: "error",
              showCancelButton: true,
              confirmButtonClass: "btn-danger",
              confirmButtonText: "Danger!"
          });
      });
  }

});
