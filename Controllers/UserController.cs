using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;
using UserManagementAPI.Services;
using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _service.GetById(id);
            return user is not null ? Ok(user) : NotFound();
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            var context = new ValidationContext(user);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(user, context, results, true))
                return BadRequest(results);

            _service.Add(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, User updatedUser)
        {
            updatedUser.Id = id;
            return _service.Update(updatedUser) ? Ok(updatedUser) : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) =>
            _service.Delete(id) ? Ok() : NotFound();
    }
}