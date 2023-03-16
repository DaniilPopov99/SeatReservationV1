using SeatReservationV1.Managers.Interfaces;
using SeatReservationV1.Models.Entities;
using SeatReservationV1.Models.Presentation;
using SeatReservationV1.Repositories;

namespace SeatReservationV1.Managers.Implementation
{
    public class UserManager : IUserManager
    {
        private readonly UsersRepository _usersRepository;

        public UserManager(UsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<int> RegisterAsync(RegisterUserVM userVM)
        {
            return await _usersRepository.CreateAsync(new UserEntity 
            {
                Name = userVM.Name,
                Surname = userVM.Surname,
                Patronymic = userVM.Patronymic,
                PhoneNumber = userVM.PhoneNumber,
                Password = userVM.Password,
                CreateDate = DateTime.UtcNow
            });
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
                FIO = user.Surname + ' ' + user.Name + (!string.IsNullOrEmpty(user.Patronymic) ? $" {user.Patronymic}" : string.Empty)
            };
        }
    }
}
