using ECF.Core.applications.Core.Interfaces;
using ECF.Core.Entities.Entity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECF.Core.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CorreccionDocumentosController : ControllerBase
    {
        private readonly ICorreccionDocumentosManager _correccionDocumentosManager;

        public CorreccionDocumentosController(ICorreccionDocumentosManager correccionDocumentosManager)
        {
            _correccionDocumentosManager= correccionDocumentosManager;  
        }
        // GET: api/<CorreccionDocumentosController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_correccionDocumentosManager.GetAll().ToList());
        }

        // POST api/<CorreccionDocumentosController>
        [HttpPost]
        public IActionResult Post([FromBody] CorreccionDocumentos value)
        {
            _correccionDocumentosManager.Insert(value);
            _correccionDocumentosManager.Commit();
            return Ok("Proceso Realizado de manera exitosa");
        }

        // PUT api/<CorreccionDocumentosController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CorreccionDocumentos value)
        {
            _correccionDocumentosManager.Update(value);
            _correccionDocumentosManager.Commit();
            return Ok("Proceso Realizado de manera exitosa");
        }
    }
}
