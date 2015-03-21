function resetActive(event, percent, step) {
    $(".progress-bar").css("width", percent + "%").attr("aria-valuenow", percent);
    $(".progress-completed").text(percent + "%");

    $("div").each(function () {
        if ($(this).hasClass("activestep")) {
            $(this).removeClass("activestep");
        }
    });

    if (event.target.className == "col-md-2") {
        $(event.target).addClass("activestep");
    }
    else {
        $(event.target.parentNode).addClass("activestep");
    }

    hideSteps();
    showCurrentStepInfo(step);
}

function hideSteps() {
    $("div").each(function () {
        if ($(this).hasClass("activeStepInfo")) {
            $(this).removeClass("activeStepInfo");
            $(this).addClass("hiddenStepInfo");
        }
    });
}

function showCurrentStepInfo(step) {
    var id = "#" + step;
    $(id).addClass("activeStepInfo");
}

function openModal() {
    $('#myModal').modal({ show: true });
}

function openTimeMin() {

    var inputMin = $('#inputTimeMin').clockpicker({
        autoclose: true

    });

    $('#btnInputTimeMin').click(function (e) {

        e.stopPropagation();
        inputMin.clockpicker('show')
                .clockpicker('toggleView', 'minutes');
    });

}

function openTimeMax() {

    var inputMax = $('#inputTimeMax').clockpicker({
        autoclose: true

    });

    $('#btnInputTimeMax').click(function (e) {

        e.stopPropagation();
        inputMax.clockpicker('show')
                .clockpicker('toggleView', 'minutes');
    });
}

function notificationError(errorType) {

    var error = errorType;
    switch (error) {

        case 1: UIkit.notify('Les informations entrées sont incorrectes.', { status: 'danger' });
            break;
        case 2: UIkit.notify('L&#39email renseigné n&#39est pas valide.', { status: 'danger' });
            break;
        case 3: UIkit.notify('Vous n&#39êtes pas autorisé à effectuer une demande de visite pour ce patient, veuillez ressayer ultérieurement.', { status: 'danger' });
            break;
        case 4: UIkit.notify('Aucun patient n&#39a été trouvé, veuillez vérifiez les informations du patient.', { status: 'danger' });
            break;
    }
}