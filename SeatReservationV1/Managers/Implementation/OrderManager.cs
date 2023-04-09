using AutoMapper;
using SeatReservationCore.Extensions;
using SeatReservationV1.Managers.Interfaces;
using SeatReservationV1.Models.Entities;
using SeatReservationV1.Models.Presentation;
using SeatReservationV1.Repositories;

namespace SeatReservationV1.Managers.Implementation
{
    public class OrderManager : IOrderManager
    {
        private readonly IMapper _mapper;

        private readonly IRestaurantManager _restaurantManager;
        private readonly IUserManager _userManager;

        private readonly OrdersRepository _ordersRepository;

        public OrderManager(IMapper mapper,
            IRestaurantManager restaurantManager,
            IUserManager userManager,
            OrdersRepository ordersRepository)
        {
            _mapper = mapper;
            _restaurantManager = restaurantManager;
            _userManager = userManager;
            _ordersRepository = ordersRepository;
        }

        public async Task<int> CreateAsync(CreateOrderVM createModel, int userId)
        {
            if (createModel.Date <= DateTime.UtcNow)
                throw new Exception();

            var orderEntity = _mapper.Map<OrderEntity>(createModel);

            orderEntity.UserId = userId;

            return await _ordersRepository.CreateAsync(orderEntity);
        }

        public async Task<IEnumerable<UserOrderVM>> GetActiveAsync(int userId)
        {
            var orders = await _ordersRepository.GetActiveAsync(userId);

            return await ToUserOrdersVMAsync(orders);
        }

        public async Task<IEnumerable<UserOrderVM>> GetHistoryAsync(int take, int skip, int userId)
        {
            var orders = await _ordersRepository.GetInactiveAsync(take, skip, userId);

            return await ToUserOrdersVMAsync(orders);
        }

        public async Task<IEnumerable<RestaurantOrderVM>> GetActiveByRestaurantIdAsync(int take, int skip, int restaurantId)
        {
            var orders = await _ordersRepository.GetActiveByRestaurantAsync(take, skip, restaurantId);

            return await ToRestaurantOrdersVMAsync(orders);
        }

        public async Task<IEnumerable<RestaurantOrderVM>> GetInactiveByRestaurantIdAsync(int take, int skip, int restaurantId)
        {
            var orders = await _ordersRepository.GetInactiveByRestaurantAsync(take, skip, restaurantId);

            return await ToRestaurantOrdersVMAsync(orders);
        }

        private async Task<IEnumerable<UserOrderVM>> ToUserOrdersVMAsync(IEnumerable<OrderEntity> orders)
        {
            if (!orders.HasElement())
                return Enumerable.Empty<UserOrderVM>();

            var restaurantIds = orders.Select(s => s.RestaurantId).Distinct();

            var restaurants = await _restaurantManager.GetBaseAsync(restaurantIds);

            var restaurantsDict = restaurants.ToDictionary(k => k.Id, v => v);

            return orders.Select(order =>
            {
                var orderVM = _mapper.Map<UserOrderVM>(order);

                orderVM.Restaurant = restaurantsDict.GetValueOrDefault(order.RestaurantId);

                return orderVM;
            });
        }

        private async Task<IEnumerable<RestaurantOrderVM>> ToRestaurantOrdersVMAsync(IEnumerable<OrderEntity> orders)
        {
            if (!orders.HasElement())
                return Enumerable.Empty<RestaurantOrderVM>();

            var userIds = orders.Select(s => s.UserId).Distinct();

            var users = await _userManager.GetAsync(userIds);

            var usersDict = users.ToDictionary(k => k.Id, v => v);

            return orders.Select(order =>
            {
                var orderVM = _mapper.Map<RestaurantOrderVM>(order);

                orderVM.User = usersDict.GetValueOrDefault(order.RestaurantId);

                return orderVM;
            });
        }
    }
}
