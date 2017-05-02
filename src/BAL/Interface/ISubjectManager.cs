using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAL.Interface
{
    public interface ISubjectManager
    {
        List<SubjectDTO> GetAll();

        int AddSubject(SubjectDTO subject);

        bool DeleteSubject(int id);

        bool UpdateSubject(SubjectDTO subject);

        SubjectDTO GetById(int id);
    }
}
