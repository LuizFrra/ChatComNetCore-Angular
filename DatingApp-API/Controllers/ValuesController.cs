using DatingApp.Data;
using DatingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace DatingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private DataContext dataContext;

        public ValuesController(DataContext _dataContext)
        {
            dataContext = _dataContext;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Value>>> GetRegisters()
        { 
            return Ok(await dataContext.Values.ToListAsync());
        }

        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<Value>> GetRegisterById(int id)
        {
            if (id <= 0)
                return BadRequest();

            var register = await dataContext.Values.FirstOrDefaultAsync(v => v.Id == id);

            if (register == null)
                return NotFound();

            return Ok(register);
        }

        // PUT api/values/5/nome
        [HttpPost("[action]")]
        public async Task<ActionResult<Value>> AddRegister([FromBody]Value register)
        {
            if (register.Id <= 0 || string.IsNullOrEmpty(register.Name) || string.IsNullOrWhiteSpace(register.Name))
                return BadRequest();

            dataContext.Add<Value>(new Value(register.Id, register.Name));
            int result = await dataContext.SaveChangesAsync();

            if (result > 0)
                return Ok(register);

            return BadRequest();
        }
    }
}
