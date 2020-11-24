using AutoMapper;
using EmployeeHolidayManagement.Contracts;
using EmployeeHolidayManagement.Data;
using EmployeeHolidayManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeHolidayManagement.Controllers
{
    [Authorize]
    public class LeaveRequestController : Controller
    {
        private readonly ILeaveRequestRepository _leaveRequestRepo;
        private readonly ILeaveAllocationRepository _leaveAllocationRepo;
        private readonly ILeaveTypeRepository _leaveTypeRepo;
        private readonly UserManager<Employee> _userManager;
        private readonly IMapper _mapper;

        public LeaveRequestController(ILeaveRequestRepository leaveRequestRepo, ILeaveAllocationRepository leaveAllocationRepo, ILeaveTypeRepository leaveTypeRepo,
            UserManager<Employee> userManager,
            IMapper mapper)
        {
            _leaveAllocationRepo = leaveAllocationRepo;
            _leaveRequestRepo = leaveRequestRepo;
            _leaveTypeRepo = leaveTypeRepo;
            _userManager = userManager;
            _mapper = mapper;
        }
        [Authorize(Roles="Administrator")]
        // GET: LeaveRequestController
        public ActionResult Index()
        {
            var leaveRequests = _leaveRequestRepo.FindAll();
            var leaveRequestVm = _mapper.Map<List<LeaveRequestVm>>(leaveRequests);
            var model = new AdminLeaveRequestsViewMV
            {
                TotalRequests = leaveRequestVm.Count,
                ApprovedRequests = leaveRequestVm.Where(q => q.Approved == true).Count(),
                PendingRequests = leaveRequestVm.Count(q => q.Approved == null),
                RejectedRequests = leaveRequestVm.Count(q => q.Approved == false),
                LeaveRequests = leaveRequestVm

            };
            
            return View(model);
        }

        // GET: LeaveRequestController/Details/5
        public ActionResult Details(int id)
        {
            var obj = _leaveRequestRepo.FindById(id);
            var objvm = _mapper.Map<LeaveRequestVm>(obj);
            return View(objvm);
        }

        public ActionResult ApproveRequest(int id)
        {
            try
            {
                var leaveRequestObj = _leaveRequestRepo.FindById(id);
                leaveRequestObj.Approved = true;
                leaveRequestObj.ApprovedById = _userManager.GetUserId(User);
                leaveRequestObj.DateActioned = DateTime.Now;

                var leaveAllocationObj = _leaveAllocationRepo.GetLeaveAllocationsByEmployeeAndType(leaveRequestObj.RequestingEmployeeId, leaveRequestObj.LeaveTypeId);
                int daysRequested = (int)(leaveRequestObj.EndDate - leaveRequestObj.StartDate).TotalDays;
                leaveAllocationObj.NumberOfDays -= daysRequested;

                _leaveRequestRepo.Update(leaveRequestObj);
                _leaveAllocationRepo.Update(leaveAllocationObj);
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));

            }


        }

        public ActionResult RejectRequest(int id)
        {
            try
            {
                var obj = _leaveRequestRepo.FindById(id);
                obj.Approved = false;
                obj.ApprovedById = _userManager.GetUserId(User);
                obj.DateActioned = DateTime.Now;
                _leaveRequestRepo.Update(obj);
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));

            }

        }

        // GET: LeaveRequestController/Create
        public ActionResult Create()
        {
            var leaveTypes = _leaveTypeRepo.FindAll();
            var leaveTypeItems = leaveTypes.Select(q => new SelectListItem
            {
                Text = q.Name,
                Value = q.Id.ToString()
            });
            var model = new CreateLeaveRequestVM
            {
                LeaveTypes = leaveTypeItems
            };
            return View(model);
        }

        // POST: LeaveRequestController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateLeaveRequestVM model)
        {
            try
            {
                var startDate = Convert.ToDateTime(model.StartDate);
                var endDate = Convert.ToDateTime(model.EndDate);
                var leaveTypes = _leaveTypeRepo.FindAll();
                var leaveTypesItems = leaveTypes.Select(q => new SelectListItem
                {
                    Text = q.Name,
                    Value = q.Id.ToString()
                });

                model.LeaveTypes = leaveTypesItems;
                if (!ModelState.IsValid)
                {
                    
                    return View(model);
                }

                if (DateTime.Compare(startDate, endDate) > 0)
                {
                    ModelState.AddModelError("", "Start date can not be further in the future than the end date");
                    return View(model);

                }
                

                var employee = _userManager.GetUserAsync(User).Result;
                var allocation = _leaveAllocationRepo.GetLeaveAllocationsByEmployeeAndType(employee.Id, model.LeaveTypeId);
                int daysRequested = (int)(endDate - startDate).TotalDays;
                
                if(daysRequested > allocation.NumberOfDays)
                {
                    ModelState.AddModelError("", "Sorry, You have requested more days than you have left");
                    return View(model);
                }

                var leaveRequest= new LeaveRequestVm
                {
                DateActioned = DateTime.Now,
                RequestingEmployeeId = employee.Id,
                StartDate=startDate,
                EndDate=endDate,
                Approved=null,
                DateRequested = DateTime.Now,
                LeaveTypeId=model.LeaveTypeId,
                RequestComments=model.RequestComments
            };
                var obj= _mapper.Map<LeaveRequest>(leaveRequest);
                

                var success = _leaveRequestRepo.Create(obj);
                if (!success)
                {
                    ModelState.AddModelError("", "Something went wrong when applying for leave");
                    return View(model);
                }
                

                return RedirectToAction(nameof(EmployeeIndex));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Something went wrong when appliying for leave");
                return View(model);
            }
        }

        public IActionResult EmployeeIndex()
        {
            var employeeId = _userManager.GetUserId(User);
            var leaveRequestObj = _leaveRequestRepo.GetLeaveRequestsByEmployee(employeeId);
            var leaveAllocationobj = _leaveAllocationRepo.GetLeaveAllocationsByEmployee(employeeId);
            var leaveAllocationsVm = _mapper.Map<List<LeaveAllocationVM>>(leaveAllocationobj);
            var leaveRequestVm = _mapper.Map<List<LeaveRequestVm>>(leaveRequestObj);
            var model = new EmployeeLeaveRequestsVM
            {
                LeaveRequests=leaveRequestVm,
                LeaveAllocations = leaveAllocationsVm

            };
            return View(model);

        }

        public IActionResult CancelRequest(int id)
        {
            string employeeId = _userManager.GetUserId(User);

            var leaveRequest = _leaveRequestRepo.FindById(id);
            var startDayOfLeave = leaveRequest.StartDate;
            var endDayOfLeave = leaveRequest.EndDate;
            int totalNumberOfDays = (int)(endDayOfLeave - startDayOfLeave).TotalDays;
            int leaveRequestId = leaveRequest.LeaveTypeId;

            var leaveAllocationForEmployee = _leaveAllocationRepo.GetLeaveAllocationsByEmployeeAndType(employeeId, leaveRequestId);
            leaveAllocationForEmployee.NumberOfDays += totalNumberOfDays;
            var updatedAllocationForUser = _leaveAllocationRepo.Update(leaveAllocationForEmployee);

            var requestDeleted = _leaveRequestRepo.Delete(leaveRequest);

            if (!requestDeleted)
            {
                ModelState.AddModelError("", "Something Went wrong when cancelling request");
                return RedirectToAction(nameof(EmployeeIndex));
            }

            return RedirectToAction(nameof(EmployeeIndex));


        }

        // GET: LeaveRequestController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LeaveRequestController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: LeaveRequestController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaveRequestController/Delete/5
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
