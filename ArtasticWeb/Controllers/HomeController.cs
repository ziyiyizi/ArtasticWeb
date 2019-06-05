﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using DAL.Repos;
using ArtasticWeb.Models;

namespace ArtasticWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ArtasticContext _db;

        public HomeController(ArtasticContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
