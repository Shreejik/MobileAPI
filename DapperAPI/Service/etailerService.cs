using DapperAPI.dto;
using DapperAPI.Repository;

namespace DapperAPI.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public int RetailerRegistration(UserRetailerRequest userRetailerRequest)
        {
            var userExists = _userRepository.FindByEmail(userRetailerRequest.Email);
            if (userExists != null)
            {
                return 0;
            }
            userRetailerRequest.username = userRetailerRequest.Email;
            
            var added = _userRepository.RetailerRegistration(userRetailerRequest);
            return 1;
        }

        UserResponse IUserService.AddUser(User users)
        {
            throw new NotImplementedException();
        }

        userLoginResponse IUserService.CheckLogin(UserLoginRequest user)
        {
            try
            {

                return _userRepository.CheckLogin(user);
            }
            catch (Exception ex)
            {
                userLoginResponse userLoginResponse = new userLoginResponse();
                userLoginResponse.error = ex.Message + ex.InnerException;
                return userLoginResponse;
            }
        }

        void IUserService.DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        IEnumerable<UserResponse> IUserService.GetAllFactoryDistributorWise(int id)
        {
            throw new NotImplementedException();
        }

        IEnumerable<UserResponse> IUserService.GetAllRoleWise(int RoleId)
        {
            throw new NotImplementedException();
        }

        IEnumerable<UserResponse> IUserService.GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        UserResponse IUserService.GetUser(int id)
        {
            return _userRepository.GetUser(id);
        }

        UserResponse IUserService.UpdateUser(User users)
        {
            throw new NotImplementedException();
        }
    }
}
