using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Job_Offers_Website.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }
        [Display(Name = "Category Description")]
        [DataType(DataType.MultilineText)]
        public string CategoryDescription { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
    }
}