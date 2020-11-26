using EmployeeHolidayManagement.Contracts;
using EmployeeHolidayManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EmployeeHolidayManagement.Repository
{
    public class LeaveTypeRepository : ILeaveTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveTypeRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> Create(LeaveType entity)
        {
             await _db.LeaveTypes.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(LeaveType entity)
        {
            _db.LeaveTypes.Remove(entity);
            return await Save();
        }

        public async Task<bool> Exists(int id)
        {
            return await _db.LeaveTypes.AnyAsync(a => a.Id == id);
        }

        public async Task<bool> Exists(string name)
        {
            return await _db.LeaveTypes.AnyAsync(a => a.Name == name);
        }

        public async Task<ICollection<LeaveType>> FindAll()
        {
            return await _db.LeaveTypes.ToListAsync();
        }

        public async Task<LeaveType> FindById(int Id)
        {
            var obj =await _db.LeaveTypes.FirstOrDefaultAsync(a => a.Id == Id);
            return obj;
        }

        public ICollection<LeaveType> GetEmployeesByLeaveType(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Save()
        {
            return await _db.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<bool> Update(LeaveType entity)
        {
            _db.LeaveTypes.Update(entity);
            return await Save();
        }
    }
}
