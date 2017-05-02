using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAL.Manager
{
    public class BaseManager
    {
        protected IUnitOfWork uOw;

        public BaseManager(IUnitOfWork uOw)
        {
            this.uOw = uOw;
        }
    }
}
