    document.addEventListener("DOMContentLoaded", function () {
    $('#tblFeedbacks').DataTable({
        "responsive": true,
        "ajax": { url: '/admin/classes/getallfeedbacks' },
        "columns": [
            { data: 'className', width: "20%" },
            { data: 'atleteName', width: "20%" },
            { data: 'comment', width: "30%" },
            { data: 'rating', width: "10%" },
            { data: 'date', width: "20%" }
        ],
        "lengthMenu": [
            [-1, 50, 25, 10],
            ['Todos', 50, 25, 10]
        ],
        "stateSave": true
    });
});
