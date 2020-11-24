using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeHolidayManagement.Models
{
    public class LeaveRequestVm
    {
        public int Id { get; set; }

        [Display(Name = "Requesting Employee")]
        public EmployeeVM RequestingEmployee { get; set; }
        
        public string RequestingEmployeeId { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Leave Type")]
        public LeaveTypeVM LeaveType { get; set; }
        public int LeaveTypeId { get; set; }
        [Display(Name ="Date Requested")]
        public DateTime DateRequested { get; set; }
        [Display(Name ="Employee Comments")]
        public string RequestComments { get; set; }
        public DateTime DateActioned { get; set; }
        public bool? Approved { get; set; }

        public EmployeeVM ApprovedBy { get; set; }

        public string ApprovedById { get; set; }
    }

    public class AdminLeaveRequestsViewMV
    {
        [Display(Name ="Total Requests")]
        public int TotalRequests { get; set; }
        [Display(Name = "Approved Requests")]
        public int ApprovedRequests { get; set; }
        [Display(Name = "Pending Requests")]
        public int PendingRequests { get; set; }
        [Display(Name = "Rejected Requests")]
        public int RejectedRequests { get; set; }
        public List<LeaveRequestVm>  LeaveRequests { get; set; }

    }

    public class EmployeeLeaveRequestsVM
    {
        public List<LeaveRequestVm> LeaveRequests { get; set; }


        public List<LeaveAllocationVM> LeaveAllocations {get; set;}

    }

    public class CreateLeaveRequestVM
    {
        [Required]
        [Display(Name ="Start Date")]
        public string StartDate { get; set; }
        [Required]
        [Display(Name = "End Date")]
        public string EndDate { get; set; }
        [Required]
        [Display(Name ="Leave Type")]
        public IEnumerable<SelectListItem> LeaveTypes { get; set; }
        [Display(Name ="Leave Type")]
        public int LeaveTypeId { get; set; }
        [Display(Name ="Comments")]
        [MaxLength(300)]
        public string RequestComments { get; set; }

    }
}
