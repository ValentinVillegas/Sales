using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.API.Helpers;
using Sales.Shared.DTOs;
using Sales.Shared.Entidades;

namespace Sales.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/productos")]
    public class ProductosController:ControllerBase
    {
        private readonly DataContext _context;
        private readonly IFileStorage _fileStorage;

        public ProductosController(DataContext context, IFileStorage fileStorage)
        {
            _context = context;
            _fileStorage = fileStorage;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsync([FromQuery] PaginationDTO paginacion)
        {
            var queryable = _context.Productos.Include(p => p.ProductoImagenes).Include(p => p.CategoriasProducto).AsQueryable();

            if (!string.IsNullOrWhiteSpace(paginacion.Filter)) queryable = queryable.Where(p => p.Nombre.ToLower().Contains(paginacion.Filter!.ToLower()));

            return Ok(await queryable.OrderBy(p => p.Nombre).Paginate(paginacion).ToListAsync());
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsync(int id)
        {
            var producto = await _context.Productos.Include(p => p.ProductoImagenes).Include(p => p.CategoriasProducto!).ThenInclude(p => p.Categoria).FirstOrDefaultAsync(p => p.Id == id);

            if(producto is null) return NotFound();

            return Ok(producto);
        }

        [HttpGet("totalPages")]
        [AllowAnonymous]
        public async Task<ActionResult> GetPaginas([FromQuery] PaginationDTO paginacion)
        {
            var queryable = _context.Productos.AsQueryable();
            if (!string.IsNullOrWhiteSpace(paginacion.Filter)) queryable = queryable.Where(p => p.Nombre.ToLower().Contains(paginacion.Filter.ToLower()));
            double cantidad = await queryable.CountAsync();
            double totalPaginas = Math.Ceiling(cantidad / paginacion.RecordsNumber);
            return Ok(totalPaginas);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(ProductoDTO productoDTO)
        {
            try
            {
                Producto prod = new Producto
                {
                    Nombre = productoDTO.Nombre,
                    Descripcion = productoDTO.Descripcion,
                    Precio = productoDTO.Precio,
                    Stock = productoDTO.Stock,
                    CategoriasProducto = new List<ProductoCategoria>(),
                    ProductoImagenes = new List<ProductoImage>()
                };

                foreach (var imagen in productoDTO.ImagenesProducto!)
                {
                    var imagenProd = Convert.FromBase64String(imagen);
                    prod.ProductoImagenes.Add(new ProductoImage
                    {
                        Image = await _fileStorage.SaveFileAsync(imagenProd, ".jpg", "products")
                    }); 
                }

                foreach (var categoriaId in productoDTO.IdCategoriasProducto)
                {
                    var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == categoriaId);
                    prod.CategoriasProducto.Add(new ProductoCategoria { Categoria = categoria!});
                }

                _context.Productos.Add(prod);
                await _context.SaveChangesAsync();

                return Ok(productoDTO);

            }catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe un producto con ese nombre.");
                }

                return BadRequest(dbUpdateException.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("agregarImagenes")]
        public async Task<IActionResult> PostAgregarImagenesAsync(ImagenDTO imagenDTO)
        {
            var producto = await _context.Productos.Include(x => x.ProductoImagenes).FirstOrDefaultAsync(x => x.Id == imagenDTO.ProductoId);

            if (producto is null) return NotFound();

            if (producto.ProductoImagenes is null) producto.ProductoImagenes = new List<ProductoImage>();

            for (int i = 0; i < imagenDTO.Imagenes.Count; i++)
            {
                if (!imagenDTO.Imagenes[i].StartsWith("https://sales2023.blob.core.windows.net/products/"))
                {
                    var imagenProducto = Convert.FromBase64String(imagenDTO.Imagenes[i]);
                    imagenDTO.Imagenes[i] = await _fileStorage.SaveFileAsync(imagenProducto, ".jpg", "products");
                    producto.ProductoImagenes.Add(new ProductoImage { Image = imagenDTO.Imagenes[i] });
                }
            }

            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
            return Ok(imagenDTO);
        }

        [HttpPost("removerUltimaImagen")]
        public async Task<IActionResult> PostRemoverUltimaImagenAsync(ImagenDTO imagenDTO)
        {
            var producto = await _context.Productos.Include(x => x.ProductoImagenes).FirstOrDefaultAsync(x => x.Id == imagenDTO.ProductoId);

            if(producto is null) return NotFound();

            if (producto.ProductoImagenes is null || producto.ProductoImagenes.Count == 0) return Ok();

            var ultimaImagen = producto.ProductoImagenes.LastOrDefault();
            await _fileStorage.RemoveFileAsync(ultimaImagen!.Image, "products");
            producto.ProductoImagenes.Remove(ultimaImagen);

            _context.Productos.Update(producto); 
            await _context.SaveChangesAsync();

            imagenDTO.Imagenes = producto.ProductoImagenes.Select(x => x.Image).ToList();

            return Ok(imagenDTO);
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(ProductoDTO productoDTO)
        {
            try
            {
                var producto = await _context.Productos
                    .Include(x => x.CategoriasProducto)
                    .FirstOrDefaultAsync(x => x.Id == productoDTO.Id);
                
                if (producto == null) return NotFound();

                producto.Nombre = productoDTO.Nombre;
                producto.Descripcion = productoDTO.Descripcion;
                producto.Precio = productoDTO.Precio;
                producto.Stock = productoDTO.Stock;
                producto.CategoriasProducto = productoDTO.IdCategoriasProducto!.Select(x => new ProductoCategoria { CategoriaId = x }).ToList();

                _context.Update(producto);
                await _context.SaveChangesAsync();
                return Ok(productoDTO);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe un producto con el mismo nombre.");
                }

                return BadRequest(dbUpdateException.Message);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete("id:int")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var prod = await _context.Productos.FirstOrDefaultAsync(p => p.Id == id);

            if (prod == null) return NotFound();

            _context.Productos.Remove(prod);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}