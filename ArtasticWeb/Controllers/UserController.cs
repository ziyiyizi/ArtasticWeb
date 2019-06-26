using BLL.Models;
using BLL.Serv;
using BLL.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Model;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtasticWeb.Controllers
{
    public class UserController : BaseController
    {
        private UserService userService;
        private UploadService uploadService;
        public UserController(ArtasticContext context) : base(context)
        {
            userService = new UserService(_uw, context);
            uploadService = new UploadService(_uw, context);
        }

        [HttpPost]
        public async Task<JsonResult> Register([FromBody] Params p)
        {
            try
            {
                if (await userService.IsNameOrMailExists(p.userName, p.email))
                {
                    p.error = true;
                    p.errorMsg = ("The name or email is already exists!");
                    return new JsonResult(p);
                }
                var user = await userService.Register(p);
                if (user == null)
                {
                    p.error = true;
                }
                else
                {
                    p.userId = user.User_ID;
                }
            }
            catch (Exception e)
            {
                p.error = true;
            }
            return new JsonResult(p);
        }

        [HttpPost]
        public async Task<JsonResult> UploadProfile()
        {
            ResponseContext responseContext = new ResponseContext();
            try
            {
                Request.Headers.TryGetValue("userid", out StringValues idstring);
                if (StringValueUtils.IsNullOrEmpty(idstring))
                {
                    responseContext.error = true;
                    responseContext.errorMsg = "Please Sign In First!";
                }
                long userId = long.Parse(idstring);
                Request.Form.TryGetValue("userSex", out StringValues sex);
                Request.Form.TryGetValue("userDescription", out StringValues desc);
                Request.Form.TryGetValue("userMail", out StringValues mail);
                Request.Form.TryGetValue("userPassword", out StringValues pwd);
                var file = Request.Form.Files.GetFile("file");
                if (file != null)
                {
                    await uploadService.UploadIcon(file.OpenReadStream(), file.FileName, userId);
                }
                await userService.UpdateUserOne(new users
                {
                    User_ID = userId,
                    User_sex = sex,
                    User_description = desc,
                    User_mail = mail,
                    User_password = pwd
                });
            }
            catch (Exception e)
            {
                responseContext.error = true;
            }
            return new JsonResult(responseContext);
        }

        [HttpPost]
        public async Task<JsonResult> Login([FromBody] Params p)
        {
            try
            {
                users user = await userService.Login(p.userName, p.password);
                if (user != null)
                {
                    p.iconURL = user.User_icon;
                    p.userId = user.User_ID;
                    p.password = "";
                }
                else
                {
                    p.error = true;
                }
            }
            catch (Exception e)
            {
                p.error = true;
            }
            return new JsonResult(p);
        }

        public async Task<JsonResult> GetMemberDetails()
        {
            ResponseContext responseContext = new ResponseContext();
            try
            {
                UserDetails userDetails = null;
                Request.Headers.TryGetValue("present", out StringValues present);
                Request.Headers.TryGetValue("userName", out StringValues userName);
                string[] strings = present.ToString().Split("/");
                string searchType = strings[1];
                string searchKey = strings[2];
                string searchUser = searchType.Equals("member") ? searchKey : userName.ToString();

                if (!StringValueUtils.IsNullOrEmpty(searchUser))
                {
                    PageRequest page = new PageRequest(20, 0);
                    userDetails = await userService.GetUserDetailsOneAsync(searchUser, page);
                }
                if (userDetails != null)
                {
                    Request.Headers.TryGetValue("userId", out StringValues userId);
                    if (!StringValueUtils.IsNullOrEmpty(userId))
                    {
                        userDetails.follow = await userService.IsFollow(long.Parse(userId), userDetails.artistId);
                    }
                }

                responseContext.member = userDetails ?? new UserDetails();

            }
            catch (Exception e)
            {
                responseContext.error = true;
            }

            return new JsonResult(responseContext);
        }

        public  async Task<JsonResult> GetUser()
        {
            users user = null;
            try
            {
                Request.Headers.TryGetValue("userid", out StringValues userid);
                user = await userService.GetUserOne(long.Parse(userid));
            }
            catch (Exception e)
            {

            }

            return user == null ? new JsonResult(new User())
                : new JsonResult(new User
                {
                    userName = user.User_name,
                    userSex = user.User_sex,
                    userDescription = user.User_description,
                    userIcon = user.User_icon,
                    userMail = user.User_mail,
                    userAge = user.User_age,
                    userPassword = user.User_password
                }
                );
        }

        public async Task<JsonResult> Follow()
        {
            ResponseContext responseContext = new ResponseContext();
            try
            {
                Request.Headers.TryGetValue("present", out StringValues artistName);
                Request.Headers.TryGetValue("userid", out StringValues userid);
                if (StringValueUtils.IsNullOrEmpty(userid) || StringValueUtils.IsNullOrEmpty(artistName))
                {
                    return new JsonResult(responseContext);
                }
                await userService.Follow(artistName, long.Parse(userid));
            }
            catch (Exception e)
            {
                responseContext.error = true;
            }

            return new JsonResult(responseContext);
        }
    }
}
