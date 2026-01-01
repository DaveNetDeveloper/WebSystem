$(function() {
  if ($("#simple-chartist").length) {
    new Chartist.Line(
      "#simple-chartist",
      {
        labels: ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday"],
        series: [[12, 9, 7, 8, 5], [2, 1, 3.5, 7, 3], [1, 3, 4, 5, 6]]
      },
      {
        fullWidth: true,
        chartPadding: {
          right: 40
        }
      }
    );
  }

  if ($("#line-chartist").length) {
    new Chartist.Line(
      "#line-chartist",
      {
        labels: [1, 2, 3, 4, 5, 6, 7, 8],
        series: [[5, 9, 7, 8, 5, 3, 5, 4]]
      },
      {
        low: 0,
        showArea: true
      }
    );
  }

  if ($("#bipolar-chartist").length) {
    new Chartist.Line(
      "#bipolar-chartist",
      {
        labels: [1, 2, 3, 4, 5, 6, 7, 8],
        series: [
          [1, 2, 3, 1, -2, 0, 1, 0],
          [-2, -1, -2, -1, -2.5, -1, -2, -1],
          [0, 0, 0, 1, 2, 2.5, 2, 1],
          [2.5, 2, 1, 0.5, 1, 0.5, -1, -2.5]
        ]
      },
      {
        high: 3,
        low: -3,
        showArea: true,
        showLine: false,
        showPoint: false,
        fullWidth: true,
        axisX: {
          showLabel: false,
          showGrid: false
        }
      }
    );

    var data = {
      labels: [
        "Jan",
        "Feb",
        "Mar",
        "Apr",
        "May",
        "Jun",
        "Jul",
        "Aug",
        "Sep",
        "Oct",
        "Nov",
        "Dec"
      ],
      series: [
        [5, 4, 3, 7, 5, 10, 3, 4, 8, 10, 6, 8],
        [3, 2, 9, 5, 4, 6, 4, 6, 7, 8, 7, 4]
      ]
    };

    var options = {
      seriesBarDistance: 10
    };

    var responsiveOptions = [
      [
        "screen and (max-width: 640px)",
        {
          seriesBarDistance: 5,
          axisX: {
            labelInterpolationFnc: function(value) {
              return value[0];
            }
          }
        }
      ]
    ];
  }

  if ($("#overlapping-bars-chartist").length) {
    new Chartist.Bar(
      "#overlapping-bars-chartist",
      data,
      options,
      responsiveOptions
    );

    // Simple pie chart
    var data = {
      series: [5, 3, 4]
    };

    var sum = function(a, b) {
      return a + b;
    };
  }

  if ($("#simple-pie-chart-chartist").length) {
    new Chartist.Pie("#simple-pie-chart-chartist", data, {
      labelInterpolationFnc: function(value) {
        return Math.round((value / data.series.reduce(sum)) * 100) + "%";
      }
    });
  }

  if ($("#donut-chart-chartist").length) {
    // Donut chart
    new Chartist.Pie(
      "#donut-chart-chartist",
      {
        series: [20, 10, 30, 40]
      },
      {
        donut: true,
        donutWidth: 60,
        donutSolid: true,
        startAngle: 270,
        showLabel: true
      }
    );
  }
});
