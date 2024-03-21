using Dapper;
using DapperAPI.Data;
using DapperAPI.dto;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DapperAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _connection;
        public UserRepository(IDbConnection dbConnection, IConfiguration configuration)
        {
            _connection = dbConnection;
            _configuration = configuration;
        }
        public IDbConnection CreateConnection()
        {
            string connectionstring = _connection.ConnectionString;

            if (string.IsNullOrEmpty(connectionstring))
            {
                throw new System.ArgumentException($"'{nameof(connectionstring)}' cannot be null or empty.", nameof(connectionstring));
            }

            IDbConnection connection = new SqlConnection(connectionstring);
            connection.Open();
            return connection;
        }
        public UserResponse AddUser(User User)
        {
            //User.UserId = new();
            var sql = "INSERT INTO Users (UserId, UserTitle, UserPrice, UserAuthor, UserPublication) VALUES (@UserId, @UserTitle, @UserPrice, @UserAuthor, @UserPublication)";
            using (var connection = CreateConnection())
            {
                return connection.QueryFirstOrDefault(sql, User);
            }
        }
        public void DeleteUser(int id)
        {
            var sql = "DELETE FROM Users WHERE UserId = @UserId";
            _connection.Execute(sql, new { UserId = id });
        }
        public IEnumerable<UserResponse> GetAllUsers()
        {
            //return _connection.Query<User>("SELECT * FROM User");
            using (var connection = CreateConnection())
            {
                return connection.Query<UserResponse>("SELECT * FROM Users");
            }

        }
        public async Task<IEnumerable<AllCodeCommon>> GetCompanysByDistributor(string dCode)
        {
            string imgurl = _configuration.GetValue<string>("imgurl");
            string query = "SELECT pcm.[id] as codeID ,pcm.[prod_company_name] as codeName , " +
                "case when isnull(pcm.imgname,'') = '' then '' else '" + imgurl + "'+'/ProductCompanyLogo/'+pcm.imgname end  CompanyLogo " +
                "FROM [ProducatCompanyMst] pcm with(nolock) " +
                //"left join ProducatCompanyMst pc with(nolock) " +
                //"on pc.company_code = pcm.company_code and pc.prod_company_name = pcm.prod_company_name  " +
                " where cancel_flag = 'N' and pcm.[company_code] = @distributorCode";
            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<AllCodeCommon>(query, new { distributorCode = dCode });
            }
        }
        public UserResponse GetUser(int id)
        {
            return _connection.QuerySingleOrDefault<UserResponse>("SELECT * FROM Users WHERE UserId = @UserId", new { UserId = id });
        }

        public object RetailerRegistration(UserRetailerRequest user)
        {
            //User.UserId = new();
            var sql = "INSERT INTO Users ( [username],[password],[FirstName],[Email],[MobileNumber],[Address1],[City],[State],[Country],[PinCode],[GSTNo],distributorCode,[RoleId])" +
                        "values(@username,@password,@companyName,@Email,@MobileNumber,@companyAddress,@City,@State,'India','123456',@gst,@distributorCode,'Retailer')";

            using (var connection = CreateConnection())
            {
                return connection.Query(sql,
                    new
                    {
                        username = user.username,
                        password = user.password,
                        companyName = user.companyName,
                        Email = user.Email,
                        MobileNumber = user.MobileNumber,
                        companyAddress = user.companyAddress,
                        City = user.city,
                        State = user.state,
                        gst = user.GST,
                        distributorCode = user.DistributorCode
                    });
            }
        }

        public UserResponse UpdateUser(User User)
        {
            var sql = "UPDATE User SET UserId = @UserId, UserTitle = @UserTitle, UserPrice = @UserPrice, UserAuthor = @UserAuthor, UserPublication = @UserPublication WHERE UserId = @UserId";
            using (var connection = CreateConnection())
            {
                return connection.QueryFirstOrDefault(sql, User);
            }
        }

        userLoginResponse IUserRepository.CheckLogin(UserLoginRequest user)
        {
            try
            {
                string imgurl = _configuration.GetValue<string>("imgurl");
                string query = "SELECT [userid] ,[username] ,u.[password] ," +
                        "[FirstName] ,[MiddleName] ,[LastName] [Email] ,[MobileNumber] ,[Address1] ,[Address2] ,[Area] ,[City] ,[State] " +
                        ",[Country] ,[PinCode] ,[GSTNo] ,[RoleId] ,[distributorCode] as sellerID " +
                        ",company_name as CompanyName ,  case when isnull(company_img,'') = '' then '' else '" + imgurl + "'+'/CompanyLogo/'+ company_img end  CompanyLogo " +
                        "FROM Users u inner join company_mst with(nolock) on company_code = u.[distributorCode]" +
                        " WHERE username = @username " +
                        " and u.password =@password";
                using (var connection = CreateConnection())
                {
                    return connection.QuerySingleOrDefault<userLoginResponse>(query, new { username = user.username, password = user.password });
                }
            }
            catch (Exception ex)
            {
                new userLoginResponse().error = ex.Message + ex.InnerException;
            }
            return new userLoginResponse();

        }

        IEnumerable<UserResponse> IUserRepository.GetAllFactoryDistributorWise(int DistributorID)
        {
            using (var connection = CreateConnection())
            {
                return connection.Query<UserResponse>("SELECT * FROM Users u inner join  DistributorFactMap dfm on dfm.FactoryID = u.userid where dfm.DistributorID=", new { UserId = DistributorID });
            }
        }

        IEnumerable<UserResponse> IUserRepository.GetAllRoleWise(int RoleId)
        {
            using (var connection = CreateConnection())
            {
                return connection.Query<UserResponse>("SELECT * FROM Users where roleid=", new { UserId = RoleId });
            }

        }

        public async Task<int> updatePassword(int userID, string password)
        {
            //User.UserId = new();
            var sql = "update Users set password = @password where userid = @userid";
            using (var connection = CreateConnection())
            {
                var result = await connection.QueryAsync<int>(sql,
                    new
                    {
                        userid = userID,
                        password = password,
                    });
                return 1;
            }
        }


        //public async Task<IEnumerable<AllCodeCommon>> FindByEmail(string dCode)
        //{
        //    string query = "SELECT [id] as codeID ,[prod_company_name] as codeName FROM [product_company_mst] with(nolock)  where [company_code] = @distributorCode";
        //    using (var connection = CreateConnection())
        //    {
        //        return await connection.QueryAsync<AllCodeCommon>(query, new { distributorCode = dCode });
        //    }
        //}

        public async Task<userLoginResponse> FindByEmail(string email)
        {
            userLoginResponse obj = new userLoginResponse();
            using (var connection = CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<userLoginResponse>("SELECT * FROM Users WHERE Email = @Email", new { Email = email });
            }
        }

        public async Task<IEnumerable<AllCodeCommon>> GetCompanysList(string UserID)
        {
            string imgurl = _configuration.GetValue<string>("imgurl");
            string query = " select CmpID as CodeID,company_name as CodeName , " +
                " case when isnull(company_img,'') = '' then '' else '" + imgurl + "'+'/CompanyLogo/'+ company_img end  CompanyLogo " +
                "from [AppCmpMap] inner join company_mst cm with(nolock) on cm.company_code = CmpID where isActive = 1 and AppUserID = @UserID ";
            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<AllCodeCommon>(query, new { UserID = UserID });
            }
        }
        async Task<int> IUserRepository.AddCompany(UserCmpMap obj)
        {
            try
            {


                var sql = "" +
                    " if not exists(select 1 from AppCmpMap where AppUserID = @AppUserID and CmpID = @CmpID) " +
                    " begin " +
                        " INSERT INTO AppCmpMap ( [AppUserID],[CmpID],[isActive]) " +
                        " values(@AppUserID,@CmpID,1) " +
                    " end " +
                    " else " +
                    " begin " +
                    " update AppCmpMap set [isActive] = 1 where AppUserID  = @AppUserID and CmpID = @CmpID " +
                    " end ";

                using (var connection = CreateConnection())
                {

                    var result = await connection.QueryAsync<int>(sql,
                        new
                        {
                            AppUserID = obj.AppUserID,
                            CmpID = obj.CmpID
                        });
                    return 1;
                }
            }
            catch (Exception ex)
            {

                return 0;
            }
        }


        async Task<int> IUserRepository.DeleteCompany(UserCmpMap obj)
        {
            try
            {
                var sql = "update AppCmpMap set [isActive] = 0 where AppUserID  = @AppUserID and CmpID = @CmpID ";

                using (var connection = CreateConnection())
                {

                    var result = await connection.QueryAsync<int>(sql,
                        new
                        {
                            AppUserID = obj.AppUserID,
                            CmpID = obj.CmpID
                        });
                    return 1;
                }
            }
            catch (Exception ex)
            {

                return 0;
            }
        }

        async Task<int> IUserRepository.UpdateCompany(UserCmpMap obj)
        {
            try
            {
                var sql = "update AppCmpMap set [isActive] = 1 where AppUserID  = @AppUserID and CmpID = @CmpID ";

                using (var connection = CreateConnection())
                {

                    var result = await connection.QueryAsync<int>(sql,
                        new
                        {
                            AppUserID = obj.AppUserID,
                            CmpID = obj.CmpID
                        });
                    return 1;
                }
            }
            catch (Exception ex)
            {

                return 0;
            }
        }

        async Task<int> IUserRepository.CheckCompany(string CmpID)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    var result = await connection.QueryAsync<int>("SELECT 1 FROM company_mst WHERE company_code = @CmpID", new { CmpID = CmpID });
                    if (result != null && result.FirstOrDefault() > 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
