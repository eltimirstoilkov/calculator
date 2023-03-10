$(document).ready(function() {
	const baseURL = "https://localhost:8080"
    let vehicleTypeSelected = "";
    let manicipalitiSelected = "";
    let tariffTypeSelected = "";
    let purposeSelect = "";
    let ageSelected = "";

    /* Get all vehicle types */
    $.ajax({
        url: baseURL + "/api/VehicleTypes",
        type: 'GET',
        dataType: 'json',
        headers: {
            'Access-Control-Allow-Origin': '*',
        },
        success: function(res) {
            if(res?.length) {
                res?.map((item) => {
                    $("#cars").append('<option value=' + item?.id + '>' + item?.description + '</option')
                })
            }
        }
    });

    /* Select vehicle types */
    $("#cars").change(function() {
		vehicleTypeSelected = $("#cars option:selected").val();

        console.log(Number(vehicleTypeSelected) === 2)

        console.log(Number(purposeSelect) === 3)

        if(Number(vehicleTypeSelected) === 2 && Number(purposeSelect) === 3) {
            $(".section.age").addClass('active');
        } else {
            $(".section.age").removeClass('active');
        }
	});

    /* Get all age */
    $.ajax({
        url: baseURL + "/api/AgeGroups",
        type: 'GET',
        dataType: 'json',
        headers: {
            'Access-Control-Allow-Origin': '*',
        },
        success: function(res) {
            if(res?.length) {
                res?.map((item) => {
                    $("#age").append('<option value=' + item?.id + '>' + item?.description + '</option')
                })
            }
        }
    });

    /* Select age */
    $("#age").change(function() {
		ageSelected = $("#age option:selected").val();
	});

    /* Get all manicipalities */
    $.ajax({
        url: baseURL + "/api/municipalities",
        type: 'GET',
        dataType: 'json',
        headers: {
            'Access-Control-Allow-Origin': '*',
        },
        success: function(res) {
            if(res?.items?.length) {
                res?.items?.map((item) => {
                    $("#cities").append('<option value=' + item?.id + '>' + item?.name + '</option')
                })
            }
        }
    });

    /* Select manicipalities */
    $("#cities").change(function() {
		manicipalitiSelected = $("#cities option:selected").val();
	});

    /* Get all tariff types */
    $.ajax({
        url: baseURL + "/api/tariffTypes",
        type: 'GET',
        dataType: 'json',
        headers: {
            'Access-Control-Allow-Origin': '*',
        },
        success: function(res) {
            if(res?.length) {
                res?.map((item) => {
                    $("#volumeengine").append('<option value=' + item?.id + '>' + item?.description + '</option')
                })
            }
        }
    });

    /* Select volume engine */
    $("#volumeengine").change(function() {
		tariffTypeSelected = $("#volumeengine option:selected").val();
	});

    /* Get all purposes */
    $.ajax({
        url: baseURL + "/api/purposes",
        type: 'GET',
        dataType: 'json',
        headers: {
            'Access-Control-Allow-Origin': '*',
        },
        success: function(res) {
            if(res?.length) {
                res?.map((item) => {
                    $("#use").append('<option value=' + item?.id + '>' + item?.description + '</option')
                })
            }
        }
    });

    /* Select purpose */
    $("#use").change(function() {
	    purposeSelect = $("#use option:selected").val();

        if(Number(vehicleTypeSelected) === 2 && Number(purposeSelect) === 3) {
            $(".section.age").addClass('active');
        } else {
            $(".section.age").removeClass('active');
        }
	});

    $( "#submit-button" ).click(function( event ) {
        event.preventDefault();

        let data = {};

        if(Number($("#cars").val()) === 2 && Number($("#use").val()) === 3) {
            data = {
                vehicleTypeId: $("#cars").val(),
                municipalityId: $("#cities").val(),
                vehicleTariffTypeId: $("#volumeengine").val(),
                vehiclePurposeId: $("#use").val(),
                ownerAgeId: $("#age").val()
            }
        } else {
            data = {
                vehicleTypeId: $("#cars").val(),
                municipalityId: $("#cities").val(),
                vehicleTariffTypeId: $("#volumeengine").val(),
                vehiclePurposeId: $("#use").val(),
                ownerAgeId: null
            }
        }

        fetch(baseURL + '/api/Calculations', {
            method: 'POST',
            headers: { 'accept': 'application/json', 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        }).then((r) => r?.json())
            .then((data) => {
                if(data) {
                    $(".dialog-paragraph").remove();

                    let basePremium = data?.basePremium;
                    let finalPremium = data?.finalPremium;
                    $(".dialog-container").append('<p class="dialog-paragraph"> Базова премия: ' + `${basePremium?.toFixed(2)} лв` + '</p>' + '<p class="dialog-paragraph"> Крайна премия: ' + `${finalPremium?.toFixed(2)} лв` + '</p>')
                    $("#dialg").dialog({ autoOpen: false, hide: "slide", show : "slide", height: 500 });
                    $("#dialg").dialog("open");
                }
            })
      });
})