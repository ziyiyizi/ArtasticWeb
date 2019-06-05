using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Model.Models;

namespace ArtasticWeb.Controllers
{
    [EnableCors("allallow")]
    public class BaseController : Controller
    {
        protected readonly ArtasticContext _db;
        protected UnitOfWork _uw;
        public BaseController(ArtasticContext db)
        {
            _db = db;
            _uw = new UnitOfWork(_db);
        }

    }
}