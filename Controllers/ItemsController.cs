using Microsoft.AspNetCore.Mvc;
using Catalog.Repositories;
using Catalog.Entities;
using System.Linq;
using Catalog.Dtos;

namespace Catalog.Controllers
{

    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase 
    {
        private readonly IInMemItemsRepository repository;

        public ItemsController(InMemItemsRepository repository)
        {
            this.repository = repository;
        }
        //Get /items
        [HttpGet]
        public IEnumerable<ItemDto> GetItems() {
            var items = repository.GetItems().Select(item => item.AsDto());
            return items;
        }
        //GET /items/id
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id) {
            var item = repository.GetItem(id);

            if (item is null) 
            {
                return NotFound();
            }

            return item.AsDto();
        }

    }



}