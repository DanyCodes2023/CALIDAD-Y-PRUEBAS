using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VENTA_COMPRA.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioID { get; set; }

        public int RoleID { get; set; }

        [Required(ErrorMessage = "El Nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "El Nombre debe tener como máximo 100 dígitos")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "El Apellido es obligatorio")]
        [MaxLength(100, ErrorMessage = "El Apellido debe tener como máximo 100 dígitos")]
        public string Apellidos { get; set; }

        [EmailAddress]
        public string Correo { get; set; }

        public string Password { get; set; }

        [ForeignKey("RoleID")]
        public Rol? Rol { get; set; }
    }
}
