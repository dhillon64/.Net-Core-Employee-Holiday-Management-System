using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EmployeeHolidayManagement.Contracts;
using EmployeeHolidayManagement.Data;
using EmployeeHolidayManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeHolidayManagement.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class LeaveTypesController : Controller
    {
        
        private readonly ILeaveTypeRepository _repo;
        private readonly IMapper _mapper;

        public LeaveTypesController(ILeaveTypeRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
            
        }
        
        // GET: LeaveTypesController
        public async Task<ActionResult> Index()
        {
            var objList = await _repo.FindAll();
            var objVm = new List<LeaveTypeVM>();
            foreach(var obj in objList)
            {
                objVm.Add(_mapper.Map<LeaveTypeVM>(obj));
            }

            return View(objVm);
        }

        // GET: LeaveTypesController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var isExists = await _repo.Exists(id);
            if (!isExists)
            {
                return NotFound();
            }
            var obj = await _repo.FindById(id);
            var objVm = _mapper.Map<LeaveTypeVM>(obj);
            return View(objVm);
        }

        // GET: LeaveTypesController/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: LeaveTypesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(LeaveTypeVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);

                }
                var exists = await _repo.Exists(model.Name);
                if (exists)
                {
                    ModelState.AddModelError("", "This LeaveType has already been created");
                    return View(model);
                }
                var obj = _mapper.Map<LeaveType>(model);
                obj.DateCreated = DateTime.Now;
                var leaveTypeCreated = await _repo.Create(obj);
                if (!leaveTypeCreated)
                {
                    ModelState.AddModelError("", "Something went wrong....");
                    return View(model);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Something went wrong...");
                return View(model);
            }
        }

        // GET: LeaveTypesController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var exists = await _repo.Exists(id);
            if (!exists)
            {
                return NotFound();
            }
            var obj =await _repo.FindById(id);
            var objVm = _mapper.Map<LeaveTypeVM>(obj);

            return View(objVm);
        }

        // POST: LeaveTypesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(LeaveTypeVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                var obj = _mapper.Map<LeaveType>(model);
                var updateSuccess =await _repo.Update(obj);
                if (!updateSuccess)
                {
                    ModelState.AddModelError("", "Something went wrong");
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Something went wrong");
                return View();
            }
        }

        // GET: LeaveTypesController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            /*if (!_repo.Exists(id))
            {
                return NotFound();
            }
            var obj = _repo.FindById(id);
            var objVm = _mapper.Map<LeaveTypeVM>(obj);

            return View(objVm);*/

            var obj = await _repo.FindById(id);
            if (obj == null)
            {
                return NotFound();
            }

            var Deleted = await _repo.Delete(obj);
            if (!Deleted)
            {
                return BadRequest();
            }
            return RedirectToAction(nameof(Index));


        }

        // POST: LeaveTypesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id,LeaveTypeVM model)
        {
            try
            {
                var obj = await _repo.FindById(id);
                if (obj == null)
                {
                    return NotFound();
                }

                var Deleted=await _repo.Delete(obj);
                if (!Deleted)
                {
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                
                return View(model);
            }
        }
    }
}
