using Cards.API.Data;
using Cards.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cards.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : Controller
    {
        private readonly CardsDbContext _cardsDbContext;
        
        public CardsController(CardsDbContext cardsDbContext)
        {
            this._cardsDbContext = cardsDbContext;
        }
        
        //Get All Cards
        [HttpGet]
        public async Task<IActionResult> GetAllCards()
        {
            var cards = await this._cardsDbContext.Cards.ToListAsync();
            return Ok(cards);
        }

        //Get single card
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetCard")]
        public async Task<IActionResult> GetCard([FromRoute] Guid id)
        {
            var card = await this._cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if (card != null)
            {
                return Ok(card);
            }
            return NotFound("Cartão não localizado");            
        }


        //Add Card
        [HttpPost]        
        public async Task<IActionResult> AddCard([FromBody] Card card)
        {   
            if (card != null)
            {
                card.Id = Guid.NewGuid();
                await this._cardsDbContext.Cards.AddAsync(card);
                await this._cardsDbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetCard), new { id = card.Id }, card);
            }
            return BadRequest("Não foi enviado os dados de cartão para incluir!");
        }

        //Update Card
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateCard([FromRoute] Guid id, [FromBody] Card card)
        {
            if (card != null)
            {
                var cardToUpdate = await this._cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
                if (cardToUpdate != null)
                {
                    cardToUpdate.CardholderName = card.CardholderName;
                    cardToUpdate.CardNumber = card.CardNumber;
                    cardToUpdate.ExpiryMonth = card.ExpiryMonth;
                    cardToUpdate.ExpiryYear = card.ExpiryYear;
                    cardToUpdate.CVC = card.CVC;

                    await this._cardsDbContext.SaveChangesAsync();
                    return Ok(cardToUpdate);
                }
                return NotFound("Cartão não localizado");
            }
            return BadRequest("Não foi enviado os dados de cartão para atualizar!");
        }

        //delete Card
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteCard([FromRoute] Guid id)
        {
            var cardToDelete = await this._cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if (cardToDelete != null)
            {
                this._cardsDbContext.Remove(cardToDelete);
                await this._cardsDbContext.SaveChangesAsync();
                return Ok(cardToDelete);
            }
            return NotFound("Cartão não localizado");
        }


    }
}
