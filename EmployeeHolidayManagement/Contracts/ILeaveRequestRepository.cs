﻿using EmployeeHolidayManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeHolidayManagement.Contracts
{
    public interface ILeaveRequestRepository :IRepositoryBase<LeaveRequest>
    {
        Task<ICollection<LeaveRequest>> GetLeaveRequestsByEmployee(string employeeId);
    }
}
