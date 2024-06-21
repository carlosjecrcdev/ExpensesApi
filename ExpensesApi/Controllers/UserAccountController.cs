using System.Threading.Tasks;
using AutoMapper;
using ExpensesApi.Exceptions;
using ExpensesApi.Interfaces;
using ExpensesApi.Models.Dtos;
using ExpensesApi.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserAccountController : ControllerBase
    {
        private readonly IUserAccountServices _userAccountServices;
        private readonly IMapper _mapper;

        public UserAccountController(IUserAccountServices userAccountServices, IMapper mapper) 
        {
            _userAccountServices = userAccountServices;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<UserAccountDto>> Get()
        {
            return _mapper.Map<IEnumerable<UserAccountDto>>(await _userAccountServices.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserAccountDto>> GetById(int id)
        {
            var user = await _userAccountServices.GetById(id);

            if (user == null)
                throw new KeyNotFoundException($"UserAccount not found");

            UserAccountDto userDto = _mapper.Map<UserAccountDto>(user);

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserAccountDto userDto)
        {
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));

                throw new ApiExceptions(messages);
            }
            UserAccount user = _mapper.Map<UserAccount>(userDto);
            await _userAccountServices.Create(user);
            return CreatedAtAction(nameof(GetById),new {id = user.UserId}, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserAccountDto userDto)
        {
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));

                throw new ApiExceptions(messages);
            }

            if (userDto.Id != id)
                throw new KeyNotFoundException($"Id is diferent");

            var user = await _userAccountServices.GetById(id);

            if (user == null)
                throw new KeyNotFoundException($"UserAccount not found");

            _mapper.Map(userDto, user);
            await _userAccountServices.Update();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userToUpdate = await _userAccountServices.GetById(id);

            if (userToUpdate == null)
                throw new KeyNotFoundException($"UserAccount not found");

            await _userAccountServices.Delete(userToUpdate);
            return Ok();
        }
    }
}
