using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthOptions _options;

        public AuthController(AuthOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        [HttpGet]
        public ActionResult<ClientOptions> GetClientOptions([FromQuery]string customerName)
        {
            var clientOptions = _options.Clients.FirstOrDefault(c => c.Name.Equals(customerName, StringComparison.OrdinalIgnoreCase));
            if (clientOptions == null) { return BadRequest(); }
            else
            {
                clientOptions.Authority = _options.Authority;
                return Ok(clientOptions);
            }
        }

        [HttpPost]
        public ActionResult<int[]> GetUserShards([FromBody]UserOptions options)
        {
            if (options == null) { return BadRequest(); }
            if (!_options.ApiKey.Equals(options.ApiKey, StringComparison.OrdinalIgnoreCase)) { return Unauthorized(); }

            if ("shire".Equals(options?.Customer, StringComparison.OrdinalIgnoreCase) && "bilbo.baggins@shire.me".Equals(options.Name, StringComparison.OrdinalIgnoreCase))
            {
                return new int[] { 1, 2, 3 };
            }
            else if ("mordor".Equals(options.Customer, StringComparison.OrdinalIgnoreCase) && "sauron@mordor.me".Equals(options.Name, StringComparison.OrdinalIgnoreCase))
            {
                return new int[] { 666 };
            }
            else if ("portland".Equals(options.Customer, StringComparison.OrdinalIgnoreCase) && "Dane Vinson".Equals(options.Name, StringComparison.OrdinalIgnoreCase))
            {
                return new int[] { 23, 24 };
            }
            else { return Array.Empty<int>(); }
        }
    }
}
