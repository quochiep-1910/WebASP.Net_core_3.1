using eShop.ViewModels.Contact;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.Contact
{
    public interface IContactService
    {
        Task<int> Create(ContactViewModel request);

        Task<int> Update(ContactViewModel request);

        Task<int> Delete(int contactId);

        Task<ContactViewModel> GetById(int productId, string languageId);
    }
}