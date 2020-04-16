$(function () {
	fetch("/api/Measurements")
		.catch(function (err) {
			console.log("catch " + err);
		})
		.then(function (response) {
			console.log("then response " + response);
			return response.json();
		})
		.then(function (json) {
			console.log("then makeChart");
			makeChart(json);
		});

	fetch("/api/Measurements/Latest")
		.catch(function (err) {
			console.log("catch " + err);
		})
		.then(function (response) {
			console.log("then response " + response);
			return response.json();
		})
		.then(function (json) {
			console.log("then latestDatapoint");
			latestDatapoint(json);
		});
});

function makeChart(json) {
	let contentCtx = document.getElementById('content-chart').getContext('2d');
	let contentChart = new Chart(contentCtx, {
		type: 'line',
		data: {
			datasets: [{
				data: json.map(e => ({ x: e.timestamp, y: e.fillContentPercentage })),
				borderColor: "#1c522c",
				lineTension: 0.2,
				fill: false
			}]
		},
		options: {
			legend: {
				display: false
			},
			scales: {
				xAxes: [{
					type: "time"
				}],
				yAxes: [{
					ticks: {
						suggestedMin: 0,
						suggestedMax: 100
					},
					scaleLabel: {
						display: true,
						labelString: "Fyllgrad %",
						padding: 0
					}
				}]
			}
		}
	});

	let temperatureCtx = document.getElementById('temperature-chart').getContext('2d');
	let temperatureChart = new Chart(temperatureCtx, {
		type: 'line',
		data: {
			datasets: [{
				data: json.map(e => ({ x: e.timestamp, y: e.temperature })),
				borderColor: "#1c522c",
				lineTension: 0.2,
				fill: false
			}]
		},
		options: {
			legend: {
				display: false
			},
			scales: {
				xAxes: [{
					type: "time"
				}],
				yAxes: [{
					ticks: {
						//suggestedMin: 0,
						//suggestedMax: 30
					},
					scaleLabel: {
						display: true,
						labelString: "Temperatur",
						padding: 0
					}
				}]
			}
		}
	});
}

function latestDatapoint(json) {
	let fillDescription;

	if (json.fillContentPercentage < 10) {
		fillDescription = "Tom";
	} else if (json.fillContentPercentage < 30) {
		fillDescription = "Noe innhold";
	} else if (json.fillContentPercentage < 60) {
		fillDescription = "Halvfull";
	} else if (json.fillContentPercentage < 80) {
		fillDescription = "Ganske full";
	} else {
		fillDescription = "Full";
	}

	$("#lastMeasurement").text(fillDescription + ", sist sjekket " + new Date(json.timestamp).toLocaleString());
}
