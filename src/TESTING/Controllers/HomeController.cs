using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BAL.Interface;
using Model.DB;
using Model.DTO;

namespace TESTING.Controllers
{
    public class HomeController : Controller
    {
        private ISubjectManager subjectManager;
        public HomeController(ISubjectManager subjectManager)
        {
            this.subjectManager = subjectManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("api/subjects/all")]
        [HttpGet]
        public IActionResult GetAllSubjects()
        {
            List<SubjectDTO> subjects = subjectManager.GetAll();
            return Json(subjects);
        }

        [Route("api/subjects/add")]
        [HttpPost]
        public IActionResult AddSubject([FromBody]SubjectDTO subject)
        {
            return Json(new { id = subjectManager.AddSubject(subject) });
        }

        [Route("api/subjects/delete")]
        [HttpPost]
        public IActionResult DeleteSubject([FromBody]SubjectDTO subject)
        {
            return Json(new { result = subjectManager.DeleteSubject(subject.Id) });
        }

        [Route("api/subjects/update")]
        [HttpPost]
        public IActionResult UpdateSubject([FromBody]SubjectDTO subject)
        {
            return Json(new { result = subjectManager.UpdateSubject(subject) });
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
