using AutoMapper;
using SeatReservationCore.Extensions;
using SeatReservationV1.Managers.Interfaces;
using SeatReservationV1.Models.Entities;
using SeatReservationV1.Models.Presentation;
using SeatReservationV1.Repositories;

namespace SeatReservationV1.Managers.Implementation
{
    public class UserManager : IUserManager
    {
        private readonly IMapper _mapper;

        private readonly UsersRepository _usersRepository;
        private readonly OrdersRepository _ordersRepository;

        public UserManager(IMapper mapper,
            UsersRepository usersRepository,
            OrdersRepository ordersRepository)
        {
            _mapper = mapper;
            _usersRepository = usersRepository;
            _ordersRepository = ordersRepository;
        }

        public async Task<int> RegisterAsync(RegisterUserVM userVM)
        {
            var userEntity = _mapper.Map<UserEntity>(userVM);

            return await _usersRepository.CreateAsync(userEntity);
        }

        public async Task<int> LoginAsync(string phoneNumber, string password)
        {
            var userId = await _usersRepository.GetIdByPhoneNumberAsync(phoneNumber, password);
            if (!userId.HasValue || userId <= 0)
                throw new Exception();

            return userId.Value;
        }

        public async Task<UserVM> GetAsync(int userId)
        {
            var user = await _usersRepository.GetAsync(userId);
            if (user == null)
                throw new Exception();

            return new UserVM
            {
                Id = user.Id,
                PhoneNumber = user.PhoneNumber,
                FIO = user.Surname + ' ' + user.Name + (!string.IsNullOrEmpty(user.Patronymic) ? $" {user.Patronymic}" : string.Empty) //TODO сделать хелпер под это
            };
        }

        public async Task<IEnumerable<UserVM>> GetAsync(IEnumerable<int> userIds)
        {
            var users = await _usersRepository.GetAsync(userIds);
            if (!users.HasElement())
                throw new Exception();

            return users.Select(user => new UserVM
            {
                Id = user.Id,
                PhoneNumber = user.PhoneNumber,
                FIO = user.Surname + ' ' + user.Name + (!string.IsNullOrEmpty(user.Patronymic) ? $" {user.Patronymic}" : string.Empty) //TODO сделать хелпер под это
            });
        }

        public async Task<UserVMAndOrdersCount> GetUserAndOrdersCountAsync(int userId, int restaurantId)
        {
            var userTask = GetAsync(userId);
            var ordersCountTask = _ordersRepository.GetCountByUserAndRestaurantAsync(userId, restaurantId);

            var user = await userTask;

            var result = _mapper.Map<UserVMAndOrdersCount>(user);

            var ordersCount = await ordersCountTask;

            result.OrdersCount = ordersCount;

            return result;
        }
    }
}
