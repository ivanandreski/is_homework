using homework.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace homework.Service.Interface
{
    public interface IScreaningService
    {
        List<Screaning> FindAll();

        List<Screaning> FindAllFiltered(DateTime date);
        Screaning FindById(Guid? id);

        int FindAvailableTicketsForScreaning(Guid id);
        void Create(Screaning screaning);
        void Update(Screaning screaning);
        void Delete(Guid id);
    }
}
