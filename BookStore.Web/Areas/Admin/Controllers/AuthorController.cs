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
    public class AuthorController : Controller
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public AuthorController(IRepositoryWrapper repositoryWrapper, ShopService bookService, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _repository = repositoryWrapper;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            List<AuthorDTO> authors = _repository.Authors.GetAll(orderBy: x => x.OrderBy(y => y.FullName)).ToList();
            List<AuthorVM> model = _mapper.Map<IEnumerable<AuthorVM>>(authors).ToList();
            return View(model);
        }
    }
}
