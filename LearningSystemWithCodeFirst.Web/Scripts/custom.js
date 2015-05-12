$(document).on('ready', function () {

    $(document).on("click", ".delete", function () {
        var getId = $(this).data('itemid');
        $('#deleteForm').attr('action', function () {
            return this.action.replace('idToReplace', getId);
        });
    });

    // search
    $('#search_input').fastLiveFilter('#search_list');

    //Initialize tinyMCE
    tinymce.init({
        selector: ".tinymce-text-area",
        plugins: [
            "advlist autolink lists link image charmap print preview anchor",
            "searchreplace visualblocks code fullscreen",
            "insertdatetime media table contextmenu paste"
        ],
        toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image",
        file_browser_callback: fileManager
    });

    //FileManager for tinyMCE
    function fileManager(field_name, url, type, win) {
        var w = window,
                d = document,
                e = d.documentElement,
                g = d.getElementsByTagName('body')[0],
                x = w.innerWidth || e.clientWidth || g.clientWidth,
                y = w.innerHeight || e.clientHeight || g.clientHeight;

        var cmsURL = '/Plugins/Filemanager-master/index.html?&field_name=' + field_name + '&lang=' + tinymce.settings.language;

        if (type == 'image') {
            cmsURL = cmsURL + "&type=images";
        }

        tinyMCE.activeEditor.windowManager.open({
            file: cmsURL,
            title: 'Filemanager',
            width: x * 0.8,
            height: y * 0.8,
            resizable: "yes",
            close_previous: "no"
        });
    }
});