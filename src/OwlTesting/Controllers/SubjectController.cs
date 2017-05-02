using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BAL.Interface;
using Model.DTO;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OwlTesting.Controllers
{
    public class SubjectController : Controller
    {
        private readonly ISubjectManager subjectManager;
        // GET: /<controller>/
        public SubjectController(ISubjectManager subjectManager)
        {
            this.subjectManager = subjectManager;
        }
		[Authorize(Roles = "Administrator")]
		public IActionResult Index()
        {
            var subjects = subjectManager.GetAll();
            return View(subjects);
        }

        [HttpGet]
		[Authorize(Roles = "Administrator")]
		public IActionResult AddSubject()
        {
            var model = new SubjectDTO();
            
            return PartialView("_AddSubject", model);
        }

        [HttpPost]
		[Authorize(Roles = "Administrator")]
		public IActionResult AddSubject(SubjectDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    subjectManager.AddSubject(model);
                    return View("Index");
                }
                catch(Exception ex)
                {

                }
            }
            return View("Error");
        }

        [HttpGet]
		[Authorize(Roles = "Administrator")]
		public IActionResult EditSubject(int id)
        {
            var model = subjectManager.GetById(id);
            if (model == null)
            {
                return PartialView("_AddSubject");
            }
            return PartialView("_EditSubject", model);
        }

        [HttpPost]
		[Authorize(Roles = "Administrator")]
		public IActionResult EditSubject(SubjectDTO subject)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    subjectManager.UpdateSubject(subject);
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    return View("Error");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
		[Authorize(Roles = "Administrator")]
		public IActionResult DeleteSubject(int id)
        {
            SubjectDTO subject = null;
            if (id != 0)
            {
                subject = subjectManager.GetById(id);
            }
            return PartialView("_DeleteSubject", subject);
        }

        [HttpPost]
		[Authorize(Roles = "Administrator")]
		public IActionResult DeleteSubjectPost(int id)
        {
            if (id != 0)
            {
                subjectManager.DeleteSubject(id);
                return RedirectToAction("Index");
            }
            return View("Error");
        }
    }
}
