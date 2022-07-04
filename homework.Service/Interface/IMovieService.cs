using homework.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace homework.Service.Interface
{
    public interface IMovieService
    {
        List<Movie> FindAll();

        Movie FindById(Guid? Id);

        void Create(Movie entity);
        void Update(Movie entity);
        void Delete(Guid id);

        List<string> GetAllGenres();
    }
}
