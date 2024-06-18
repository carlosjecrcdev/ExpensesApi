using System.Threading.Tasks;
using ExpensesApi.Interfaces;
using ExpensesApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserAccountController : ControllerBase
    {
        private readonly IUserAccountServices _userAccountServices;
        public UserAccountController(IUserAccountServices userAccountServices) 
        {
            _userAccountServices = userAccountServices;
        }

        [HttpGet]
        public async Task<IEnumerable<UserAccount>> Get()
        {
            return await _userAccountServices.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserAccount>> GetById(int id)
        {
            var user = await _userAccountServices.GetById(id);

            if (user == null)
                throw new KeyNotFoundException($"UserAccount {id} no se encontro");

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserAccount user)
        {
            await _userAccountServices.Create(user);
            return CreatedAtAction(nameof(GetById),new {id = user.UserId}, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserAccount user)
        {
            var userToUpdate = await _userAccountServices.GetById(id);

            if (userToUpdate == null)
                throw new KeyNotFoundException($"UserAccount {id} no se encontro");

            await _userAccountServices.Update(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userToUpdate = await _userAccountServices.GetById(id);

            if (userToUpdate == null)
                throw new KeyNotFoundException($"UserAccount {id} no se encontro");

            await _userAccountServices.Delete(id);
            return Ok();
        }
    }
}
