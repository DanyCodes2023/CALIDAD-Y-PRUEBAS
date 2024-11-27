using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VENTA_COMPRA.Models
{
    public class Product
    {
        // Datos del Cliente
        [Key]
        public int IdProduct { get; set; }
        public int RoleID { get; set; }

        public int UsuarioID { get; set; }

        [MaxLength(100)]
        public string Nombres { get; set; } = "";

        [MaxLength(100)]
        public string Apellidos { get; set; } = "";

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [MaxLength(8, ErrorMessage = "El DNI no puede exceder los 8 caracteres.")]
        public string DNI { get; set; } = ""; 
        [MaxLength(15)]
        public string Telefono { get; set; } = "";

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debe ser una dirección de email válida.")]
        [MaxLength(100, ErrorMessage = "El email no puede exceder los 100 caracteres.")]
        public string Email { get; set; } = "";

        [MaxLength(100)]
        public string Departamento { get; set; } = "";

        [MaxLength(100)]
        public string Provincia { get; set; } = "";

        [MaxLength(100)]
        public string Distrito { get; set; } = "";

        // Datos del Vehículo
        [MaxLength(100)]
        public string NombreAuto { get; set; } = "";

        [Precision(16, 2)]
        public decimal Precio { get; set; } // Precio del vehículo

        public int Kilometros { get; set; } // Kilometraje del vehículo

        [MaxLength(50)]
        public string Combustible { get; set; } = ""; // Tipo de combustible

        [MaxLength(100)]
        public string Motor { get; set; } = ""; // Motor del vehículo

        [MaxLength(50)]
        public string Color { get; set; } = ""; // Color del vehículo

        public int AñoModelo { get; set; } // Año del modelo del vehículo

        [MaxLength(50)]
        public string Transmision { get; set; } = ""; // Transmisión del vehículo (Automática, Manual, etc.)

        [MaxLength(50)]
        public string Tipo { get; set; } = ""; // Tipo de vehículo (SUV, Sedán, etc.)

        public int Puertas { get; set; } // Número de puertas del vehículo

        public string Descripcion { get; set; } = ""; // Descripción adicional del vehículo

        // Fecha de Venta
        public DateTime FechaVenta { get; set; } // Fecha en la que se realizó la venta

        // Otros campos del vehículo previamente existentes
        [MaxLength(100)]
        public string ImageFileName { get; set; } = ""; // Nombre del archivo de imagen del vehículo

        public DateTime CreateAt { get; set; }  // Fecha de creación del registro

        public int? Estado { get; set; }

        [ForeignKey("UsuarioID")]
        public Usuario? NavegadorPersona { get; set; }
    }
}
