using ECF.Core.applications.Core.Interfaces;
using ECF.Core.Entities.Dto;
using ECF.Core.Entities.Entity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECF.Core.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceSettingController : ControllerBase
    {
        private readonly IInvoiceSettingManager _invoiceSettingManager;
        public InvoiceSettingController(IInvoiceSettingManager invoiceSettingManager)
        {
            _invoiceSettingManager = invoiceSettingManager;
        }

        // GET api/<InvoiceSettingController>/5
        [HttpGet("{cIDInvoice},{IdCustumer}")]
        public IActionResult Get(string cIDInvoice, string IdCustumer)
        {
            var consultaFactura = _invoiceSettingManager.ObtenerInformacionFactura(cIDInvoice, IdCustumer);
            return Ok(consultaFactura);
        }

        // POST api/<InvoiceSettingController>
        [HttpPost]
        public IActionResult Post([FromBody] FacturaAjusteDto ajusteFactura)
        {
            _invoiceSettingManager.AjusteFactura(ajusteFactura);
            return Ok(new RespuestaGenerica<bool>());
        }
    }
}
