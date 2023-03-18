using SeatReservationCore.Extensions;
using SeatReservationV1.Managers.Interfaces;
using SeatReservationV1.Models.Entities;
using SeatReservationV1.Models.Presentation;
using SeatReservationV1.Repositories;

namespace SeatReservationV1.Managers.Implementation
{
    public class OrderManager : IOrderManager
    {
        private readonly IRestaurantManager _restaurantManager;

        private readonly OrdersRepository _ordersRepository;

        public OrderManager(IRestaurantManager restaurantManager,
            OrdersRepository ordersRepository)
        {
            _restaurantManager = restaurantManager;
            _ordersRepository = ordersRepository;
        }

        public async Task<int> CreateAsync(CreateOrderVM createModel, int userId)
        {
            return await _ordersRepository.CreateAsync(new OrderEntity 
            { 
                RestaurantId = createModel.RestaurantId,
                UserId = userId,
                Date = createModel.Date,
                CreateDate = DateTime.UtcNow,
                PersonCount = createModel.PersonCount,
                Comment = createModel.Comment,
                IsActive = true
            });
        }

        public async Task<IEnumerable<OrderVM>> GetActiveAsync(int userId)
        {
            var orders = await _ordersRepository.GetActiveAsync(userId);
            if (!orders.HasElement())
                return Enumerable.Empty<OrderVM>();

            var restaurantIds = orders.Select(s => s.RestaurantId).Distinct();

            var restaurants = await _restaurantManager.GetBaseAsync(restaurantIds);

            return orders.Select(order => new OrderVM 
            {
                OrderId = order.Id,
                Date = order.Date,
                CreateDate = order.CreateDate,
                Comment = order.Comment,
                PersonCount = order.PersonCount,
                Restaurant = restaurants.FirstOrDefault(f => f.Id == order.RestaurantId)
            });
        }

        public async Task<IEnumerable<OrderVM>> GetHistoryAsync(int take, int skip, int userId)
        {
            var orders = await _ordersRepository.GetInactiveAsync(take, skip, userId);
            if (!orders.HasElement())
                return Enumerable.Empty<OrderVM>();

            var restaurantIds = orders.Select(s => s.RestaurantId).Distinct();

            var restaurants = await _restaurantManager.GetBaseAsync(restaurantIds);

            return orders.Select(order => new OrderVM
            {
                OrderId = order.Id,
                Date = order.Date,
                CreateDate = order.CreateDate,
                Comment = order.Comment,
                PersonCount = order.PersonCount,
                Restaurant = restaurants.FirstOrDefault(f => f.Id == order.RestaurantId)
            });
        }
    }
}
