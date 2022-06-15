using System.Collections.Generic;
using ApiConventions.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiConventions.Controllers
{
    #region snippet_ApiConventionTypeAttribute
    /// <summary>
    /// 约定 
    /// </summary>
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("api/[controller]")]
    public class ContactsConventionController : ControllerBase
    {
        #endregion
        private readonly IContactRepository _contacts;

        public ContactsConventionController(IContactRepository contacts)
        {
            _contacts = contacts;
        }

        // GET api/contactsconvention
        /// <summary>
        /// 查询全部 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Contact> Get()
        {
            return _contacts.GetAll();
        }

        // GET api/contactsconvention/{guid}
        /// <summary>
        /// 根据Id返回
        /// </summary>
        /// <remarks>
        /// 传入Id 返回时联系人
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetById")]
        public ActionResult<Contact> Get(string id)
        {
            var contact = _contacts.Get(id);

            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }

        // POST api/contactsconvention
        /// <summary>
        /// 新增 
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post(Contact contact)
        {
            _contacts.Add(contact);

            return CreatedAtRoute("GetById", new { id = contact.ID }, contact);
        }

        #region snippet_ApiConventionMethod
        // PUT api/contactsconvention/{guid}
        [HttpPut("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), 
                             nameof(DefaultApiConventions.Put))]
        public IActionResult Update(string id, Contact contact)
        {
            var contactToUpdate = _contacts.Get(id);

            if (contactToUpdate == null)
            {
                return NotFound();
            }

            _contacts.Update(contact);

            return NoContent();
        }
        #endregion

        // DELETE api/contactsconvention/{guid}
        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var contact = _contacts.Get(id);

            if (contact == null)
            {
                return NotFound();
            }

            _contacts.Remove(id);

            return NoContent();
        }
    }
}
