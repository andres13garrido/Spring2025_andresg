// Api.eCommerce/Controllers/ShoppingCartController.cs
using Api.eCommerce.EC;
using Library.eCommerce.Models;
using Library.eCommerce.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Api.eCommerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ShoppingCartEC _ec = new();

        [HttpGet]
        public IEnumerable<Item> Get() =>
            _ec.GetAll();

        [HttpGet("Search")]
        public IEnumerable<Item> Search([FromBody] QueryRequest qr) =>
            _ec.Search(qr.Query);

        [HttpPost("Purchase/{id}")]
        public Item Purchase(int id) =>
            _ec.Purchase(id);

        [HttpPost("Return/{id}")]
        public Item Return(int id) =>
            _ec.Return(id);

        [HttpPost("Checkout")]
        public Receipt Checkout() =>
            _ec.FinalizePurchase();
    }
}
