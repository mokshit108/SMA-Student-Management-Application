using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Crudapp.Models
{
    public class StudentView
    {
        [Key]
        public int StudentId { get; set; }
        [ForeignKey("Department")]
        public int DepId { get; set; }

        public DepartmentView Department { get; set; }

        [ForeignKey("Course")]
        public int CId { get; set; }

        public CourseView Course { get; set; }


        [Required]
        [RegularExpression(@"^[A-Z]+[a-z0-9""'\s-]*$"), StringLength(6)]
        [Remote(action: "CheckSRollnoAvailable", controller: "Student", ErrorMessage = "Roll no Already Available")]
        public string SRollno { get; set; }
        [Required]
        [StringLength(20, ErrorMessage ="Name not be exceed")]
        public string SName { get; set; }

        
        [Required]
        [Range(18, 30, ErrorMessage = "Should be greated than or equal to 18")]
        public int Age { get; set; }
        
    }
}
