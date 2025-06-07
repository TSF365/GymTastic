document.addEventListener("DOMContentLoaded", function () {
    $('#tblData').DataTable({
        responsive: true,
        ajax: {
            url: '/atlete/classes/getallsuggestions',
            dataSrc: 'data'
        },
        columns: [
            { data: 'classname', width: "15%" },
            {
                data: 'classtime',
                "width": "10%",
                "render": function (data) {
                    if (!data) return "";
                    var date = new Date(data);
                    var day = String(date.getDate()).padStart(2, '0');
                    var month = String(date.getMonth() + 1).padStart(2, '0');
                    var year = date.getFullYear();
                    var hours = String(date.getHours()).padStart(2, '0');
                    var minutes = String(date.getMinutes()).padStart(2, '0');
                    return `${day}/${month}/${year} ${hours}:${minutes}`;
                }
            },
            { data: 'email', width: "15%" },
            { data: 'speciality', width: "10%" },
            { data: 'maxatletes', width: "10%" },
            { data: 'regatletes', width: "10%" },
            {
                data: null,
                render: function (data) {
                    const token = $('input[name="__RequestVerificationToken"]').val();
                    let formHtml = '';

                    if (data.isRegistered) {
                        formHtml = `
                            <form method="post" action="/atlete/classes/unregistersugestion" style="display:inline;">
                                <input type="hidden" name="classId" value="${data.id}" />
                                <input type="hidden" name="__RequestVerificationToken" value="${token}" />
                                <button type="submit" class="btn btn-danger" title="Cancelar Inscrição">
                                    <i class="bi bi-x-circle"></i>
                                </button>
                            </form>
                        `;
                    } else {
                        formHtml = `
                            <form method="post" action="/atlete/classes/registersugestion" style="display:inline;">
                                <input type="hidden" name="classId" value="${data.id}" />
                                <input type="hidden" name="__RequestVerificationToken" value="${token}" />
                                <button type="submit" class="btn btn-success" title="Inscrever-se">
                                    <i class="bi bi-check-circle"></i>
                                </button>
                            </form>
                        `;
                    }

                    return formHtml;
                },
                width: "10%"
            }
        ],
        lengthMenu: [[-1, 50, 25, 10], ['Todos', 50, 25, 10]],
        stateSave: true
    });
});
