using homework.Domain.Models;
using homework.Repository.Interface;
using homework.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace homework.Service.Implementation
{
    public class ScreaningService : IScreaningService
    {
        private readonly IScreaningRepository _screaningRepository;
        private readonly IRepository<Movie> _movieRepository;

        public ScreaningService(IScreaningRepository screaningRepository, IRepository<Movie> movieRepository)
        {
            _screaningRepository = screaningRepository;
            _movieRepository = movieRepository;
        }

        public void Create(Screaning screaning)
        {
            screaning.Id = Guid.NewGuid();
            Movie movie = _movieRepository.Get(screaning.MovieId);
            screaning.Movie = movie;

            _screaningRepository.Insert(screaning);
        }

        public void Delete(Guid id)
        {
            var screaning = _screaningRepository.Get(id);
            _screaningRepository.Delete(screaning);
        }

        public List<Screaning> FindAll()
        {
            return _screaningRepository.GetAll().ToList();
        }

        public Screaning FindById(Guid? id)
        {
            return _screaningRepository.Get(id.Value);
        }

        public void Update(Screaning screaning)
        {
            _screaningRepository.Update(screaning);
        }
    }
}
