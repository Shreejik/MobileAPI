using Dapper;
using DapperAPI.Data;
using DapperAPI.dto;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DapperAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _connection;
        public OrderRepository(IDbConnection dbConnection, IConfiguration configuration)
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
        public async Task<int> PlaceOrder(OrdersReq odrReq)
        {
            int orderID = 0, orderIDItem = 0;
            string query = " declare @invNo int =0; select @invNo = max(invno) + 1 from user_mst where company_code = @SellerID ; " +
                " update user_mst set invno = invno + 1 where company_code = @SellerID ; " +
                " insert into orders (Buyer,Seller,OrderDate,orderstatus,TotalAmount,InvoiceNo,Oldamount)values(@Buyer,@SellerID,getdate(),1,@TotalAmount,@invNo,@TotalAmount);SELECT CAST(SCOPE_IDENTITY() as int) ";

            using (var connection = CreateConnection())
            {
                orderID = await connection.QueryFirstOrDefaultAsync<int>(query, new { Buyer = odrReq.Buyer, SellerID = odrReq.SellerID, TotalAmount = odrReq.TotalAmount });

                query = " insert into orderitems (orderID,ProductID,Price,quantity,TotalAmount)values(@orderID,@ProductID,@Price,@Quantity,@TotalAmount) ";
                if (orderID != 0 && odrReq.Items != null)
                {
                    foreach (var item in odrReq.Items)
                    {
                        orderIDItem = await connection.QueryFirstOrDefaultAsync<int>(query, new { orderID = orderID, ProductID = item.Productid, Price = item.Price, Quantity = item.Quantity, TotalAmount = item.TotalAmount });
                    }
                }
            }
            return orderID;
        }
        public async Task<IEnumerable<OrderSearchResp>> getOrdersByBuyerID(OrderSearchReq req)
        {
            string query = "select orderid, orderstatus, " +
                " case when orderstatus = 1 then 'Pending' " +
                " when orderstatus = 2 then 'Confirm' " +
                " when orderstatus = 3 then 'Rejected' " +
                " else '' end  orderStatusStr, orderdate, " +
                " isnull(TotalAmount, 0) TotalAmount, ltrim(rtrim(isnull(InvoiceNo,''))) InvoiceNo," +
                " isnull(Percentage ,0) Percentage , isnull(Oldamount,0) Oldamount  " +
                " from Orders where Buyer = @ID and Seller = @SellerID " +
                " order by orderdate desc  OFFSET @offset Rows " +
                " fetch next @PageSize Rows Only ";
            using (var connection = CreateConnection())
            {
                IEnumerable<OrderSearchResp> result = await connection.QueryAsync<OrderSearchResp>(query,
                    new
                    {
                        searchstr = req.Criteria,
                        ID = req.ID,
                        SellerID = req.SellerID,
                        offset = (req.PageNumber - 1) * req.PageSize,
                        PageSize = req.PageSize
                    });
                return result;
            }
        }
        public async Task<OrderDetails> getOrderFullByBuyerID(OrderSearchReq req)
        {
            OrderDetails obj = null;
            string imgurl = _configuration.GetValue<string>("imgurl");
            string query = " select orderid, orderstatus, " +
                " case when orderstatus = 1 then 'Pending' " +
                " when orderstatus = 2 then 'Confirm' " +
                " when orderstatus = 3 then 'Rejected' " +
                " else '' end  orderStatusStr, orderdate, " +
                " isnull(TotalAmount, 0) TotalAmount , ltrim(rtrim(isnull(InvoiceNo,''))) InvoiceNo ," +
                " isnull(Percentage ,0) Percentage , isnull(Oldamount,0) Oldamount  " +
                " from Orders where orderid = @ID " +
                " select OrderItemId,oi.ProductId,pd.product_name ProductName,pd.product_code Productcode, oi.Price,oi.Quantity, " +
                " oi.Discount,oi.TotalAmount TotalPrice, '"+imgurl + "/ProductImage/" + "' + pd.image_name as imgUrl , " +
                "case when isnull(pc.imgname,'') = '' then '' else '" + imgurl + "'+'/ProductCompanyLogo/'+pc.imgname end  CompanyLogo, " +
                " pcm.prod_company_name FactoryName, isnull(description ,'') description from orders o  " +
                " inner join orderitems oi on oi.orderid = o.orderid " +
                " inner join product_details pd on pd.id = oi.productid " +
                " inner join product_company_mst pcm on pcm.prod_company_code = pd.product_company_code and pd.product_code = pcm.prod_code  " +
                " left join ProducatCompanyMst pc with(nolock) on pc.company_code = pcm.company_code and pc.prod_company_name = pcm.prod_company_name where o.orderid = @ID ";

            using (var connection = CreateConnection())
            {
                var multi = await connection.QueryMultipleAsync(query, new { ID = req.ID });

                obj = multi.Read<OrderDetails>().Single();
                obj.items = multi.Read<OrderItems>().ToList();

                return obj;
            }
        }
    }
}
