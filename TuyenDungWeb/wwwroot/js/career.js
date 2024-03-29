﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/career/getall' },
        "columns": [
            { "data": "name", "width": "30%" },
            {
                data: 'id',
                "render": function (data, type, row, meta) {
                    var name = row.name;
                    return `
                    <div class="w-75 btn-group" role="group">
                        <a asp-controller="Career" asp-action="Edit" asp-route-id="${data}" class="btn btn-primary mx-2 edit-button" 
                           onclick="openEditModal('${data}', '${name}')" data-bs-toggle="modal" data-bs-target="#editModel">
                            <i class="bi bi-pencil-square"></i> Cập nhật
                        </a>

                     <a onClick=Delete('/admin/career/delete/${data}') class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Xóa</a>
                    </div>
                    `;
                },
                "width": "30%"
            }
        ]
    });
}
function Delete(url) {
    Swal.fire({
        title: 'Bạn có muốn xóa?',
        text: "Bạn có muốn xóa dòng này không?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Vâng, hãy xóa nó!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}