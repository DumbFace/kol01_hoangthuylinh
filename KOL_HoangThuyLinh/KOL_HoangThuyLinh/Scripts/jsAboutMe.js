

function checkImage() {
    var isAvatar = $('#Avatar').val();

    if (isAvatar == "") {
        $('#error-Avatar').html("Vui lòng upload hình cover");
    } else {
        $('#error-Avatar').html("");
    }
    if (isAvatar != "") {
        return true;
    } else {
        return false;
    }
}


function InsertHTML(data) {
    const viewFragment = editor.data.processor.toView(data);
    const modelFragment = editor.data.toModel(viewFragment);
    editor.model.insertContent(modelFragment);
}






function checkBody() {
    var isBody = $('#Body').val();

    if (isBody == "") {
        $('#error-Body').html("Vui lòng nhập nội dung");
    } else {
        $('#error-Body').html("");
    }
    if (isBody != "") {
        return true;
    } else {
        return false;
    }
}