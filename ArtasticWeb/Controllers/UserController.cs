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
        public UserController(ArtasticContext context) : base(context)
        {
            userService = new UserService(_uw, context);
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

            return user == null ? new JsonResult(new users()) : new JsonResult(user);
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

            }

            return new JsonResult(responseContext);
        }
    }
}
