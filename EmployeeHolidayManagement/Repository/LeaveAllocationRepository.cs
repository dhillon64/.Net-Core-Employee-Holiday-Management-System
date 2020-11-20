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

        public bool CheckAllocation(int leaveTypeId, string employeeId)
        {
            var period = DateTime.Now.Year;
            return FindAll().Where(a => a.EmployeeId == employeeId && a.LeaveTypeId == leaveTypeId && a.Period == period).Any();
        }

        public bool Create(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Add(entity);
            return Save();
        }

        public bool Delete(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Remove(entity);
            return Save();
        }

        public bool Exists(int id)
        {
            return _db.LeaveAllocations.Any(a => a.Id == id);
        }

        public bool Exists(string name)
        {
            throw new NotImplementedException();
        }

        public ICollection<LeaveAllocation> FindAll()
        {
            return _db.LeaveAllocations.Include(a => a.LeaveType).ToList();
        }

        public LeaveAllocation FindById(int Id)
        {
            return _db.LeaveAllocations.Include(a=>a.Employee).Include(a=>a.LeaveType).FirstOrDefault(a => a.Id == Id);
        }

        public ICollection<LeaveAllocation> GetLeaveAllocationsByEmployee(string id)
        {
            var period = DateTime.Now.Year;
            return FindAll().Where(a => a.EmployeeId == id && a.Period==period).ToList();
        }

        public bool Save()
        {
            return _db.SaveChanges() > 0 ? true : false;
        }

        public bool Update(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Update(entity);
            return Save();
        }
    }
}
