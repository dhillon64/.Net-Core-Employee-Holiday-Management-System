using AutoMapper;
using EmployeeHolidayManagement.Contracts;
using EmployeeHolidayManagement.Data;
using EmployeeHolidayManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeHolidayManagement.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class LeaveAllocationController : Controller
    {
        private readonly ILeaveTypeRepository _leaveTypeRepo;
        private readonly ILeaveAllocationRepository _leaveAllocationRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;

        public LeaveAllocationController(ILeaveTypeRepository leaveTypeRepo, ILeaveAllocationRepository leaveAllocationRepo, IMapper mappper, UserManager<Employee> userManager)
        {
            _leaveTypeRepo = leaveTypeRepo;
            _leaveAllocationRepo = leaveAllocationRepo;
            _mapper = mappper;
            _userManager = userManager;
        }


        // GET: LeaveAllocationController
        public ActionResult Index()
        {
            var objList = _leaveTypeRepo.FindAll();
            var mappedLeaveTypes = new List<LeaveTypeVM>();
            foreach (var obj in objList)
            {
                mappedLeaveTypes.Add(_mapper.Map<LeaveTypeVM>(obj));
            }
            var model = new CreateLeaveAllocationVM
            {
                LeaveTypes = mappedLeaveTypes,
                NumberUpdated = 0
            };

            return View(model);
        }

        public ActionResult SetLeave(int Id)
        {
            var leaveType = _leaveTypeRepo.FindById(Id);
            var employees = _userManager.GetUsersInRoleAsync("Employee").Result;
            foreach(var employee in employees)
            {
                if (_leaveAllocationRepo.CheckAllocation(Id, employee.Id))
                    continue;
           
                var allocation = new LeaveAllocationVM
                {
                    DateCreated=DateTime.Now,
                    EmployeeId=employee.Id,
                    LeaveTypeId=Id,
                    NumberOfDays=leaveType.DefaultDays,
                    Period=DateTime.Now.Year
                };
                var leaveAllocation = _mapper.Map<LeaveAllocation>(allocation);
                _leaveAllocationRepo.Create(leaveAllocation);
            }
            return RedirectToAction(nameof(Index));

        }

        public ActionResult ListEmployees()
        {
            var employees = _userManager.GetUsersInRoleAsync("Employee").Result;
            var employeesVm = new List<EmployeeVM>();
            foreach(var employee in employees)
            {
                employeesVm.Add(_mapper.Map<EmployeeVM>(employee));
            }
            return View(employeesVm);
        }

        // GET: LeaveAllocationController/Details/5
        public ActionResult Details(string id)
        {
            var employee = _mapper.Map<EmployeeVM>(_userManager.FindByIdAsync(id).Result);
            var allocations = _mapper.Map<List<LeaveAllocationVM>>(_leaveAllocationRepo.GetLeaveAllocationsByEmployee(id));
            var model = new ViewAllocationsVM
            {
                Employee = employee,
                LeaveAllocations = allocations
            };

            return View(model);
        }

        // GET: LeaveAllocationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaveAllocationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LeaveAllocationController/Edit/5
        public ActionResult Edit(int id)
        {
            var objVm = _mapper.Map<EditLeaveAllocationVM>(_leaveAllocationRepo.FindById(id));

            return View(objVm);
        }

        // POST: LeaveAllocationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditLeaveAllocationVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                var obj = _leaveAllocationRepo.FindById(model.Id);
                obj.NumberOfDays = model.NumberOfDays;
                var editSuccess = _leaveAllocationRepo.Update(obj);
                if (!editSuccess)
                {
                    ModelState.AddModelError("", "Error while updating");
                    return View(model);
                }
                return RedirectToAction(nameof(Details), new { id = model.EmployeeId });
            }
            catch
            {
                ModelState.AddModelError("", "Error while updating");
                return View();
            }
        }

        // GET: LeaveAllocationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaveAllocationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
