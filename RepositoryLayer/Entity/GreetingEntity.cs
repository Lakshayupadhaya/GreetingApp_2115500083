using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class GreetingEntity
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Greeting { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        public override string ToString()
        {
            return $"ID : {Id} Greeting : {Greeting}";
        }
    }
}
