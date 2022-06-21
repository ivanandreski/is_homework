using homework.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace homework.Service.Interface
{
    public interface IScreaningService
    {
        List<Screaning> FindAll();
        Screaning FindById(Guid? id);
        void Create(Screaning screaning);
        void Update(Screaning screaning);
        void Delete(Guid id);
    }
}
