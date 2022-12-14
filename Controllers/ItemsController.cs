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
        private readonly IItemsRepository repository;

        public ItemsController(IItemsRepository repository)
        {
            this.repository = repository;
        }
        //Get /items
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync() {
            var items = (await repository.GetItemsAsync())
            .Select(item => item.AsDto());
            return items;
        }
        //GET /items/id
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItem(Guid id) {
            var item = await repository.GetItemAsync(id);

            if (item is null) 
            {
                return NotFound();
            }

            return item.AsDto();
        }
        //POST /items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
        {
            Item item = new(){
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                CreatedDate = DateTimeOffset.UtcNow
            };
            await repository.CreateItemAsync(item);

            return CreatedAtAction(nameof(GetItemsAsync),new { id = item.Id}, item.AsDto());
        }
        //PUT /items/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItem(Guid id, UpdateItemDto itemDto){
            var existingItem = await repository.GetItemAsync(id);

            if(existingItem is null) {
                return NotFound();
            }

            Item updatedItem = existingItem with {
                Name = itemDto.Name,
            };

           await repository.UpdateItemAsync(updatedItem);
            return NoContent();
        }
        //DELETE /item
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(Guid id){
             var existingItem = await repository.GetItemAsync(id);

            if(existingItem is null) {
                return NotFound();
            }
            await repository.DeleteItemAsync(id);
            return NoContent();
        }
    }



}