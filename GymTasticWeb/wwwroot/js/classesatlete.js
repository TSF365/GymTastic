document.addEventListener("DOMContentLoaded", function () {
    $('#tblData').DataTable({
        responsive: true,
        ajax: {
            url: '/atlete/classes/getall',
            dataSrc: 'data'
        },
        columns: [
            { data: 'classname', width: "15%" },
            { data: 'classtime', width: "20%" },
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
                            <form method="post" action="/atlete/classes/unregister" style="display:inline;">
                                <input type="hidden" name="classId" value="${data.id}" />
                                <input type="hidden" name="__RequestVerificationToken" value="${token}" />
                                <button type="submit" class="btn btn-danger" title="Cancelar Inscrição">
                                    <i class="bi bi-x-circle"></i>
                                </button>
                            </form>
                        `;
                    } else {
                        formHtml = `
                            <form method="post" action="/atlete/classes/register" style="display:inline;">
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
