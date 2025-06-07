$(document).ready(function () {
    $('#tblPreferences').DataTable({
        "responsive": true,
        "ajax": { url: "/admin/atlete/getallpreferences" },
        "columns": [
            { data: "name", width: "80%" },
            {
                data: "id",
                render: function (data) {
                    return `
                        <div class="w-0 btn-group" role="group">
                            <a href="/Atlete/EditPreference?id=${data}" class="btn btn-warning mx-1">
                                <i class="bi bi-pencil-square"></i>
                            </a>
                            <a href="/Atlete/DeletePreference?id=${data}" class="btn btn-danger mx-1">
                                <i class="bi bi-trash"></i>
                            </a>
                        </div>`;
                },
                width: "20%"
            }
        ],
        "language": {
            "emptyTable": "Nenhuma preferência encontrada."
        }
    });
});
