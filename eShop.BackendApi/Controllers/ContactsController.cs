using eShop.Application.Contacts;
using eShop.ViewModels.Contact;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShop.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var contact = await _contactService.GetAll();
            return Ok(contact);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] ContactPagingRequest request)
        {
            var contacts = await _contactService.GetAllPaging(request);
            return Ok(contacts);
        }

        [HttpGet("{contactId}")]
        public async Task<IActionResult> GetById(int contactId)
        {
            var contact = await _contactService.GetById(contactId);
            if (contact == null)
                return BadRequest("khong tim thay contact");
            return Ok(contact);
        }

        [HttpGet("totalContact")]
        [Authorize]
        public async Task<IActionResult> GetTotalContact()
        {
            var totalContact = await _contactService.GetTotalContact();

            return Ok(totalContact);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] ContactCreateViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var contactId = await _contactService.Create(request);
            if (contactId == 0)
                return BadRequest();

            var contact = await _contactService.GetById(contactId);

            return CreatedAtAction(nameof(GetById), new { id = contactId }, contact);
        }

        [HttpPut("{contactId}")]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int contactId, [FromForm] ContactViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            request.Id = contactId;
            var affectedResult = await _contactService.Update(request);
            if (affectedResult == 0)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("{contactId}")]
        [Authorize]
        public async Task<IActionResult> Delete(int contactId)
        {
            var affectedResult = await _contactService.Delete(contactId);
            if (affectedResult == 0)
                return BadRequest();

            return Ok();
        }
    }
}