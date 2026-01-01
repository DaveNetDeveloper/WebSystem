$(function() {
  // Wrap charts

  if ($("#chart-bars").length) {
    var barsChart = new Chart(
      document.getElementById("chart-bars").getContext("2d"),
      {
        type: "bar",
        data: {
          labels: ["Italy", "UK", "USA", "Germany", "France", "Japan"],
          datasets: [
            {
              label: "2010 customers #",
              data: [53, 99, 14, 10, 43, 27],
              borderWidth: 1,
              backgroundColor: "rgba(205, 220, 57, 0.3)",
              borderColor: "#CDDC39"
            },
            {
              label: "2014 customers #",
              data: [55, 74, 20, 90, 67, 97],
              borderWidth: 1,
              backgroundColor: "rgba(103, 58, 183, 0.3)",
              borderColor: "#673AB7"
            }
          ]
        },

        // Demo
        options: {
          responsive: false,
          maintainAspectRatio: false
        }
      }
    );
  }

  if ($("#chart-doughnut").length) {
    var doughnutChart = new Chart(
      document.getElementById("chart-doughnut").getContext("2d"),
      {
        type: "doughnut",
        data: {
          labels: ["Red", "Blue", "Yellow"],
          datasets: [
            {
              data: [137, 296, 213],
              backgroundColor: ["#FF6384", "#36A2EB", "#FFCE56"],
              hoverBackgroundColor: ["#FF6384", "#36A2EB", "#FFCE56"]
            }
          ]
        },

        // Demo
        options: {
          responsive: false,
          maintainAspectRatio: false
        }
      }
    );
  }
});
