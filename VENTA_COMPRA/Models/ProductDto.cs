
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VENTA_COMPRA.Models
{
    public class ProductDto
    {
        public int Id { get; set; }

        [Required,MaxLength(100)]
        public string Nombres { get; set; } = "";

        [Required, MaxLength(100)]
        public string Apellidos { get; set; } = "";

        [Required, MaxLength(8, ErrorMessage = "El DNI no puede exceder los 8 caracteres.")]
        public string DNI { get; set; } = "";
        [Required, MaxLength(15)]
        public string Telefono { get; set; } = "";

       
        [Required, MaxLength(100, ErrorMessage = "El email no puede exceder los 100 caracteres.")]
        public string Email { get; set; } = "";

        [Required, MaxLength(100)]
        public string Departamento { get; set; } = "";

        [Required, MaxLength(100)]
        public string Provincia { get; set; } = "";

        [Required, MaxLength(100)]
        public string Distrito { get; set; } = "";

        // Datos del Vehículo
        [Required, MaxLength(100)]
        public string NombreAuto { get; set; } = "";
        [Required]
        public decimal Precio { get; set; } // Precio del vehículo
        [Required]
        public int Kilometros { get; set; } // Kilometraje del vehículo

        [Required, MaxLength(50)]
        public string Combustible { get; set; } = ""; // Tipo de combustible

        [Required, MaxLength(100)]
        public string Motor { get; set; } = ""; // Motor del vehículo

        [Required, MaxLength(50)]
        public string Color { get; set; } = ""; // Color del vehículo

        [Required]
        public int AñoModelo { get; set; } // Año del modelo del vehículo

        [Required, MaxLength(50)]
        public string Transmision { get; set; } = ""; // Transmisión del vehículo (Automática, Manual, etc.)

        [Required, MaxLength(50)]
        public string Tipo { get; set; } = ""; // Tipo de vehículo (SUV, Sedán, etc.)

        [Required]
        public int Puertas { get; set; } // Número de puertas del vehículo

        [Required]
        public string Descripcion { get; set; } = ""; // Descripción adicional del vehículo

        public IFormFile? ImageFileName { get; set; }
        public int? Estado { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioID { get; set; }
        public Usuario? Usuario { get; set; }
    }
}
