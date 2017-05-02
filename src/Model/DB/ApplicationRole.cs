using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model.DB
{
    public class ApplicationRole : IdentityRole
    {
        public string Describtion { get; set; }
    }
}
