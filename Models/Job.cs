using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication3.Models;
using System.Web.Mvc;
namespace Job_Offers_Website.Models
{
    public class Job
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }
        [Display(Name = "Job Description")]
        [DataType(DataType.MultilineText)]
       // [AllowHtml]
        public string JobContent { get; set; }
        [Display(Name="Job Image")]
        public string  JobImg { get; set; }
        [Required]
        [Display(Name ="Type Job")]
        public int CategoryId { get; set; }
        public string UserID { get; set; }
        public virtual Category Category { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}