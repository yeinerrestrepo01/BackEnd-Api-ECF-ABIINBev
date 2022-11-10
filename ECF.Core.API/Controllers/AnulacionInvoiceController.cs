using ECF.Core.applications.Core.Interfaces.Anulacion;
using ECF.Core.Entities.Dto;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECF.Core.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnulacionInvoiceController : ControllerBase
    {
        private readonly IAnulacionManager _anulacionManager;

        /// <summary>
        /// Inicializador de controller <clase>AnulacionInvoiceController</clase>
        /// </summary>
        /// <param name="anulacionManager"></param>
        public AnulacionInvoiceController(IAnulacionManager anulacionManager)
        {
            _anulacionManager = anulacionManager;
        }

        // POST api/<AnulacionInvoiceController>
        [HttpPost]
        public IActionResult Post([FromBody] AnulacionInvoideDto anulacionInvoideDto)
        {
            var resultadoAnluacion = _anulacionManager.AnulacionFactura(anulacionInvoideDto);
            return Ok(resultadoAnluacion);
        }
    }
}
