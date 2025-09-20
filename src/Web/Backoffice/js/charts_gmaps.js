(function($) {
  "use strict";

  function initialize() {
    var latlng = new google.maps.LatLng(18.520266, 73.856406);
    var latlng2 = new google.maps.LatLng(28.579943, 77.330006);
    var myOptions = {
      zoom: 15,
      center: latlng,
      mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    var myOptions2 = {
      zoom: 15,
      center: latlng2,
      mapTypeId: google.maps.MapTypeId.SATELLITE
    };

    var map = new google.maps.Map(
      document.getElementById("map_canvas"),
      myOptions
    );

    var map2 = new google.maps.Map(
      document.getElementById("map_canvas_2"),
      myOptions2
    );

    var myMarker = new google.maps.Marker({
      position: latlng,
      map: map,
      title: "Pune"
    });

    var myMarker2 = new google.maps.Marker({
      position: latlng2,
      map: map2,
      title: "Noida"
    });
  }

  if ($("#map_canvas,#map_canvas_2").length != 0) {
    google.maps.event.addDomListener(window, "load", initialize);
  }
})(jQuery);
