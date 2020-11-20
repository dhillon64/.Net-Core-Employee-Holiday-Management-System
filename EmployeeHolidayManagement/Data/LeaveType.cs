using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeHolidayManagement.Data
{
    public class LeaveType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int DefaultDays { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
