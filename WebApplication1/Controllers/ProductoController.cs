using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{

    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {

        public readonly PracticasSafsContext db_context;

        public ProductoController(PracticasSafsContext context)
        {
            db_context = context;

        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Producto> lista = new List<Producto>();


            try
            {

                lista = db_context.Productos.Include(c => c.oCategoria).ToList();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista });

            }
            catch (Exception ex) {

                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex, response = lista });

            }


        }


        [HttpGet]
        [Route("Obtener/{idProducto:int}")]
        public IActionResult Obtener(int idProducto)
        {
            Producto oProducto = db_context.Productos.Find(idProducto);

            if (oProducto == null)
            {
                return BadRequest("Producto no encontrado");

            }

            try
            {

                oProducto = db_context.Productos.Include(c => c.oCategoria).Where(p => p.IdProducto == idProducto).FirstOrDefault();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = oProducto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = oProducto });


            }
        }




        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Producto objeto)
        {


            try
            {
                db_context.Productos.Add(objeto);
                db_context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }




        }



        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Producto objeto)
        {
            Producto oProducto = db_context.Productos.Find(objeto.IdProducto);

            if (oProducto == null)
            {
                return BadRequest("Producto no encontrado");

            }

            try
            {
                oProducto.CodigoBarra = objeto.CodigoBarra is null ? oProducto.CodigoBarra : objeto.CodigoBarra;
                oProducto.Descripcion = objeto.Descripcion is null ? oProducto.Descripcion : objeto.Descripcion;
                oProducto.Marca = objeto.Marca is null ? oProducto.Marca : objeto.Marca;
                oProducto.IdCategoria = objeto.IdCategoria is null ? oProducto.IdCategoria : objeto.IdCategoria;
                oProducto.Precio = objeto.Precio is null ? oProducto.Precio : objeto.Precio;



                db_context.Productos.Update(oProducto);
                db_context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }

        }



        [HttpDelete]
        [Route("Eliminar/{idProducto:int}")]
        public IActionResult Eliminar(int idProducto)
        {

            Producto oProducto = db_context.Productos.Find(idProducto);

            if (oProducto == null)
            {
                return BadRequest("Producto no encontrado");

            }

            try
            {

                db_context.Productos.Remove(oProducto);
                db_context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }


        }

    }
}
