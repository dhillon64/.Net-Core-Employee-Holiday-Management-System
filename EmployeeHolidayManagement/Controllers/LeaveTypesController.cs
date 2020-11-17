using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EmployeeHolidayManagement.Contracts;
using EmployeeHolidayManagement.Data;
using EmployeeHolidayManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeHolidayManagement.Controllers
{
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
        public ActionResult Index()
        {
            var objList = _repo.FindAll();
            var objVm = new List<LeaveTypeVM>();
            foreach(var obj in objList)
            {
                objVm.Add(_mapper.Map<LeaveTypeVM>(obj));
            }

            return View(objVm);
        }

        // GET: LeaveTypesController/Details/5
        public ActionResult Details(int id)
        {
            if (!_repo.Exists(id))
            {
                return NotFound();
            }
            var obj = _repo.FindById(id);
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
        public ActionResult Create(LeaveTypeVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);

                }
                
                if (_repo.Exists(model.Name))
                {
                    ModelState.AddModelError("", "This LeaveType has already been created");
                    return View(model);
                }
                var obj = _mapper.Map<LeaveType>(model);
                obj.DateCreated = DateTime.Now;
                var leaveTypeCreated = _repo.Create(obj);
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
        public ActionResult Edit(int id)
        {
            if (!_repo.Exists(id))
            {
                return NotFound();
            }
            var obj = _repo.FindById(id);
            var objVm = _mapper.Map<LeaveTypeVM>(obj);

            return View(objVm);
        }

        // POST: LeaveTypesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LeaveTypeVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                var obj = _mapper.Map<LeaveType>(model);
                var updateSuccess = _repo.Update(obj);
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
        public ActionResult Delete(int id)
        {
            /*if (!_repo.Exists(id))
            {
                return NotFound();
            }
            var obj = _repo.FindById(id);
            var objVm = _mapper.Map<LeaveTypeVM>(obj);

            return View(objVm);*/

            var obj = _repo.FindById(id);
            if (obj == null)
            {
                return NotFound();
            }

            var Deleted = _repo.Delete(obj);
            if (!Deleted)
            {
                return BadRequest();
            }
            return RedirectToAction(nameof(Index));


        }

        // POST: LeaveTypesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id,LeaveTypeVM model)
        {
            try
            {
                var obj = _repo.FindById(id);
                if (obj == null)
                {
                    return NotFound();
                }

                var Deleted=_repo.Delete(obj);
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
