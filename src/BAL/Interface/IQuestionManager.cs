using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAL.Interface
{
    public interface IQuestionManager
    {
        QuestionDTO GetByID(int id);
        void UpdateQuestion(QuestionDTO model);
    }
}
