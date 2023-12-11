var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblDataCompanyComment').DataTable({
        "ajax": { url: '/admin/companycomment/getall' },
        "columns": [
            //if data.userId == data1.id show data1.name
            {data: 'user.name',"width": "15%"},
            { data: 'company.name', "width": "15%" },
            { data: 'description', "width": "38%" },
            { data: 'rate', "width": "7%" },
            { data: 'dateAdded', "width": "15%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">             
                     <a onClick=Delete('/admin/companycomment/delete/${data}') class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Xóa</a>
                    </div>`
                },
                "width": "25%"
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