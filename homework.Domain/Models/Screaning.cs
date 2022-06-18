using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace homework.Domain.Models
{
    public class Screaning : BaseEntity
    {

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public int MaxTickets { get; set; }

        [Required]
        public double Price { get; set; }

        public string MovieId { get; set; }

        public Movie Movie { get; set; }

        public virtual List<Ticket> Tickets { get; set; }

    }
}
