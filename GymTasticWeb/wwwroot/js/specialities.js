$(document).ready(function () {
    $('#tblSpecialities').DataTable({
        "responsive": true,
        "ajax": { url: "/admin/trainer/getallspecialities" },
        "columns": [
            { data: "name", width: "80%" },
            {
                data: "id",
                render: function (data) {
                    return `
                        <div class="w-0 btn-group" role="group">
                            <a href="/admin/trainer/editspeciality?id=${data}" class="btn btn-warning mx-1">
                                   <i class="bi bi-pencil-square"></i>
                                </a>
                         <a href="/admin/trainer/deletespeciality?id=${data}" class="btn btn-danger mx-1">
                                   <i class="bi bi-trash"></i>
                        </div>`;
                },
                width: "20%"
            }
        ],
        "language": {
            "emptyTable": "Nenhuma especialidade encontrada."
        }
    });
});
