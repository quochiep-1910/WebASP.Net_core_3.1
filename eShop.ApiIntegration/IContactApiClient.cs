using eShop.ViewModels.Common;
using eShop.ViewModels.Contact;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.ApiIntegration
{
    public interface IContactApiClient
    {
        Task<bool> Create(ContactCreateViewModel request);

        Task<bool> Update(ContactViewModel request);

        Task<bool> DeleteContact(int contactId);

        Task<List<ContactViewModel>> GetAll();

        Task<PagedResult<ContactViewModel>> GetAllPaging(ContactPagingRequest request);

        Task<ContactViewModel> GetById(int contactId);

        Task<int> GetTotalContact();
    }
}