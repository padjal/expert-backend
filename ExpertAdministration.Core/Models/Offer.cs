using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpertAdministration.Core.Models
{
    public class Offer
    {
        [Required]
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt{ get; set; }

        public string Description{ get; set; }

        public List<string> ImageUrls{ get; set; }

        public string Owner{ get; set; }

        public double Price{ get; set; }

        public string Status{ get; set; }

        public List<string> Categories { get; set; }
    }
}
