using Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Model.IRepos
{
    public interface IArtdata : IRepository<artdata>
    {
        Task<string> GetUrlById(long artworkId);

    }
}
