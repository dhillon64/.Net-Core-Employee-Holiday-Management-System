using EmployeeHolidayManagement.Contracts;
using EmployeeHolidayManagement.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeHolidayManagement.Repository
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveRequestRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(LeaveRequest entity)
        {
            await _db.LeaveRequests.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(LeaveRequest entity)
        {
            _db.LeaveRequests.Remove(entity);
            return await Save();
        }

        public async Task<bool> Exists(int id)
        {
            return await _db.LeaveRequests.AnyAsync(a => a.Id == id);
        }


        public async Task<ICollection<LeaveRequest>> FindAll()
        {
            return await _db.LeaveRequests.Include(a=>a.RequestingEmployee).Include(a=>a.ApprovedBy).Include(a=>a.LeaveType).ToListAsync();
        }

        public async Task<LeaveRequest> FindById(int Id)
        {
            return await _db.LeaveRequests.Include(a => a.RequestingEmployee).Include(a => a.ApprovedBy).Include(a => a.LeaveType).FirstOrDefaultAsync(a=>a.Id==Id);
        }

        public async Task<ICollection<LeaveRequest>> GetLeaveRequestsByEmployee(string employeeId)
        {
            var leaveRequests = await FindAll();
            return leaveRequests.Where(q => q.RequestingEmployeeId == employeeId).ToList();
        }

        public async Task<bool> Save()
        {
           return await _db.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<bool> Update(LeaveRequest entity)
        {
            _db.LeaveRequests.Update(entity);
            return await Save();
        }
    }
}
