using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace homework.Domain.Models
{
    public class Movie : BaseEntity
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public string Genre { get; set; }

        public virtual List<Screaning> Screanings { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
