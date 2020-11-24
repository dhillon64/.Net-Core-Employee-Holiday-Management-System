using EmployeeHolidayManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeHolidayManagement.Contracts
{
    public interface ILeaveAllocationRepository : IRepositoryBase<LeaveAllocation>
    {
        bool CheckAllocation(int leaveTypeId, string employeeId);
        ICollection<LeaveAllocation> GetLeaveAllocationsByEmployee(string employeeId);

        LeaveAllocation GetLeaveAllocationsByEmployeeAndType(string employeeId, int leaveTypeId);
    }
}
