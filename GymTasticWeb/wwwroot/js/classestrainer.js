document.addEventListener("DOMContentLoaded", function () {
    var dataTable = $('#tblData').DataTable({
        "responsive": true,
        "ajax": { url: '/trainer/classes/getall' },
        "columns": [
            { data: 'classname', "width": "8%" },
            { data: 'classtime', "width": "10%" },
            { data: 'email', "width": "8%" },
            { data: 'speciality', "width": "3%" },
            { data: 'maxatletes', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-0 btn-group" role="group">
                                <a href="/trainer/classes/edit?id=${data}" class="btn btn-warning mx-1">
                                   <i class="bi bi-pencil-square"></i>
                                </a>
                                <a href="/trainer/classes/delete?id=${data}" class="btn btn-danger mx-1">
                                   <i class="bi bi-trash"></i>
                                </a>
                            </div>`;
                },
                "width": "10%"
            }
        ],
        "lengthMenu": [
            [-1, 50, 25, 10],
            ['Todos', 50, 25, 10]
        ],
        "stateSave": true
    });
});
