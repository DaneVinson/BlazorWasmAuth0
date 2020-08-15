using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CharactersController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Character>>> GetCharactersAsync()
        {
            var characters = new Character[]
            {
                new Character() { Name = "Bilbo", Race = "Hobbit" },
                new Character() { Name = "Samwise", Race = "Hobbit" },
                new Character() { Name = "Aragorn", Race = "Human" },
                new Character() { Name = "Gandalf", Race = "Human" },
                new Character() { Name = "Legolas", Race = "Elf" },
                new Character() { Name = "Smeagol", Race = "?" }
            };

            return await Task.FromResult(Ok(characters));
        }
    }
}
