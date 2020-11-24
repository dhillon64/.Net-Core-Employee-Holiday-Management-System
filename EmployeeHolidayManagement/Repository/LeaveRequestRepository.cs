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

        public bool Create(LeaveRequest entity)
        {
            _db.LeaveRequests.Add(entity);
            return Save();
        }

        public bool Delete(LeaveRequest entity)
        {
            _db.LeaveRequests.Remove(entity);
            return Save();
        }

        public bool Exists(int id)
        {
            return _db.LeaveRequests.Any(a => a.Id == id);
        }

        public bool Exists(string name)
        {
            throw new NotImplementedException();
        }

        public ICollection<LeaveRequest> FindAll()
        {
            return _db.LeaveRequests.Include(a=>a.RequestingEmployee).Include(a=>a.ApprovedBy).Include(a=>a.LeaveType).ToList();
        }

        public LeaveRequest FindById(int Id)
        {
            return _db.LeaveRequests.Include(a => a.RequestingEmployee).Include(a => a.ApprovedBy).Include(a => a.LeaveType).FirstOrDefault(a=>a.Id==Id);
        }

        public ICollection<LeaveRequest> GetLeaveRequestsByEmployee(string employeeId)
        {
            return FindAll().Where(q => q.RequestingEmployeeId == employeeId).ToList();
        }

        public bool Save()
        {
           return _db.SaveChanges() > 0 ? true : false;
        }

        public bool Update(LeaveRequest entity)
        {
            _db.LeaveRequests.Update(entity);
            return Save();
        }
    }
}
