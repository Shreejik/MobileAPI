using API;
using DapperAPI.dto;
using DapperAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Buffers.Text;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;

namespace DapperAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        public JwtService JwtService { get; }
        public UserController(IUserService userService, JwtService jwtService)
        {
            _userService = userService;
            JwtService = jwtService;
        }

        //[HttpGet]

        //public IActionResult GetAllUsers() {

        //    return Ok(_userService.GetAllUsers());
        //}

        //[HttpGet("id")]

        //public IActionResult GetUser(int id)
        //{
        //    return Ok(_userService.GetUser(id));
        //}

        [HttpPost("CheckLogin")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckLogin(UserLoginRequest user)
        {
            userLoginResponse userlogin = await Task.FromResult(_userService.CheckLogin(user));
            try
            {
                if (userlogin !=null &&  userlogin.error == null)
                {
                    userlogin.Token = JwtService.GenerateToken(userlogin);
                    return Ok(userlogin);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { status="500",message = ex.Message });
            }
            return BadRequest(new { status="401",message = "Username or password is wrong" });
        }

        [HttpPost("RetailerRegistration")]
        [AllowAnonymous]
        public async Task<IActionResult> RetailerRegistration(UserRetailerRequest user)
        {
            try
            {
                //check user already exists return user already exists
                int status = await _userService.RetailerRegistration(user);
                if (status == 0)
                {
                    return BadRequest(new { status = "302", message = "Email Alerady Exist" });
                }
                else if (status == 1)
                {
                    return Ok(new {status="200" ,message= "User Created Successfully"}); //(nameof(UserRetailerRequest), user);
                }
            }
            catch (Exception ex)
            {
                return  BadRequest (new { status = StatusCodes.Status500InternalServerError, message = ex.Message });
            }
            return BadRequest();
        }
        
        [HttpGet("getEmailOTP")]
        [AllowAnonymous]
        public IActionResult getEmailOTP(string email)
        {
            string emailaddr = "shreejiinfoservices@gmail.com", password = "fxie hnpd bovu hiws";
            string message = new Random().Next(99999, 999999).ToString();

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailaddr);
                mail.To.Add(email);
                mail.Subject = "One Time Password for signup - Shreeji Info Services";
                mail.Body = "<div style=\"font-family: Helvetica,Arial,sans-serif;min-width:1000px;overflow:auto;line-height:2\">\r\n  <div style=\"margin:50px auto;width:70%;padding:20px 0\">\r\n    <div style=\"border-bottom:1px solid #eee\">\r\n      <a href=\"\" style=\"font-size:1.4em;color: #00466a;text-decoration:none;font-weight:600\">Shreeji info Services</a>\r\n    </div>\r\n    <p style=\"font-size:1.1em\">Hi,</p>\r\n    <p>Thank you for choosing Shreeji info services. Use the following OTP to complete your Sign Up procedures. OTP is valid for 5 minutes</p>\r\n    <h2 style=\"background: #00466a;margin: 0 auto;width: max-content;padding: 0 10px;color: #fff;border-radius: 4px;\">"+ message + "</h2>\r\n    <p style=\"font-size:0.9em;\">Regards,<br />Shreeji info services</p>\r\n    <hr style=\"border:none;border-top:1px solid #eee\" />\r\n    <div style=\"float:right;padding:8px 0;color:#aaa;font-size:0.8em;line-height:1;font-weight:300\">\r\n      <p>Shreeji info services </p>\r\n</div>\r\n  </div>\r\n</div>";
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(emailaddr, password);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }

            return Ok(new { status = "200", message , email = email });
        }


        [HttpGet("GetCompanysByDistributor")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCompanysByDistributor(string distributorCode)
        {
            return Ok(await _userService.GetCompanysByDistributor(distributorCode));
        }


        [HttpGet("GetCompanysList")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCompanysList(string UserID)
        {
            return Ok(await _userService.GetCompanysList(UserID));
        }

        [HttpPost("updatePassword")]
        [AllowAnonymous]
        public async Task<IActionResult> updatePassword(clsUserPassword obj)
        {
            await _userService.updatePassword(obj.userId,obj.Password);
            return Ok(new { status = "200", message = "Updated Password Successfully"});
        }

        [HttpGet("FindByEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> FindByEmail(string email)
        {
            var result = await _userService.FindByEmail(email);
            string emailaddr = "shreejiinfoservices@gmail.com", password = "fxie hnpd bovu hiws";
            string message = new Random().Next(99999, 999999).ToString();

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailaddr);
                mail.To.Add(email);
                mail.Subject = "One Time Password for change Password - Shreeji Info Services";
                mail.Body = "<div style=\"font-family: Helvetica,Arial,sans-serif;min-width:1000px;overflow:auto;line-height:2\">\r\n  <div style=\"margin:50px auto;width:70%;padding:20px 0\">\r\n    <div style=\"border-bottom:1px solid #eee\">\r\n      <a href=\"\" style=\"font-size:1.4em;color: #00466a;text-decoration:none;font-weight:600\">Shreeji info Services</a>\r\n    </div>\r\n    <p style=\"font-size:1.1em\">Hi,</p>\r\n    <p>Thank you for choosing Shreeji info services. Use the following OTP to complete your change password procedures. OTP is valid for 5 minutes</p>\r\n    <h2 style=\"background: #00466a;margin: 0 auto;width: max-content;padding: 0 10px;color: #fff;border-radius: 4px;\">" + message + "</h2>\r\n    <p style=\"font-size:0.9em;\">Regards,<br />Shreeji info services</p>\r\n    <hr style=\"border:none;border-top:1px solid #eee\" />\r\n    <div style=\"float:right;padding:8px 0;color:#aaa;font-size:0.8em;line-height:1;font-weight:300\">\r\n      <p>Shreeji info services </p>\r\n</div>\r\n  </div>\r\n</div>";
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(emailaddr, password);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                    result.Token = message;
                }
            }
            return Ok(result);


        }

        [HttpPost("AddCompany")]
        [AllowAnonymous]
        public async Task<IActionResult> AddCompany(UserCmpMap user)
        {
            try
            {
                //check user already exists return user already exists
                int status = await _userService.AddCompany(user);
                if (status == 0)
                {
                    return BadRequest(new { status = "401", message = "Company not Exist" });
                }
                else if (status == 1)
                {
                    return Ok(new { status = "200", message = "Added Company Successfully" }); //(nameof(UserRetailerRequest), user);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = StatusCodes.Status500InternalServerError, message = ex.Message });
            }
            return BadRequest();
        }

        [HttpPost("DeleteCompany")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteCompany(UserCmpMap user)
        {
            try
            {
                //check user already exists return user already exists
                int status = await _userService.DeleteCompany(user);
                if (status == 1)
                {
                    return Ok(new { status = "200", message = "Deleted Company Successfully" }); 
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = StatusCodes.Status500InternalServerError, message = ex.Message });
            }
            return BadRequest();
        }

    }
}
