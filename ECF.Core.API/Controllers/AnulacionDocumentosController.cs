using ECF.Core.applications.Core.Interfaces.Anulacion;
using ECF.Core.Entities.Entity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECF.Core.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnulacionDocumentosController : ControllerBase
    {
        private readonly IAnulacionDocumentosManager _anulacionDocumentosManager;

        public AnulacionDocumentosController(IAnulacionDocumentosManager anulacionDocumentosManager)
        {
            _anulacionDocumentosManager = anulacionDocumentosManager;
        }


        // GET: api/<AnulacionDocumentosController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_anulacionDocumentosManager.GetAll());
        }

        // POST api/<AnulacionDocumentosController>
        [HttpPost]
        public IActionResult Post([FromBody] AnulacionDocumentos value)
        {
            _anulacionDocumentosManager.Insert(value);
            _anulacionDocumentosManager.Commit();
            return Ok("Proceso realziado de manera exitosa");
        }

        // PUT api/<AnulacionDocumentosController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] AnulacionDocumentos value)
        {
            _anulacionDocumentosManager.Update(value);
            _anulacionDocumentosManager.Commit();
            return Ok("Proceso realziado de manera exitosa");
        }
    }
}
