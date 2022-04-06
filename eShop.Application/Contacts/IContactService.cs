using eShop.ViewModels.Common;
using eShop.ViewModels.Contact;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.Application.Contacts
{
    public interface IContactService
    {
        Task<int> Create(ContactCreateViewModel request);

        Task<int> Update(ContactViewModel request);

        Task<int> Delete(int contactId);
        Task<List<ContactViewModel>> GetAll();
        Task<PagedResult<ContactViewModel>> GetAllPaging(ContactPagingRequest request);

        Task<ContactViewModel> GetById(int contactId);
    }
}