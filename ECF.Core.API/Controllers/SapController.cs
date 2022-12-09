using ECF.Core.applications.Core.Interfaces.EventosSap;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECF.Core.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SapController : ControllerBase
    {
        private readonly IAccionesSap _accionesSap;

        public SapController(IAccionesSap accionesSap)
        {
            _accionesSap = accionesSap;
        }
        // GET: api/<SapController>
        [HttpGet]
        public IActionResult Get()
        {
            var resultadoConsultaDocumento = _accionesSap.DocumentoOriginalNCF();
            return Ok(resultadoConsultaDocumento);
        }

        [HttpPost]
        public IActionResult Post([FromBody]  int idReporte)
        {
            return Ok("");
        }
    }
}
