using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace TuyenDungWeb.Models
{
    public class CompanyFollow
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [ValidateNever]
        public Company Company { get; set; }
    }
}
