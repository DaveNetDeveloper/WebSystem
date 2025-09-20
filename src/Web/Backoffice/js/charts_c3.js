$(function() {
  if ($("#c3-graph").length) {
    c3.generate({
      bindto: "#c3-graph",
      color: { pattern: ["#00BCD4", "#607D8B"] },
      data: {
        columns: [
          ["data1", 70, 48, 42, 2, 81, 35],
          ["data2", 61, 61, 51, 94, 52, 55]
        ]
      }
    });
  }

  if ($("#c3-spline").length) {
    c3.generate({
      bindto: "#c3-spline",
      color: { pattern: ["#FF4081", "#795548"] },
      data: {
        type: "spline",
        columns: [
          ["data1", 93, 70, 98, 44, 42, 84],
          ["data2", 39, 18, 90, 4, 61, 38]
        ]
      }
    });
  }

  if ($("#c3-area").length) {
    c3.generate({
      bindto: "#c3-area",
      color: { pattern: ["#E040FB", "#9E9E9E"] },
      data: {
        columns: [
          ["data1", 313, 396, 125, 0, 0, 0],
          ["data2", 125, 446, 370, 169, 248, 434]
        ],
        types: {
          data1: "area",
          data2: "area-spline"
        }
      }
    });
  }

  if ($("#c3-bar").length) {
    c3.generate({
      bindto: "#c3-bar",
      color: { pattern: ["#FF5722", "#4CAF50"] },
      data: {
        columns: [
          ["data1", 492, 118, 124, 332, 262, 182],
          ["data2", 205, 138, 164, 474, 244, 216]
        ],
        type: "bar"
      },
      bar: {
        width: { ratio: 0.5 }
      }
    });
  }

  if ($("#c3-donut").length) {
    c3.generate({
      bindto: "#c3-donut",
      color: {
        pattern: ["#673AB7", "#E040FB", "#D32F2F", "#9E9E9E", "#0288D1"]
      },
      data: {
        columns: [
          ["data1", 79],
          ["data2", 91],
          ["data3", 71],
          ["data4", 85],
          ["data5", 57]
        ],
        type: "donut"
      },
      donut: { title: "Some title" }
    });
  }

  if ($("#c3-gauge").length) {
    c3.generate({
      bindto: "#c3-gauge",
      data: {
        columns: [["data", 67]],
        type: "gauge"
      },
      color: {
        pattern: ["#FF0000", "#F97600", "#F6C600", "#60B044"],
        threshold: {
          values: [30, 60, 90, 100]
        }
      },
      size: {
        height: 180
      }
    });
  }
});
