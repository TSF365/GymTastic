document.addEventListener("DOMContentLoaded", function () {
    var dataTable = $('#tblData').DataTable({
        "responsive": true,
        "ajax": { url: '/atlete/PersonalizedTraining/getall' },
        "columns": [
            { data: 'trainingname', "width": "8%" },
            { data: 'atlete', "width": "10%" },
            { data: 'trainer', "width": "8%" },
            { data: 'trainingobjective', "width": "3%" },
            { data: 'email', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-0 btn-group" role="group">
                                <a href="/atlete/PersonalizedTraining/edit?id=${data}" class="btn btn-warning mx-1">
                                   <i class="bi bi-pencil-square"></i>
                                </a>`;
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
