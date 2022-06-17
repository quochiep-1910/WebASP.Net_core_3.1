using eShop.Data.EF;
using eShop.Data.Entities;
using eShop.Utilities.Exceptions;
using eShop.ViewModels.Common;
using eShop.ViewModels.Contact;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static eShop.Utilities.Constants.SystemConstants;

namespace eShop.Application.Contacts
{
    public class ContactService : IContactService
    {
        private readonly EShopDbContext _eShopDbContext;

        public ContactService(EShopDbContext eShopDbContext)
        {
            _eShopDbContext = eShopDbContext;
        }

        public async Task<List<ContactViewModel>> GetAll()
        {
            var query = from c in _eShopDbContext.Contacts
                        select c;
            return await query.Select(x => new ContactViewModel
            {
                Name = x.Name,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Message = x.Message,
                Id = x.Id,
                Status = (Status)x.Status
            }).ToListAsync();
        }

        public async Task<PagedResult<ContactViewModel>> GetAllPaging(ContactPagingRequest request)
        {
            var query = from c in _eShopDbContext.Contacts
                        select c;
            if (request.Keyword != null)
            {
                query = query.Where(c => c.Name == request.Keyword || c.Email == request.Keyword);
            }
            var totalRow = query.Count();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                             .Take(request.PageSize)
                             .Select(x => new ContactViewModel()
                             {
                                 Id = x.Id,
                                 Name = x.Name,
                                 Email = x.Email,
                                 PhoneNumber = x.PhoneNumber,
                                 Message = x.Message,
                                 Status = (Status)x.Status
                             }).ToListAsync();
            var result = new PagedResult<ContactViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return result;
        }

        public async Task<int> Create(ContactCreateViewModel request)
        {
            var contact = new Contact()
            {
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Message = request.Message,
                Status = request.Status
            };
            _eShopDbContext.Add(contact);
            await _eShopDbContext.SaveChangesAsync();
            return contact.Id;
        }

        public async Task<int> Delete(int contactId)
        {
            var existingContact = await _eShopDbContext.Contacts.FindAsync(contactId);
            if (existingContact == null)
            {
                throw new EShopException($"Khong the tim thay contact : {contactId}");
            }
            _eShopDbContext.Contacts.Remove(existingContact);
            return await _eShopDbContext.SaveChangesAsync();
        }

        public async Task<int> GetTotalContact()
        {
            var totalContacts = await _eShopDbContext.Contacts.CountAsync();
            if (totalContacts < 0)
            {
                //cannot find or error
                return 0;
            }
            return totalContacts;
        }

        public async Task<ContactViewModel> GetById(int contactId)
        {
            var existingContact = await _eShopDbContext.Contacts.FindAsync(contactId);
            if (existingContact == null)
            {
                throw new EShopException($"Khong the tim thay contact: {contactId}");
            }
            var contact = new ContactViewModel()
            {
                Id = existingContact.Id,
                Name = existingContact.Name,
                Email = existingContact.Email,
                PhoneNumber = existingContact.PhoneNumber,
                Message = existingContact.Message,
                Status = existingContact.Status
            };
            return contact;
        }

        public async Task<int> Update(ContactViewModel request)
        {
            var existingContact = await _eShopDbContext.Contacts.FindAsync(request.Id);
            if (existingContact == null)
            {
                throw new EShopException($"Khong the tim thay contact: {request.Id}");
            }
            existingContact.Name = request.Name;
            existingContact.PhoneNumber = request.PhoneNumber;
            existingContact.Message = request.Message;
            existingContact.Status = request.Status;
             _eShopDbContext.Contacts.Update(existingContact);
            await _eShopDbContext.SaveChangesAsync();
            return existingContact.Id;
        }
    }
}