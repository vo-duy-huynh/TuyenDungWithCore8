var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/user/getall' },
        "columns": [
            { "data": "fullName", "width": "12%" },
            { "data": "email", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "company.name", "width": "15%" },
            { "data": "role", "width": "12%" },
            {
                data: { companyId: "companyId", company: "company" },
                "render": function (data) {
                    if (data.company.isApproved == false) {
                        return `
                    <div class="text-center">
                        <a onclick=ApproveCompany('${data.companyId}') class="btn btn-danger text-white" style="cursor:pointer; width:70px;">
                        <i class="bi bi-lock-fill"></i> Từ Chối
                        </a> 
                    </div>
                        `;
                    } else if (data.company.isApproved == true) {
                        return `
                    <div class="text-center">
                        <a onclick=ApproveCompany('${data.companyId}') class="btn btn-success text-white" style="cursor:pointer; width:70px;">
                        <i class="bi bi-unlock-fill"></i> Xác Nhận
                        </a>
                    </div>
                    `;
                    }
                    else {
                        return `
                    <div class="text-center">

                        </div>
                    `;
                    }
                    
                },
                "width": "10%"
            },

            {
                data: { id: "id", lockoutEnd: "lockoutEnd" },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();

                    if (lockout > today) {
                        return `
                        <div class="text-center">
                             <a onclick=LockUnlock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:100px;">
                                    <i class="bi bi-lock-fill"></i>  Khóa
                                </a> 
                                <a href="/admin/user/RoleManagment?userId=${data.id}" class="btn btn-danger text-white" style="cursor:pointer; width:170px;">
                                     <i class="bi bi-pencil-square"></i> Phân Quyền
                                </a>
                        </div>
                    `
                    }
                    else {
                        return `
                        <div class="text-center">
                              <a onclick=LockUnlock('${data.id}') class="btn btn-success text-white" style="cursor:pointer; width:150px;">
                                    <i class="bi bi-unlock-fill"></i>  Mở Khóa
                                </a>
                                <a href="/admin/user/RoleManagment?userId=${data.id}" class="btn btn-danger text-white" style="cursor:pointer; width:170px;">
                                     <i class="bi bi-pencil-square"></i> Phân Quyền
                                </a>
                        </div>
                    `
                    }


                },
                "width": "31%"
            }
        ]
    });
}

function ApproveCompany(companyId) {
    $.ajax({
        type: "POST",
        url: '/Admin/User/Approve',
        data: JSON.stringify(companyId),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
        }
    });
}
function LockUnlock(id) {
    $.ajax({
        type: "POST",
        url: '/Admin/User/LockUnlock',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
        }
    });
}