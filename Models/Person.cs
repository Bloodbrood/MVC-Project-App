using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectApp.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        

        [Required(ErrorMessage = "Zadání jména je povinné.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Křestní jméno musí mít minimálně 2 a maximálně 50 znaků.")]
        [RegularExpression(@"^[\p{L}\s]+$", ErrorMessage = "Jméno může obsahovat pouze písmena, české diakritické znaky, mezery a pomlčky")]
        [Display(Name = "Jméno")]
        public string Jmeno { get; set; } = " ";

        [Required(ErrorMessage = "Zadání Příjmení je povinné")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Příjmení musí mít minimálně 2 a maximálně 50 znaků.")]
        [RegularExpression(@"^[\p{L}\s]+$", ErrorMessage = "Příjmení nesmí obsahovat čísla ani speciální znaky.")]
        [Display(Name = "Příjmení")]
        public string Prijmeni { get; set; } = " ";

        [Required(ErrorMessage = "Telefonní číslo je povinné.")]
        [RegularExpression(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "Telefonní číslo není ve správném formátu.")]
        [Display(Name = "Telefonní číslo")]
        public string TelefoniCislo { get; set; } = " ";

        public string Email { get; set; } = " "; // cizí klíč identity User

        [RegularExpression(@"^[\p{L}\s]{2,100}$", ErrorMessage = "Město může obsahovat pouze písmena, číslice a mezery.")]
        [Display(Name = "Město")]
        public string Mesto { get; set; } = " ";

        [RegularExpression(@"^[\p{L}\s]{2,100}$", ErrorMessage = "Ulice může obsahovat pouze písmena, čísla a speciální znaky (.,-/), a musí mít délku 2 až 100 znaků.")]
        public string Ulice { get; set; } = " ";

        [Range(1, int.MaxValue, ErrorMessage = "Číslo popisné musí být kladné celé číslo.")]
        [Display(Name = "Číslo Popisné")]
        public int CisloPopisne { get; set; }

        [RegularExpression(@"^\d{3}\s?\d{2}$", ErrorMessage = "PSČ musí být ve formátu '123 45'.")]
        [Display(Name = "PSČ")]
        public string PSC { get; set; }

        public virtual IdentityUser? IdentityUser { get; set; }
        [ForeignKey("IdentityUser")]
        public string? IdentityUserId { get; set; }

        public virtual ICollection<Insurance>? Insurances { get; set; }

    }
}
