using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace homework.Domain.Models
{
    public class Ticket : BaseEntity
    {
        public string UserId { get; set; }
        public string ScreaningId { get; set; }

        public User User { get; set; }

        public Screaning Screaning { get; set; }
    }
}
