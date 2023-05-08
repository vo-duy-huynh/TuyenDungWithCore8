var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/company/jobpost/getall' },
        "columns": [
            { data: 'name', "width": "15%" },
            { data: 'companyEmail', "width": "15%" },
            { data: 'phoneNumber', "width": "10%" },
            { data: 'location', "width": "25%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                     <a href="/admin/company/upsert?id=${data}" class="btn btn-primary"> <i class="bi bi-pencil-square"></i>Cập nhật</a>               
                     <a onClick=Delete('/admin/company/delete/${data}') class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Xóa</a>
                    </div>`
                },
                "width": "35%"
            }
        ]
    });
}