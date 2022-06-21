using homework.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace homework.Repository.Interface
{
    public interface IScreaningRepository
    {
        List<Screaning> GetAll();
        Screaning Get(Guid? id);
        void Insert(Screaning entity);
        void Update(Screaning entity);
        void Delete(Screaning entity);
    }
}
