using System.ComponentModel.DataAnnotations;

namespace TuyenDungWeb.Models
{
    public class JobPostTemp
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Hãy nhập tiêu đề tin!!!")]
        public string Heading { get; set; }
        [Required(ErrorMessage = "Hãy nhập nội dung tin!!!")]
        public string Content { get; set; }
        [Required(ErrorMessage = "Hãy nhập mô tả ngắn!!!")]
        public string? ShortDescription { get; set; }

        public DateTime? CreatedDate { get; set; }
        [Required(ErrorMessage = "Hãy chọn hạn nạp hồ sơ!!!")]
        [CustomValidation(typeof(JobPostTemp), "ValidateEndDate")]
        public DateTime? EndDate { get; set; }
        public static ValidationResult ValidateEndDate(DateTime? endDate, ValidationContext context)
        {
            if (endDate.HasValue && endDate <= DateTime.Now)
            {
                return new ValidationResult("Hạn nạp hồ sơ phải sau ngày hiện tại.");
            }

            return ValidationResult.Success;
        }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tuyển dụng phải lớn hơn 0")]
        public int NumberOfRecruiting { get; set; }
        [Required(ErrorMessage = "Hãy nhập số năm !!!")]
        [Range(0, int.MaxValue, ErrorMessage = "Số năm kinh nghiệm phải lớn hơn 0")]
        public double? Experience { get; set; }
        [Required(ErrorMessage = "Hãy nhập giới tính!!!")]
        public string? Gender { get; set; }
        [Required(ErrorMessage = "Hãy nhập mức lương!!!")]
        public string? Salary { get; set; }
        [Required(ErrorMessage = "Hãy nhập địa điểm!!!")]
        public string? Location { get; set; }
        [Required(ErrorMessage = "Hãy nhập loại công việc!!!")]
        public int JobTypeId { get; set; }
        [Required(ErrorMessage = "Hãy nhập công ty!!!")]
        public int CompanyId { get; set; }
        public int? JobPostId { get; set; }
        public int? JobId { get; set; }
        public string Message { get; set; }
        public bool IsApprove { get; set; }
        public string UserIdSend { get; set; }
        public string UserIdReceive { get; set; }
    }
}
