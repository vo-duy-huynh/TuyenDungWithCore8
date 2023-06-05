using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace TuyenDungWeb.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Note { get; set; }
        public int? CareerId { get; set; }
        [ForeignKey("CareerId")]
        [ValidateNever]
        public Career? Career { get; set; }
    }
}
