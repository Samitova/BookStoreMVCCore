using AutoMapper;
using BookStore.Data.Models.ModelsDTO;
using BookStore.Data.Models.ViewModels;
using BookStore.Services.DataBaseService.Interfaces;
using BookStore.Services.ShopService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace BookStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PublisherController : Controller
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        public PublisherController(IRepositoryWrapper repositoryWrapper, ShopService bookService, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {            
            _repository = repositoryWrapper;            
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            List<PublisherDTO> publishers = _repository.Publishers.GetAll(orderBy: x => x.OrderBy(y => y.PublisherName)).ToList();
            List<PublisherVM> model = _mapper.Map<IEnumerable<PublisherVM>>(publishers).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult CreateUpdatePublisher(int? id)
        {
            PublisherDTO publisher = new PublisherDTO();     
            if (id == null || id == 0)
            {
                return View(_mapper.Map<PublisherVM>(publisher));
            }
            else
            {
                publisher = _repository.Publishers.GetById(id);
                if (publisher == null)
                    return NotFound();
                else
                {                    
                    return View(_mapper.Map<PublisherVM>(publisher));
                }                    
            }
        }

        [HttpPost]
        public IActionResult CreateUpdatePublisher(PublisherVM publisherVM)
        {
            if (ModelState.IsValid)
            {
                if (publisherVM.Id == 0)
                {
                    _repository.Publishers.Add(_mapper.Map<PublisherDTO>(publisherVM));
                    TempData["success"] = "Publisher was created successfuly";
                }
                else
                {
                    _repository.Publishers.Update(_mapper.Map<PublisherDTO>(publisherVM));
                    TempData["success"] = "Publisher was updated successfuly";
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeletePublisher(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var publisher = _repository.Publishers.GetById(id);
            if (publisher == null)
            {
                return NotFound();
            }
            return View(_mapper.Map<PublisherVM>(publisher));
        }

        [HttpPost, ActionName("DeletePublisher")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePublisherPost(int? id)
        {
            var publisher = _repository.Publishers.GetById(id);
            if (publisher == null)
            {
                return NotFound();
            }
            _repository.Publishers.Delete(publisher);
            TempData["success"] = $"Publisher \"{publisher.PublisherName}\" was deleted successfuly";
            return RedirectToAction("Index");
        }
    }
}
