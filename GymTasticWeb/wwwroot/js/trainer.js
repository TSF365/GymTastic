document.addEventListener("DOMContentLoaded", function () {
    var dataTable = $('#tblData').DataTable({
        "responsive": true,
        "ajax": { url: '/admin/trainer/getall' },
        "columns": [
            { data: 'fullname', "width": "15%" },
            { data: 'phonenumber', "width": "8%" },
            { data: 'email', "width": "8%" },
            {
                data: 'speciality',
                render: function (data) {
                    return data.map(s => `${s}`).join(", ");
                },
                "width": "15%"
            },
            { data: 'tptd', "width": "3%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-0 btn-group" role="group">
                                <a href="/admin/trainer/edit?id=${data}" class="btn btn-warning mx-1">
                                   <i class="bi bi-pencil-square"></i>
                                </a>
                                <a href="/admin/trainer/delete?id=${data}" class="btn btn-danger mx-1">
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
