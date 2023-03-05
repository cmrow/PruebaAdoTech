using System.ComponentModel.DataAnnotations;

namespace PruebaAdoTech.Modelos
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Número de identificación es un campo requerido")]
        public string NumeroIdentificacion { get; set; }

        [Required(ErrorMessage = "Nombres es un campo requerido")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Apellidos es un campo requerido")]
        public string Apellidos { get; set; }

        public string Genero { get; set; }
    }
}
