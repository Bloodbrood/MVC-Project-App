using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectApp.Models
{
    public class Insurance
    {

        public enum TypPojisteni
        {
            Majetek,
            Osoba
        }

            [Key]
            public int Id { get; set; } //primární klíč
        [Display(Name = "Částka")]
        public decimal Castka { get; set; }
        
        [Display(Name = "Platnost-Od")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PlatnostOd { get; set; }

        [Display(Name = "Platnost-Do")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PlatnostDo { get; set; }

        public string PredmetPojisteni { get; set; }
            public TypPojisteni Typ { get; set; }
            public Person? Person { get; set; }

            [ForeignKey("Person")]
            public int? PersonId { get; set; } //cizí klíč
        

    }
}
