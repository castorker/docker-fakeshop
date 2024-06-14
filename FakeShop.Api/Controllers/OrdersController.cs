using AutoMapper;
using FakeShop.Api.ApiModels;
using FakeShop.Api.Domain.Entities;
using FakeShop.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FakeShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;

        public OrdersController(IOrderRepository orderRepository,
            ILogger<OrdersController> logger, 
            IMapper mapper)
        {
            _orderRepository = orderRepository ?? 
                throw new ArgumentNullException(nameof(orderRepository));
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? 
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        public async Task<Guid> SubmitOrder(OrderDto orderInfo)
        {
            _logger.LogInformation($"Submitting order for {orderInfo.Quantity} of {orderInfo.ProductId}.");

            var orderToBeCreated = _mapper.Map<Order>(orderInfo);

            var orderCreated = await _orderRepository.PlaceOrder(orderToBeCreated, 1234); // would get customer id from authN system/User claims

            return orderCreated;
        }
    }
}
