using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeHolidayManagement.Models
{
    public class LeaveTypeVM
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        [Range(1,130, ErrorMessage ="Please Enter Valid Number")]
        [Display(Name ="Default Number Of Days")]
        public string DefaultDays { get; set; }

        [Display(Name="Date Created")]
        public DateTime? DateCreated { get; set; }
    }

}
