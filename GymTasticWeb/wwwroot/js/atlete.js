﻿document.addEventListener("DOMContentLoaded", function () {
    var dataTable = $('#tblData').DataTable({
        "responsive": true,
        "ajax": { url: '/admin/atlete/getall' },
        "columns": [
            { data: 'fullname', "width": "15%" },
            { data: 'year', "width": "3%" },
            { data: 'phonenumber', "width": "8%" },
            { data: 'email', "width": "8%" },
            { data: 'inscriptiondate', "width": "8%", render: (data) => new Date(data).toLocaleDateString('pt-PT') },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-0 btn-group" role="group">
                                <a href="/admin/atlete/edit?id=${data}" class="btn btn-warning mx-1">
                                   <i class="bi bi-pencil-square"></i>
                                </a>
                                <a href="/admin/atlete/delete?id=${data}" class="btn btn-danger mx-1">
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
