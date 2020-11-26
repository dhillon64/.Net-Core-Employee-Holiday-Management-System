using EmployeeHolidayManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeHolidayManagement.Contracts
{
    public interface ILeaveAllocationRepository : IRepositoryBase<LeaveAllocation>
    {
        Task<bool> CheckAllocation(int leaveTypeId, string employeeId);
        Task<ICollection<LeaveAllocation>> GetLeaveAllocationsByEmployee(string employeeId);

        Task<LeaveAllocation> GetLeaveAllocationsByEmployeeAndType(string employeeId, int leaveTypeId);
    }
}
