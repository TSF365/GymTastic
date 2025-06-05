document.addEventListener("DOMContentLoaded", function () {
    var dataTable = $('#tblData').DataTable({
        "responsive": true,
        "ajax": { url: '/admin/classes/getall' },
        "columns": [
            { data: 'classname', "width": "8%" },
            { data: 'classtime', "width": "10%" },
            { data: 'email', "width": "8%" },
            { data: 'speciality', "width": "10%" },
            { data: 'maxatletes', "width": "10%" },
            { data: 'regatletes', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-0 btn-group" role="group">
                                <a href="/admin/classes/edit?id=${data}" class="btn btn-warning mx-1">
                                   <i class="bi bi-pencil-square"></i>
                                </a>
                                <a href="/admin/classes/delete?id=${data}" class="btn btn-danger mx-1">
                                   <i class="bi bi-trash"></i>
                                </a>
                            </div>`;
                },
                "width": "5%"
            }
        ],
        "lengthMenu": [
            [-1, 50, 25, 10],
            ['Todos', 50, 25, 10]
        ],
        "stateSave": true
    });
});
