using Dapper;
using DapperAPI.Data;
using DapperAPI.dto;
using Microsoft.Data.SqlClient;
using NLog.Config;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace DapperAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _connection;
        public ProductRepository(IDbConnection dbConnection, IConfiguration configuration)
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
        public async Task<IEnumerable<ProductSearchResp>> Search(ProductSearchReq productSearchReq)
        {
            IEnumerable<ProductSearchResp> obj;
            string imgurl = _configuration.GetValue<string>("imgurl");

            string query = " select  p.id AS productId, p.product_name AS productname, p.product_code AS Productcode, p.unit_price AS Price" +
                " , 'http://webtest.shreejiinfoservices.com/ProductImage/' + p.image_name as imgUrl, pcm.ID as factoryID, pcm.prod_company_name as factoryName , " +
                " color , size , ltrim(rtrim(weight)) weight, '' modelYear , p.quantity, " +
                " case when isnull(pc.imgname,'') = '' then '' else 'http://webtest.shreejiinfoservices.com'+'/ProductCompanyLogo/'+pc.imgname end  CompanyLogo , isnull(description,'') as description  " +
                " From product_details p  inner join product_company_mst pcm on pcm.prod_company_code = p.product_company_code and p.product_code = pcm.prod_code " +
                " left join ProducatCompanyMst pc on pc.company_code = pcm.company_code and pc.prod_company_code = pcm.prod_company_code " +
                " where 1 = 1 and pcm.cancel_flag = p.cancel_flag  and p.user_id = @distributorCode order by p.created_date desc";


            //string query = "select p.id AS productId,p.product_name AS productname,p.product_code AS Productcode,p.unit_price AS Price " +
            //        " , '"+imgurl+ "/ProductImage/" + "' + p.image_name as imgUrl,pcm.ID as factoryID,pcm.prod_company_name as factoryName , " +
            //        "color , size , ltrim(rtrim(weight)) weight, '' modelYear ,p.quantity, "+
            //        " case when isnull(pc.imgname,'') = '' then '' else '" + imgurl + "'+'/ProductCompanyLogo/'+pc.imgname end  CompanyLogo " +
            //        ", isnull(description,'') as description " +
            //        " from product_details p with(nolock) inner join product_company_mst pcm with(nolock) on pcm.prod_company_code = p.product_company_code and p.product_code = pcm.prod_code" +
            //        //" left join ProducatCompanyMst pc with(nolock) on pc.company_code = pcm.company_code and pc.prod_company_name = pcm.prod_company_name " +
            //        " where p.user_id = @distributorCode " +
            ////and((product_name like '%"+productSearchReq.ProdCriteria+"%') or (product_code like '%" + productSearchReq.ProdCriteria + "%')) " +
            //" and  (@companyID = 0  or pcm.prod_company_name = (select prod_company_name from product_company_mst where id = @CompanyID) ) order by product_name, product_code";
            ////" order by product_name, product_code OFFSET @offset Rows "+
            ////" fetch next @PageSize Rows Only ";
            using (var connection = CreateConnection())
            {
               obj = await connection.QueryAsync<ProductSearchResp>(query,
                    new
                    {
                        distributorCode = productSearchReq.DistributorCode,
                        offset = (productSearchReq.PageNumber - 1) * productSearchReq.PageSize,
                        PageSize = productSearchReq.PageSize,
                        CompanyID = productSearchReq.CompanyID
                    });
            }
            return obj;
        }

        public async Task<IEnumerable<ProductSearchResp>> RecentByBuyerID(ProductSearchReq productSearchReq)
        {
            IEnumerable<ProductSearchResp> obj;
            string imgurl = _configuration.GetValue<string>("imgurl");
            using (var connection = CreateConnection())
            {
                string query = " select distinct p.id as productId,p.product_name as productname,p.product_code Productcode,p.unit_price Price,'0' factoryID, '" +
                    imgurl + "/ProductImage/" + "' +p.image_name imgUrl,factory.prod_company_name as factoryName ," +
                "color , size , ltrim(rtrim(weight)) weight , '' modelYear " +
                " from Orders o with(nolock)  inner join orderitems oi on oi.orderid = o.orderid " +
                " inner join product_details p with(nolock) on p.Id = oi.productid " +
                " inner join product_company_mst factory with(nolock) on factory.prod_company_code = p.product_company_code  " +
                " where Buyer = @Buyer and p.user_id = @cmpID ";// order by product_name OFFSET @offset Rows  fetch next @PageSize Rows Only ";
                obj = await connection.QueryAsync<ProductSearchResp>(query,
                        new
                        {
                            Buyer = productSearchReq.CompanyID,
                            cmpID = productSearchReq.DistributorCode,
                            offset = (productSearchReq.PageNumber - 1) * productSearchReq.PageSize,
                            PageSize = productSearchReq.PageSize
                        });
            }
            return obj;
        }

        async Task<ProductRespById> IProductRepository.GetByID(int pid)
        {
            ProductRespById obj;
            string imgurl = _configuration.GetValue<string>("imgurl");
            string query = " select  p.id AS productId, p.product_name AS productname, p.product_code AS Productcode, p.unit_price AS Price" +
                " , 'http://webtest.shreejiinfoservices.com/ProductImage/' + p.image_name as imgUrl, pcm.ID as factoryID, pcm.prod_company_name as factoryName , " +
                " color , size , ltrim(rtrim(weight)) weight, '' modelYear , p.quantity, " +
                " case when isnull(pc.imgname,'') = '' then '' else 'http://webtest.shreejiinfoservices.com'+'/ProductCompanyLogo/'+pc.imgname end  CompanyLogo , isnull(description,'') as description  " +
                " From product_details p  inner join product_company_mst pcm on pcm.prod_company_code = p.product_company_code and p.product_code = pcm.prod_code " +
                " left join ProducatCompanyMst pc on pc.company_code = pcm.company_code and pc.prod_company_code = pcm.prod_company_code " +
                " where 1 = 1 and pcm.cancel_flag = p.cancel_flag  and p.id = @pid ";
            
            using (var connection = CreateConnection())
            {
                obj = (ProductRespById)await connection.QueryFirstAsync<ProductRespById>(query,
                       new
                       {
                           pid = pid
                       });
                obj.images = (List<string>?)await connection.QueryAsync<string>(" select case when isnull(product_img,'') = '' then '' else '" + imgurl + "'+'/ProductImage/' + product_img end from ProductImageDtl where product_code = @pid", new { pid = obj.Productcode });
            }
            return obj;


        }
    }
}
