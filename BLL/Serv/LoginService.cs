using DAL;
using DAL.Repos;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Serv
{
    public class LoginService
    {
        private UnitOfWork _uw;

        public LoginService(UnitOfWork uw)
        {
            _uw = uw;
        }

        public async Task<users> Login(string name, string pwd)
        {
            users user =await _uw.UserRepository.GetByName(name);
            return user.User_password == pwd ? user : null;

        }
    }
}
