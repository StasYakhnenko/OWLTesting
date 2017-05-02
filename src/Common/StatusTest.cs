using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Common
{
    public enum StatusTest : byte
    {
        [Display(Name = "Не завершений")]
        NotFinished = 1,
        [Display(Name = "Завершений")]
        Finished
    }
}
