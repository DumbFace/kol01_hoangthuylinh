function initValidate() {
    var submitSettings = $.data($("#frmEdit").get(0), 'validator').settings;

    submitSettings.messages = {
        Title: {
            required: "Vui lòng nhập tiêu đề",
        },
        Teaser: {
            required: "Vui lòng nhập tóm tắt",
        }
    };
    submitSettings.highlight = function (e) {
        $(e).closest('.form-group').find(".form-control:first").addClass('is-invalid');
    };
    submitSettings.unhighlight = function (e) {
        $(e).closest('.form-group').find(".form-control:first").removeClass('is-invalid');
        $(e).closest('.form-group').find(".form-control:first").addClass('is-valid');
    };
}

function initValidateUrl() {
    var submitSettings = $.data($("#frmUrl").get(0), 'validator').settings;

    submitSettings.messages = {
        urlImage: {
            required: "Vui lòng nhập link hình",
        },
    };
}

function savecontent(mode) {
    if ($("#frmEdit").valid()) {
        $('#Body').val(editor.getData());

        var frm = $("#frmEdit").serializeArray();
        frm.push({ name: "intstatus", value: $("#cbStatus").val() });
        frm.push({ name: "intFlag", value: mode });
        frm.push({ name: "tags", value: $("#MasterTag").val() });
        frm.push({ name: "dtPublic", value: $("#PublishDate").data('daterangepicker').startDate.format("YYYY/MM/DD HH:mm") });

        $.ajax({
            url: '/admincp/content/savenews',
            type: 'POST',
            data: frm,
            success: function (data) {
                window.location.href = document.referrer;
            },
            error: function (data) { }
        });
    }
}

DecoupledDocumentEditor
    .create(document.querySelector('#editor'), {
        mediaEmbed: { previewsInData: !0 },
        toolbar: {
            items: [
                'heading', '|', 'fontSize', 'fontFamily', '|', 'bold', 'italic', 'underline', 'strikethrough', 'highlight', '|', 'alignment',
                '|', 'numberedList', 'bulletedList', '|', 'indent', 'outdent',
                '|', 'todoList', 'link', 'blockQuote', 'imageUpload', 'insertTable', 'mediaEmbed', '|', 'undo', 'redo'
            ]
        },
        image: {
            toolbar: [
                'imageTextAlternative', 'imageStyle:full', 'imageStyle:side'
            ]
        },
        table: {
            contentToolbar: [
                'tableColumn', 'tableRow', 'mergeTableCells'
            ]
        },
    })
    .then(editor => {
        window.editor = editor;
        document.querySelector('.document-editor__toolbar').appendChild(editor.ui.view.toolbar.element);
        document.querySelector('.ck-toolbar').classList.add('ck-reset_all');
    })
    .catch(error => {
    });

$(function () {
    $("#errAvatar").css("display", "none");
    $("#errAvatarFB").css("display", "none");
    $("#errMasterTag").css("display", "none");
});

function InsertHTML(data) {
    const viewFragment = editor.data.processor.toView(data);
    const modelFragment = editor.data.toModel(viewFragment);
    editor.model.insertContent(modelFragment);
}

function SaveUrl() {
    if ($("#frmUrl").valid()) {
        var id = parseInt($("#IdType").val());
        var url = $("#urlImage").val();
        var img = '<img src="' + url + '" class="img-content" />&nbsp;';
        switch (id) {
            case 1:
                InsertHTML(img);
                break;
            case 2:
                $("#loadImgThumbnail").html(img);
                $("#Avatar").val(url);
                break;
            case 3:
                $("#loadImgThumbnailFB").html(img);
                $("#AvatarFB").val(url);
                break;
        }
        $('#frmdlg').modal('hide');
    }

}