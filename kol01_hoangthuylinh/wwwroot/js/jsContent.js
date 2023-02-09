
const getDimension = (url, cb) => {
    const img = new Image();
    img.onload = () => cb(null, img);
    img.onerror = (err) => cb(err);
    img.src = url;
};


function initValidate() {
    var submitSettings = $.data($("#frmEdit").get(0), 'validator').settings;

    submitSettings.messages = {
        Title: {
            required: "Vui lòng nhập tiêu đề",
        },
        Teaser: {
            required: "Vui lòng nhập tóm tắt",
        },

    };

    submitSettings.highlight = function (e) {
        $(e).addClass('is-invalid');
    };
    submitSettings.unhighlight = function (e) {
        $(e).removeClass('is-invalid');
        $(e).addClass('is-valid');
    };
}

function GetTypeWeb(typeWeb, url) {
    var splitUrl = url.split("/");
    if (splitUrl[2] == "www.yan.vn" || splitUrl[2] == "yan.vn") {
        return "yan";
    }

    if (splitUrl[2] == "www.youtube.com" || splitUrl[2] == "youtube.com") {
        return "youtube";
    }

    if (splitUrl[2] == "www.bestie.vn" || splitUrl[2] == "bestie.vn") {
        return "bestie";
    }

    if (splitUrl[2] == "www.dienanh.net" || splitUrl[2] == "dienanh.net") {
        return "dienanh";
    }

    if (splitUrl[2] == "www.tiin.vn" || splitUrl[2] == "tiin.vn") {
        return "tiin";
    }

    if (splitUrl[2] == "www.kenh14.vn" || splitUrl[2] == "kenh14.vn") {
        return "kenh14";
    }
}

function AutoFill(e) {
    var urlLink = $('#UrlRequest').val();
    var selectWeb = $('#TypeCrawling').val();

    if (urlLink == undefined || urlLink == "") {
        alert("Bạn vui lòng nhập Url bài viết")
        return false;
    }

    var typeWeb = GetTypeWeb(selectWeb, urlLink);

    $.ajax({
        url: `/admincp/content/autofilter?url=${urlLink}&whichWeb=${typeWeb}`,
        type: "GET",
        success: function (data) {
            // console.log(data);
            if (data.status == true) {
                $('#Title').val(data.tbl.title);
                $('#Teaser').val(data.tbl.teaser);
                $('#UrlSource').val(urlLink);
                $("#editor").html("");
                editor.setData("");
                InsertHTML(data.tbl.body);
                if (data.tbl.avatar2x1 != "" && data.tbl.avatar2x1 != undefined) {
                    var img = '<img src="' + data.tbl.avatar2x1 + '" class="img-content" />&nbsp;';
                    $("#loadImgThumbnail").html(img);
                    $("#Avatar2x1").val(data.tbl.avatar2x1);
                    getDimension(data.tbl.avatar2x1, (err, img) => {
                        $("#AvatarWidth").val(img.naturalWidth);
                        $("#AvatarHeight").val(img.naturalHeight);
                    });
                }
                else {
                    alert("Bài viết không có hình Avatar, bạn vui lòng chọn từ máy tính");
                    $('#UploadAvatar').click();
                }

                if (data.tbl.avatarFB != null && data.tbl.avatarFB != undefined) {
                    var img = '<img src="' + data.tbl.avatarFB + '" class="img-content" />&nbsp;';
                    $("#loadImgThumbnailFB").html(img);
                    $("#AvatarFB").val(data.tbl.avatarFB);
                }
            }
        }
    })

}

function initValidateUrl() {
    var submitSettings = $.data($("#frmUrl").get(0), 'validator').settings;

    submitSettings.messages = {
        urlImage: {
            required: "Vui lòng nhập link hình",
        },
    };
}

function checkImage(isSaveTemp) {
    var dimensionAvatar;
    if (isSaveTemp) {
        return true;
    }
    else {
        var isAvatarFB = $('#AvatarFB').val();
        var isAvatar = $('#Avatar2x1').val();

        if (isAvatarFB == "") {
            $('#error-AvatarFB').html("Vui lòng upload hình facebook");
        } else {
            $('#error-AvatarFB').html("");
        }

        if (isAvatar == "") {
            $('#error-Avatar').html("Vui lòng upload hình cover");
        } else {
            $('#error-Avatar').html("");
            dimensionAvatar = checkDimension(isSaveTemp);
        }

        if (isAvatarFB != "" && isAvatar != "" && dimensionAvatar) {
            return true;
        } else {
            return false;
        }
    }
}


