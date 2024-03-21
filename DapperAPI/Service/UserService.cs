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

        public async Task<IEnumerable<AllCodeCommon>> GetCompanysByDistributor(string dCode)
        {
            return await _userRepository.GetCompanysByDistributor(dCode);
        }

        public async Task<IEnumerable<AllCodeCommon>> GetCompanysList(string UserID)
        {
            return await _userRepository.GetCompanysList(UserID);
        }

        public async Task<int> RetailerRegistration(UserRetailerRequest userRetailerRequest)
        {
            var userExists = await _userRepository.FindByEmail(userRetailerRequest.Email);
            if (userExists != null)
            {
                return 0;
            }
            userRetailerRequest.username = userRetailerRequest.Email;
            
            var added =  _userRepository.RetailerRegistration(userRetailerRequest);
            return 1;
        }

        public async Task<int> AddCompany(UserCmpMap obj)
        {
            var userExists = await _userRepository.CheckCompany(obj.CmpID);
            if (userExists != null && userExists == 0 )
            {
                return 0;
            }
            var added = _userRepository.AddCompany(obj);
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

        public async Task<int> updatePassword(int userID, string password)
        { 
            return await _userRepository.updatePassword(userID, password);
        }

        public async Task<userLoginResponse> FindByEmail(string email)
        { 
            var result = await _userRepository.FindByEmail(email);
            return result;
        }

        async Task<int> IUserService.DeleteCompany(UserCmpMap obj)
        {
            var added = await _userRepository.DeleteCompany(obj);
            return 1;
        }
    }
}
