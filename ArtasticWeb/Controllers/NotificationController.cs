using BLL.Models;
using BLL.Serv;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtasticWeb.Controllers
{
    public class NotificationController : BaseController
    {
        private NotificationService notificationService;
        public NotificationController(ArtasticContext db) : base(db)
        {
            notificationService = new NotificationService(_uw);
        }

        public async Task<ResponseContext> Pull()
        {
            ResponseContext responseContext = new ResponseContext();
            try
            {
                Request.Headers.TryGetValue("username", out StringValues username);
                if (!StringValues.IsNullOrEmpty(username))
                {
                    var n = await notificationService.PullNotification(username);
                    responseContext.notification = n.ToList();
                }
            }
            catch (Exception e)
            {
                responseContext.error = true;
            }
            return responseContext;
        }

        public async Task<ResponseContext> Fetch()
        {
            ResponseContext responseContext = new ResponseContext();
            try
            {
                Request.Headers.TryGetValue("username", out StringValues username);
                if (!StringValues.IsNullOrEmpty(username))
                {
                    var n = await notificationService.CountNotification(username);
                    responseContext.notifyNum = n;
                }
            }
            catch (Exception e)
            {
                responseContext.error = true;
            }
            return responseContext;
        }
    }
}
