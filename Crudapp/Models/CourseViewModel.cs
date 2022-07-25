using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Crudapp.Models
{
    public class CourseView
    {
        [Key]
        public int CId { get; set; }
        [Remote(action: "CheckCCodeAvailable", controller: "Course", ErrorMessage = "CCode Already Available")]
        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$"), System.ComponentModel.DataAnnotations.StringLength(10),]
        public string CCode { get; set; }
       
        [Required]
        public string CName { get; set; }

        [ForeignKey("Department")]
        public int DepId { get; set; }

        public DepartmentView Department { get; set; }

    }
}
