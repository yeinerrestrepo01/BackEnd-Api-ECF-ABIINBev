using ECF.Core.applications.Core.Interfaces;
using ECF.Core.Entities.Entity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECF.Core.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfiguracionTipoNcfController : ControllerBase
    {
        private readonly IConfiguracionTipoNcfManager _configuracionTipoNcfManager;

        public ConfiguracionTipoNcfController(IConfiguracionTipoNcfManager configuracionTipoNcfManager)
        {
            _configuracionTipoNcfManager = configuracionTipoNcfManager;
        }

        // GET: api/<ConfiguracionTipoNcfController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_configuracionTipoNcfManager.ObtenerConfiguracionTipoNCFEmpresa());
        }

        // GET api/<ConfiguracionTipoNcfController>/5
        [HttpGet("{idEmpresa}")]
        public IActionResult Get(string idEmpresa)
        {
            return Ok(_configuracionTipoNcfManager.ObtenerConfiguracionTipoNCFEmpresa(idEmpresa));
        }

        // POST api/<ConfiguracionTipoNcfController>
        [HttpPost]
        public IActionResult Post([FromBody] ConfiguracionTipoNCF value)
        {
            _configuracionTipoNcfManager.Insert(value);
            _configuracionTipoNcfManager.Commit();
            return Ok("Registro Creado Exitosamente");
        }
    }
}
