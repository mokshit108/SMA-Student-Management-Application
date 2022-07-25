using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Crudapp.Models
{
    public class DepartmentView
    {
        [Key]
        public int DepId { get; set; }
        [Remote(action: "CheckDepCodeAvailable", controller: "Department", ErrorMessage = "DepCode Already Available")]
        [System.ComponentModel.DataAnnotations.Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$"), System.ComponentModel.DataAnnotations.StringLength(7)]
        public string DepCode { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string DName { get; set; }
    }
}
