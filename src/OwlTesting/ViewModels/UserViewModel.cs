using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OwlTesting.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
		[Display(Name = "Нікнейм")]
		public string UserName { get; set; }
        [DataType(DataType.Password)]
		[Display(Name = "Пароль")]
		public string Password { get; set; }
        [Display(Name = "Підтвердіть пароль")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
		[Display(Name = "Ім'я")]
		public string FirstName { get; set; }
		[Display(Name = "Прізвище")]
		public string LastName { get; set; }
		[Display(Name = "Номер групи (якщо користувач - студент)")]
		public int? Group { get; set; }
		[Display(Name = "Електрона пошта")]
		public string Email { get; set; }
        public List<SelectListItem> ApplicationRoles { get; set; }
		[Display(Name = "Роль")]
		public string ApplicationRoleId { get; set; }
    }
}
