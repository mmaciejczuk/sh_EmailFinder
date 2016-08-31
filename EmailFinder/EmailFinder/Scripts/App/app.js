$(document).ready(function () {
    $("#find").click(getEmailsList);
});

function getEmailsList() {
    const name = $("#name").val();
    const surname = $("#surname").val();
    const domain = $("#domain").val();
    const url = `/Finder/GetEmails?name=${name}&surname=${surname}&domain=${domain}`;
    $("#emails-list").html("");
    $("#emails-list-loader").show();
    $.get(url, function (data) {
        $("#emails-list-loader").hide();
        $("#emails-list").html(data);
        checkEmails();
    });
}

function checkEmails() {
    var loader = "";

    const discoveryServices = $('input[type=checkbox]:checked');

    for (var i = 0; i < discoveryServices.length; i++) {
        loader += `<span data-discoveryService='${discoveryServices[i].id}'><span class='glyphicon glyphicon-refresh glyphicon-loader'></span>${discoveryServices[i].getAttribute("data-description")}</span>`;
    }


    $(".list-group-item > .finder-result").html(loader);

    $(".list-group-item").each(function () {

        const email = $(this).attr("data-email");
        discoveryServices.each(function() {
            const url = "/Finder/EmailExists?email=" + email + "&service=" + $(this).attr("id");
            $.get(url, function (data) {
                var row = $("li[data-email='" + data.Email + "']");
                var loader = row.find("span[data-discoveryservice='" + data.Service + "']");

                if (data.IsValid) {
                    loader.addClass("text-success");
                    loader.find(".glyphicon-loader").removeClass("glyphicon-refresh glyphicon-loader").addClass("glyphicon-ok-sign text-success");
                    var x = $("<span>" + " / " + data.Limit + " / " + data.Score + "</span>");
                    x.addClass("dataContent");
                    loader.append(x);
                } else {
                    loader.addClass("text-danger");
                    loader.find(".glyphicon-loader").removeClass("glyphicon-refresh glyphicon-loader").addClass("glyphicon-remove-sign text-danger");
                    var x = $("<span>" + " / " + data.Limit + " / " + data.Score + "</span>");
                    x.addClass("dataContent");
                    loader.append(x);
                }
            });
        });
    });
}
