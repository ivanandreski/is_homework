using homework.Domain.Models;
using homework.Repository.Implementation;
using homework.Repository.Interface;
using homework.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace homework.Service.Implementation
{
    public class MovieService : IMovieService
    {

        private readonly IRepository<Movie> _movieRepository;

        public MovieService(IRepository<Movie> movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public void Create(Movie entity)
        {
            entity.Id = Guid.NewGuid();
            _movieRepository.Insert(entity);
        }

        public void Delete(Guid id)
        {
            Movie movie = _movieRepository.Get(id);
            _movieRepository.Delete(movie);
        }

        public List<Movie> FindAll()
        {
            return _movieRepository.GetAll().ToList();
        }

        public Movie FindById(Guid? Id)
        {
            if (Id == null) return null;

            return _movieRepository.Get(Id);
        }

        public void Update(Movie entity)
        {
            _movieRepository.Update(entity);
        }
    }
}