function checkDimension(isSaveTemp) {
    if (isSaveTemp) {
        return true;
    }
    else {
        var width = $('#AvatarWidth').val();
        var height = $('#AvatarHeight').val();
        if (width == '480' && height == '480') {
            return true;
        }
        else {
            alert("Bạn vui lòng chọn hình có kích thước 480x480")
            return false;
        }
    }
}



function savecontent(mode, isSaveTemp) {
    if (isSaveTemp) {
        var d = new Date();
        var dateTimeNow = `${d.getDate()}-${d.getMonth() + 1}-${d.getFullYear()} ${d.getHours()}:${d.getMinutes()}:${d.getSeconds()}`;
        var title = $('#Title').val();
        var teaser = $('#Teaser').val();
        if (title == undefined || title == "") {
            $('#Title').val(`Tiêu đề: ${dateTimeNow}`);
        }
        if (teaser == undefined || teaser == "") {
            $('#Teaser').val(`Tóm tắt: ${dateTimeNow}`);
        }
    }

    var isFormValidate = $("#frmEdit").valid();
    var isValidateImage = checkImage(isSaveTemp)
    if (isFormValidate && isValidateImage) {
        $('#Body').val(editor.getData());

        var frm = $("#frmEdit").serializeArray();

        //Nút Lưu tạm thì -1, Nút Lưu thì lấy theo select Options.
        if (isSaveTemp != null) {
            frm.push({ name: "intstatus", value: -1 });
        }
        else {
            frm.push({ name: "intstatus", value: $("#cbStatus").val() });
        }
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


$(function () {
    $("#errAvatar").css("display", "none");
    $("#errAvatarFB").css("display", "none");
    $("#errMasterTag").css("display", "none");
});


function SaveUrl() {
    if ($("#frmUrl").valid()) {
        var id = parseInt($("#IdTypeImage").val());
        var url = $("#urlImage").val();
        var img = '<img src="' + url + '" class="img-content" />&nbsp;';
        switch (id) {
            case 1:
                InsertHTML(img);
                break;
            case 2:
                $("#loadImgThumbnail").html(img);
                $("#Avatar2x1").val(url);
                getDimension(url, (err, img) => {
                    $("#AvatarWidth").val(img.naturalWidth);
                    $("#AvatarHeight").val(img.naturalHeight);
                });
                break;
            case 3:
                $("#loadImgThumbnailFB").html(img);
                $("#AvatarFB").val(url);
                break;
        }
        $('#frmdlg').modal('hide');
    }
}


function UploadAvatar(e, id) {
    var goUpload = true;
    var uploadFile = e.target.files[0];
    if (!(/\.(jpg|png)$/i).test(uploadFile.name)) { //Image jpg, png
        confirm("Bạn vui lòng chọn file png hoặc jpg")
        goUpload = false;
    }
    if (uploadFile.size > 3000000) { // 3MB
        confirm("Bạn vui lòng chọn file hình có kích thước bé hơn 3MB")
        goUpload = false;
    }
    if (goUpload == true) {
        var fileData = new FormData();
        fileData.append(uploadFile.name, uploadFile);
        $.ajax({
            type: "POST",
            url: "/admincp/uploads/SaveImageContent",
            contentType: false,
            processData: false,
            data: fileData,
            success: function (result) {
                if (result.status == true) {
                    var urlImage = window.location.origin + "/uploads/" + result.fileName;
                    $("#loadImgThumbnail").html(`<img src="${urlImage}" class="img-content" />&nbsp;`);
                    $("#Avatar2x1").val(result.fileName);
                }
            }
        })
    }
}

DecoupledDocumentEditor
    .create(document.querySelector('#editor'), {
        mediaEmbed: {
            previewsInData: true,

        },
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


function InsertHTML(data) {
    const viewFragment = editor.data.processor.toView(data);
    const modelFragment = editor.data.toModel(viewFragment);
    editor.model.insertContent(modelFragment);
}

