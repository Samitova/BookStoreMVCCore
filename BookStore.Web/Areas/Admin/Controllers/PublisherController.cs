using AutoMapper;
using BookStore.Data.Models.ModelsDTO;
using BookStore.Data.Models.ViewModels;
using BookStore.Services.DataBaseService.Interfaces;
using BookStore.Services.ShopService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

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
    }
}
