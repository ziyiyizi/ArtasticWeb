using Model.IRepos;
using Model.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repos
{
    public class CommentRepository : RepositoryBase<comments>, IComment
    {
        public CommentRepository(ArtasticContext context) : base(context)
        {

        }

        //public async Task<IEnumerable<comments>> GetAllPageById(long artworkId)
        //{
        //    return await GetAllList(e => e.Artwork_ID == artworkId, e => e.Comment_time);
        //}

        public async Task<IList> GetAllPageById(long artworkId)
        {
            return await (from u in _dbcontext.comments join ru in _dbcontext.users
                          on u.Commentor_Name equals ru.User_name
                          where u.Artwork_ID == artworkId
                          select new
                          {
                              userName = u.User_Name,
                              comment = u.Comment,
                              commentTime = u.Comment_time,
                              commentorName = u.Commentor_Name,
                              userIcon = ru.User_icon

                          }).ToListAsync();
        }
    }
}
