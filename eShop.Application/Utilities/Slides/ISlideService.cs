using eShop.ViewModels.Utilities.Slides;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.Application.Utilities.Slides
{
    public interface ISlideService
    {
        Task<List<SlideViewModel>> GetAll();
    }
}