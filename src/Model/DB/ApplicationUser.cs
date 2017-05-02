using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Model.DB
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int? Group { get; set; }
        public virtual ICollection<TeacherTest> TeacherTests { get; set; }
        public virtual ICollection<TestResult> TestHistory { get; set; }
    }
}
