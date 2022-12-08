using ECF.Core.Entities.Dto;
using Microsoft.AspNetCore.Mvc;
using System.DirectoryServices.AccountManagement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECF.Core.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {

        // POST api/<AutenticacionController>
        [HttpPost]
        public void Post([FromBody] LoginRequestDto value)
        {
            bool IsValid = false;
            var a = ContextType.Domain;
            PrincipalContext ctxModelo = new PrincipalContext(ContextType.Domain,"modelo.gmodelo.com.mx");
            IsValid = ctxModelo.ValidateCredentials("yjmerino", "22222222");


        }
    }
}
