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
        public async Task<IActionResult> GetAsync([FromQuery] PaginationDTO paginacion)
        {
            var queryable = _context.Productos.Include(p => p.ProductoImagenes).Include(p => p.CategoriasProducto).AsQueryable();

            if (!string.IsNullOrWhiteSpace(paginacion.Filter)) queryable = queryable.Where(p => p.Nombre.ToLower().Contains(paginacion.Filter!.ToLower()));

            return Ok(await queryable.OrderBy(p => p.Nombre).Paginate(paginacion).ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var producto = await _context.Productos.Include(p => p.ProductoImagenes).Include(p => p.CategoriasProducto!).ThenInclude(p => p.Categoria).FirstOrDefaultAsync(p => p.Id == id);

            if(producto is null) return NotFound();

            return Ok(producto);
        }

        [HttpGet("totalPages")]
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

        [HttpPut]
        public async Task<IActionResult> PutAsync(Producto prod)
        {
            try
            {
                _context.Update(prod);
                await _context.SaveChangesAsync();
                return Ok(prod);

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