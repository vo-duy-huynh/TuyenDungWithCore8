$(document).ready(function () {
    $('.apply-button').click(function (e) {
        e.preventDefault(); 

        var jobId = $(this).data('applyjobid'); 

        $.ajax({
            url: '/Customer/JobPost/Check',
            type: 'POST',
            data: { jobId: jobId }, 
            success: function (response) {
                if (response.isExpired || response.isRecruitingFull) {
                    Swal.fire({
                        title: 'Thông báo',
                        text: response.errorMessage,
                        icon: 'error',
                        confirmButtonText: 'OK',
                        footer: '<a href=\'/Customer/JobPost/Apply?id=' + jobId + '\'> Vẫn ứng tuyển?</a>'
                    });
                } else {
                    window.location.href = '/Customer/JobPost/Apply?id=' + jobId;
                }
            }
        });
    });
});
