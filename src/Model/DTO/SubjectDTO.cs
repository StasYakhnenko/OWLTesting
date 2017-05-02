using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class SubjectDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Describtion { get; set; }
		public List<TestDTO> Tests { get; set; }
	}
}
