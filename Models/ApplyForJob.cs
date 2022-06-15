using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication3.Models;

namespace Job_Offers_Website.Models
{
    public class ApplyForJob
    {
        public int Id { get; set; }

       [DataType(DataType.MultilineText)]
        public string Message { get; set; }
        public string Cv { get; set; }
        public DateTime ApplyDate { get; set; }
        public int JobId { get; set; }
        public string UserId { get; set; }

        public virtual Job job { get; set; }
        public virtual ApplicationUser user  { get; set; }
    }
}