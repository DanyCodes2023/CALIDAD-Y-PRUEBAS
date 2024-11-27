using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using VENTA_COMPRA.Data;
using VENTA_COMPRA.Models;



namespace VENTA_COMPRA.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment environment;

        public ProductsController(AppDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var products = context.Product.ToList();
            return View(products);

        }

        public IActionResult VerPerfil()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = context.Usuario.FirstOrDefault(u => u.UsuarioID.ToString()== userId);
            var products = context.Product.Where(p => p.UsuarioID.ToString() == userId).ToList();

            var model = new VerPerfilViewModel
            {
                Usuario = user,
                Productos = products
            };

            return View(model);
        }
        [HttpGet]
        public JsonResult ElCarroLePertenece(int productoId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Json(false); // Usuario no autenticado
            }

            var product = context.Product.FirstOrDefault(p => p.IdProduct == productoId);

            // Retorna true si el producto pertenece al usuario, false en caso contrario
            return Json(product != null && product.UsuarioID.ToString() == userId);
        }

        public IActionResult Buscar(string query, string ordenar)
        {
            var productos = context.Product.AsQueryable();

          
                if (!string.IsNullOrEmpty(query))
                {
                    productos = productos.Where(p =>
                        p.NombreAuto.Contains(query) ||
                        p.Combustible.Contains(query) ||
                        p.AñoModelo.ToString().Contains(query) ||
                        p.Color.Contains(query) ||
                        p.Transmision.Contains(query) ||
                        p.Tipo.Contains(query)||
                        p.AñoModelo.Equals(query)||
                        p.Motor.Equals(query)
                     
                    );
                }
            switch (ordenar)
            {
                case "añoAsc":
                    productos = productos.OrderBy(p => p.AñoModelo);
                    break;
                case "añoDesc":
                    productos = productos.OrderByDescending(p => p.AñoModelo);
                    break;
                case "precioAsc":
                    productos = productos.OrderBy(p => p.Precio);
                    break;
                case "precioDesc":
                    productos = productos.OrderByDescending(p => p.Precio);
                    break;
                default:
                    // Orden por defecto si no se selecciona nada
                    productos = productos.OrderBy(p => p.NombreAuto);
                    break;
            }
            if (!productos.Any())
                {
                    return RedirectToAction("Index", "Products");
                }
    
                 return View("Buscar", productos.ToList()); // Devuelve la vista "Buscar.cshtml"
        }
        public IActionResult Vender()
        {
            return View();
        }
        /*
        [HttpPost]
        public IActionResult Vender(ProductDto productDto)
        {
            if (productDto.ImageFileName==null)
            {
                ModelState.AddModelError("ImageFileName","The image file is required");
            }
            if (!ModelState.IsValid)
            {
                return View(productDto);
            }

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(productDto.ImageFileName!.FileName);

            string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                productDto.ImageFileName.CopyTo(stream);
            }
            Product product = new Product()
            {
                Nombres = productDto.Nombres,
                Apellidos = productDto.Apellidos,
                DNI = productDto.DNI,
                Telefono = productDto.Telefono,
                Email = productDto.Email,
                Departamento = productDto.Departamento,
                Provincia = productDto.Provincia,
                Distrito = productDto.Distrito,
                NombreAuto = productDto.NombreAuto,
                Precio = productDto.Precio,
                Kilometros = productDto.Kilometros,
                Combustible = productDto.Combustible,
                Motor = productDto.Motor,
                Color = productDto.Color,
                AñoModelo = productDto.AñoModelo,
                Transmision = productDto.Transmision,
                Tipo = productDto.Tipo,
                Puertas = productDto.Puertas,
                Descripcion = productDto.Descripcion,
                ImageFileName = newFileName,
                FechaVenta = DateTime.Now,
                CreateAt = DateTime.Now,
                UsuarioID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                Estado=1
            };

            context.Product.Add(product);
            context.SaveChanges();

            TempData["SuccessMessage"] = "Auto registrado con éxito.";

            return RedirectToAction("Index", "Products");
        }

        */

        [HttpPost]
        public IActionResult Vender(ProductDto productDto)
        {
            // Verificar si el archivo de imagen fue enviado
            if (productDto.ImageFileName == null || productDto.ImageFileName.Length == 0)
            {
                ModelState.AddModelError("ImageFileName", "An image file must be selected.");
            }

            // Validaciones de otros campos
            if (string.IsNullOrEmpty(productDto.NombreAuto))
            {
                ModelState.AddModelError("NombreAuto", "The car name is required.");
            }

            if (productDto.Precio <= 0)
            {
                ModelState.AddModelError("Precio", "The price must be a positive number.");
            }

            if (productDto.Kilometros < 0)
            {
                ModelState.AddModelError("Kilometros", "The kilometers cannot be negative.");
            }

            // Validar el ID de usuario
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                ModelState.AddModelError("UserId", "Invalid user identifier.");
                return View(productDto); // Regresar la vista con el error
            }

            // Verificar si el modelo es válido antes de proceder
            if (!ModelState.IsValid)
            {
                return View(productDto);
            }

            // Guardar la imagen
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(productDto.ImageFileName.FileName);
            string imageFullPath = Path.Combine(environment.WebRootPath, "products", newFileName);

            using (var stream = System.IO.File.Create(imageFullPath))
            {
                productDto.ImageFileName.CopyTo(stream);
            }

            // Crear el nuevo producto
            Product product = new Product()
            {
                Nombres = productDto.Nombres,
                Apellidos = productDto.Apellidos,
                DNI = productDto.DNI,
                Telefono = productDto.Telefono,
                Email = productDto.Email,
                Departamento = productDto.Departamento,
                Provincia = productDto.Provincia,
                Distrito = productDto.Distrito,
                NombreAuto = productDto.NombreAuto,
                Precio = productDto.Precio,
                Kilometros = productDto.Kilometros,
                Combustible = productDto.Combustible,
                Motor = productDto.Motor,
                Color = productDto.Color,
                AñoModelo = productDto.AñoModelo,
                Transmision = productDto.Transmision,
                Tipo = productDto.Tipo,
                Puertas = productDto.Puertas,
                Descripcion = productDto.Descripcion,
                ImageFileName = newFileName,
                FechaVenta = DateTime.Now,
                CreateAt = DateTime.Now,
                UsuarioID = userId, // Usar el ID de usuario válido
                Estado = 1
            };

            // Agregar el producto a la base de datos
            context.Product.Add(product);
            context.SaveChanges();

            TempData["SuccessMessage"] = "Auto registrado con éxito.";

            return RedirectToAction("Index", "Products");
        }



        public IActionResult Detalle(int id)
        {
            var product = context.Product.FirstOrDefault(p => p.IdProduct == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email,string dni) {

            ViewBag.Email = email;
            ViewBag.DNI = dni;

            // Verificar si el usuario existe en la base de datos
            var user = context.Product.FirstOrDefault(u => u.Email == email && u.DNI == dni);

            if (user == null)
            {
                // Enviar mensaje de error si los datos no existen
                ViewBag.ErrorMessage = "Usuario o contraseña incorrectos.";
                return View();
            }

            // Enviar mensaje de éxito si el usuario es encontrado
            ViewBag.SuccessMessage = "¡Usuario logueado correctamente!";
            return RedirectToAction("Index", "Home"); // Redirigir a la acción deseada
        }
        [HttpPost]
        public IActionResult Logout()
        {
            // Eliminar datos del usuario de la sesión
            HttpContext.Session.Remove("UserEmail");

            // Redirigir a la página de login
            return RedirectToAction("Login");
        }


        // Acción GET para mostrar el formulario de edición
        // GET: Products/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = context.Product.FirstOrDefault(p => p.IdProduct == id);
            if (product == null)
            {
                return NotFound();
            }

            var productDto = new ProductDto
            {
                Nombres = product.Nombres,
                Apellidos = product.Apellidos,
                DNI = product.DNI,
                Telefono = product.Telefono,
                Email = product.Email,
                Departamento = product.Departamento,
                Provincia = product.Provincia,
                Distrito = product.Distrito,
                NombreAuto = product.NombreAuto,
                Precio = product.Precio,
                Kilometros = product.Kilometros,
                Combustible = product.Combustible,
                Motor = product.Motor,
                Color = product.Color,
                AñoModelo = product.AñoModelo,
                Transmision = product.Transmision,
                Tipo = product.Tipo,
                Puertas = product.Puertas,
                Descripcion = product.Descripcion,
                Estado= product.Estado,
            };

            return View(productDto);
        }

        // POST: Products/Edit/5
        [HttpPost]
        public IActionResult Edit(int id, ProductDto productDto)
        {
            if (id != productDto.Id) // Asegúrate de que el id coincide
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(productDto);
            }

            var product = context.Product.FirstOrDefault(p => p.IdProduct == id);
            if (product == null)
            {
                return NotFound();
            }

            // Actualiza los campos del producto
            product.Nombres = productDto.Nombres;
            product.Apellidos = productDto.Apellidos;
            product.DNI = productDto.DNI;
            product.Telefono = productDto.Telefono;
            product.Email = productDto.Email;
            product.Departamento = productDto.Departamento;
            product.Provincia = productDto.Provincia;
            product.Distrito = productDto.Distrito;
            product.NombreAuto = productDto.NombreAuto;
            product.Precio = productDto.Precio;
            product.Kilometros = productDto.Kilometros;
            product.Combustible = productDto.Combustible;
            product.Motor = productDto.Motor;
            product.Color = productDto.Color;
            product.AñoModelo = productDto.AñoModelo;
            product.Transmision = productDto.Transmision;
            product.Tipo = productDto.Tipo;
            product.Puertas = productDto.Puertas;
            product.Descripcion = productDto.Descripcion;
            product.Estado = productDto.Estado;

            // Manejar la imagen
            if (productDto.ImageFileName != null)
            {
                // Aquí podrías agregar lógica para eliminar la imagen anterior si es necesario
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(productDto.ImageFileName.FileName);
                string imageFullPath = Path.Combine(environment.WebRootPath, "products", newFileName);
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    productDto.ImageFileName.CopyTo(stream);
                }
                product.ImageFileName = newFileName;
            }

            context.SaveChanges();
            TempData["SuccessMessage"] = "Producto editado con éxito.";
            return RedirectToAction("Index");
        }


    }
}
