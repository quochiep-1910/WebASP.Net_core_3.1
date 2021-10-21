using eShop.ViewModels.Contact;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.Contact
{
    public class ContactService : IContactService
    {
        public Task<int> Create(ContactViewModel request)
        {
            throw new NotImplementedException();
        }

        public Task<int> Delete(int contactId)
        {
            throw new NotImplementedException();
        }

        public Task<ContactViewModel> GetById(int productId, string languageId)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(ContactViewModel request)
        {
            throw new NotImplementedException();
        }
    }
}