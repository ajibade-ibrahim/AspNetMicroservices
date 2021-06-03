using System;
using System.Threading.Tasks;
using Basket.API.Entities;
using Basket.API.Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        public BasketController(IBasketRepository basketRepository, ILogger<BasketController> logger)
        {
            _basketRepository = basketRepository;
            _logger = logger;
        }

        private readonly IBasketRepository _basketRepository;
        private readonly ILogger<BasketController> _logger;

        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string username)
        {
            try
            {
                var basket = await _basketRepository.GetBasket(username);
                return Ok(basket ?? new ShoppingCart(username));
            }
            catch (Exception exception)
            {
                LogException(exception);
                return GetInternalServerError(exception);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart cart)
        {
            try
            {
                return Ok(await _basketRepository.UpdateBasket(cart));
            }
            catch (Exception exception)
            {
                LogException(exception);
                return GetInternalServerError(exception);
            }
        }

        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteBasket(string username)
        {
            try
            {
                await _basketRepository.DeleteBasket(username);
                return NoContent();
            }
            catch (Exception exception)
            {
                LogException(exception);
                return GetInternalServerError(exception);
            }
        }

        private ObjectResult GetInternalServerError(Exception exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
        }

        private void LogException(Exception exception)
        {
            _logger.LogError($"Error occurred: {exception}");
        }
    }
}