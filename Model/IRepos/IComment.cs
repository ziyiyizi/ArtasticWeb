using Model.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Model.IRepos
{
    public interface IComment : IRepository<comments>
    {
        Task<IList> GetAllPageById(long artworkId);
    }
}
