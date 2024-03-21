namespace DapperAPI.Repository
{
    using DapperAPI.dto;
    public interface IUserRepository
    {
        Task<IEnumerable<AllCodeCommon>> GetCompanysByDistributor(string dCode);
        IEnumerable<UserResponse> GetAllUsers();

        IEnumerable<UserResponse> GetAllRoleWise(int RoleId);

        IEnumerable<UserResponse> GetAllFactoryDistributorWise(int id);

        void DeleteUser(int id);

        UserResponse AddUser(User users);

        UserResponse UpdateUser(User users);
        UserResponse GetUser(int id);

        userLoginResponse CheckLogin(UserLoginRequest user);

        Task<userLoginResponse> FindByEmail(string email);
        object RetailerRegistration(UserRetailerRequest userRetailerRequest);

        Task<int> updatePassword(int userID, string password);

        Task<IEnumerable<AllCodeCommon>> GetCompanysList(string dCode);


        Task<int> AddCompany(UserCmpMap obj);
        Task<int> CheckCompany(string CmpID);
        Task<int> DeleteCompany(UserCmpMap obj);
        Task<int> UpdateCompany(UserCmpMap obj);
    }
}
