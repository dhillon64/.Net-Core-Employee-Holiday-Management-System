using EmployeeHolidayManagement.Contracts;
using EmployeeHolidayManagement.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeHolidayManagement.Repository
{
    public class LeaveAllocationRepository : ILeaveAllocationRepository
    {

        private readonly ApplicationDbContext _db;

        public LeaveAllocationRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CheckAllocation(int leaveTypeId, string employeeId)
        {
            var period = DateTime.Now.Year;
            var allocations= await FindAll();


            return allocations.Where(a => a.EmployeeId == employeeId && a.LeaveTypeId == leaveTypeId && a.Period == period).Any();
        }

        public async Task<bool> Create(LeaveAllocation entity)
        {
            await _db.LeaveAllocations.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Remove(entity);
            return await Save();
        }

        public async Task<bool> Exists(int id)
        {
            return await _db.LeaveAllocations.AnyAsync(a => a.Id == id);
        }

        public bool Exists(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<LeaveAllocation>> FindAll()
        {
            return await _db.LeaveAllocations.Include(a => a.LeaveType).ToListAsync();
        }

        public async Task<LeaveAllocation> FindById(int Id)
        {
            return await _db.LeaveAllocations.Include(a=>a.Employee).Include(a=>a.LeaveType).FirstOrDefaultAsync(a => a.Id == Id);
        }

        public async Task<ICollection<LeaveAllocation>> GetLeaveAllocationsByEmployee(string employeeId)
        {
            var period = DateTime.Now.Year;
            var allocations = await FindAll();
            return allocations.Where(a => a.EmployeeId == employeeId && a.Period==period).ToList();
        }

        public async Task<LeaveAllocation> GetLeaveAllocationsByEmployeeAndType(string employeeId, int leaveTypeId)
        {
            var period = DateTime.Now.Year;
            var allocations = await FindAll();
            return allocations.FirstOrDefault(a => a.EmployeeId == employeeId && a.Period == period&& a.LeaveTypeId==leaveTypeId);
        }

        public async Task<bool> Save()
        {
            return await _db.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<bool> Update(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Update(entity);
            return await Save();
        }
    }
}
