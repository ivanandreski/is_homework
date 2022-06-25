using homework.Domain.Models;
using homework.Repository.Interface;
using homework.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace homework.Service.Implementation
{
    public class TicketService : ITicketService
    {

        private readonly ITicketRepository _ticketRepository;
        private readonly IRepository<Movie> _movieRepository;
        private readonly IScreaningRepository _screaningRepository;

        public TicketService(ITicketRepository ticketRepository, IRepository<Movie> movieRepository, IScreaningRepository screaningRepository)
        {
            _ticketRepository = ticketRepository;
            _movieRepository = movieRepository;
            _screaningRepository = screaningRepository;
        }

        public List<Ticket> GetAllValidFromUser(string userId)
        {
            List<Ticket> temp =  _ticketRepository.GetAll()
                .Where(t => t.UserId.Equals(userId))
                .Where(t => t.Screaning.Date >= DateTime.Now)
                .ToList();

            foreach(var ticket in temp)
            {
                var movieName = _screaningRepository.Get(ticket.ScreaningId).Movie.Name;
                ticket.Movie = movieName;
            }

            return temp;
        }
    }
}
