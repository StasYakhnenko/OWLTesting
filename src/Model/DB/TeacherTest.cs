using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model.DB
{
    public class TeacherTest
    {
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int TestId { get; set; }
        public virtual Test Test { get; set; }
    }
}
