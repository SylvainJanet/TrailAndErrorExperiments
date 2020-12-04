using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Address : BaseEntity
    {
        [Required]
        [Key]
        [Column(Order = 1)]
        public int Number { get; set; }

        [Required]
        [MaxLength(200)]
        [Key]
        [Column(Order = 2)]
        public string Street { get; set; }

        public Address(int number, string street)
        {
            Number = number;
            Street = street;
        }

        public Address()
        {
        }

        public override bool Equals(object obj)
        {
            return obj is Address address &&
                   Number == address.Number &&
                   Street == address.Street;
        }

        public override int GetHashCode()
        {
            int hashCode = -945657314;
            hashCode = hashCode * -1521134295 + Number.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Street);
            return hashCode;
        }
    }
}