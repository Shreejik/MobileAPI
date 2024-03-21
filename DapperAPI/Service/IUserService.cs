namespace DapperAPI.Service
{
    using DapperAPI.dto;
    public interface IUserService
    {
        Task<IEnumerable<AllCodeCommon>> GetCompanysByDistributor(string dCode);
        IEnumerable<UserResponse> GetAllUsers(); 

        IEnumerable<UserResponse> GetAllRoleWise(int RoleId);

        IEnumerable<UserResponse> GetAllFactoryDistributorWise(int id);

        UserResponse GetUser(int id);

        void DeleteUser(int id);

        Task<int> RetailerRegistration(UserRetailerRequest userRetailerRequest);

        UserResponse AddUser (User users);

        UserResponse UpdateUser(User users);
        userLoginResponse CheckLogin(UserLoginRequest user);

        Task<int> updatePassword(int userID, string password);

       Task<userLoginResponse> FindByEmail(string email);

        Task<IEnumerable<AllCodeCommon>> GetCompanysList(string dCode);

        Task<int> AddCompany(UserCmpMap obj);
        Task<int> DeleteCompany(UserCmpMap obj);
    }

}
