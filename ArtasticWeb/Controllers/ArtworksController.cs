using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BLL.Models;
using BLL.Serv;
using BLL.Utils;
using DAL;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Model;
using Model.Models;

namespace ArtasticWeb.Controllers
{
    public class ArtworksController : BaseController
    {
        private ArtworkService artworkService;
        private SearchService searchService;
        private UserService userService;
        private UploadService uploadService;
        public ArtworksController(ArtasticContext context) : base(context)
        {
            artworkService = new ArtworkService(_uw, context);
            searchService = new SearchService(_uw);
            userService = new UserService(_uw, context);
            uploadService = new UploadService(_uw, context);
        }

        public async Task<JsonResult> GetWeekly()
        {
            ResponseContext responseContext = new ResponseContext();
            try
            {
                Weekly weekly = new Weekly();
                weekly.tags = await artworkService.GetPopularTags();
                var artwork = await artworkService.GetArtworkWeekly();
                if (artwork != null)
                {
                    weekly.artworkId = artwork.Artwork_ID;
                    weekly.artworkName = artwork.Artwork_name;
                    long artistId = artwork.Artist_ID;
                    weekly.fileURL = artwork.Artdata1;
                    weekly.artworkviews = await artworkService.CountClicks(artwork.Artwork_ID);
                    users user = await userService.GetUserOne(artistId);
                    if (user != null)
                    {
                        weekly.artistName = user.User_name;
                        weekly.iconURL = user.User_icon;
                        weekly.frenzy = await userService.CountFollows(artistId);
                    }
                }
                responseContext.weekly = weekly;
            }
            catch (Exception e)
            {
                responseContext.error = true;
            }
            return new JsonResult(responseContext);
        }

        public async Task<JsonResult> GetRecommendTags()
        {
            ResponseContext responseContext = new ResponseContext();
            try
            {
                Request.Headers.TryGetValue("present", out StringValues key);
                var values = await artworkService.GetSimilarTags(key);
                if (StringValueUtils.IsNullOrEmpty(values.First()))
                {
                    values.Add("unknown");
                }
                responseContext.values = values;
            }
            catch (Exception e)
            {
                responseContext.error = true;
            }
            return new JsonResult(responseContext);
        }

        [HttpPost]
        public async Task<JsonResult> Upload()
        {
            ResponseContext responseContext = new ResponseContext();
            try
            {
                Request.Headers.TryGetValue("userid", out StringValues userId);
                if (!StringValueUtils.IsNullOrEmpty(userId))
                {
                    Request.Form.TryGetValue("folders", out StringValues folders);
                    Request.Form.TryGetValue("tags", out StringValues tags);
                    Request.Form.TryGetValue("title", out StringValues title);
                    Request.Form.TryGetValue("description", out StringValues description);
                    var file = Request.Form.Files.GetFile("file");
                    if (file != null)
                    {
                        await uploadService.Upload(file.OpenReadStream(), file.FileName, title, tags, folders, description, long.Parse(userId));
                    }
                    
                }
                
            }
            catch (Exception e)
            {
                responseContext.error = true;
            }

            return new JsonResult(responseContext);
        }

        public async Task<JsonResult> GetSearch()
        {
            ResponseContext responseContext = new ResponseContext();
            try
            {
                Request.Headers.TryGetValue("present", out StringValues present);
                Request.Headers.TryGetValue("userId", out StringValues userId);
                Request.Headers.TryGetValue("page", out StringValues _page);
                int pageNo = 0;
                if (!StringValues.IsNullOrEmpty(_page))
                {
                    pageNo = int.Parse(_page);
                }
                string[] strings = present.ToString().Split("/");
                string searchType = strings[1];
                string searchKey = strings[2];
                if (searchType.Equals("member"))
                {
                    IEnumerable<UserDetails> userDetails = await userService.GetUserDetailsAsync(searchKey, new PageRequest(12, pageNo));
                    responseContext.members = userDetails.ToList();
                    return new JsonResult(responseContext);
                }
                else
                {
                    IEnumerable<artworks> _artworks = await searchService.Search(searchType, searchKey, pageNo);
                    List<ArtworkDetails> artworkDetails = new List<ArtworkDetails>();
                    if (StringValues.IsNullOrEmpty(userId))
                    {
                        foreach (var _artwork in _artworks)
                        {
                            artworkDetails.Add(await artworkService.GetArtworkDetailsOneAsync(_artwork));
                        }
                    }
                    else
                    {
                        long clientId = long.Parse(userId);
                        foreach (var _artwork in _artworks)
                        {
                            artworkDetails.Add(await artworkService.GetArtworkDetailsOneAsync(_artwork, clientId));
                        }
                    }
                    responseContext.posts = artworkDetails;
                }
            }
            catch (Exception e)
            {
                responseContext.error = true;
            }
            return new JsonResult(responseContext);
        }

        public async Task<JsonResult> Comment()
        {
            ResponseContext responseContext = new ResponseContext();
            try
            {
                
                Request.Form.TryGetValue("comment", out StringValues content);
                Request.Headers.TryGetValue("username", out StringValues sender);
                if (!StringValueUtils.IsNullOrEmpty(content) && !StringValueUtils.IsNullOrEmpty(sender))
                {
                    Request.Form.TryGetValue("artworkId", out StringValues artworkId);
                    Request.Form.TryGetValue("responseTo", out StringValues receiver);
                    await artworkService.Comment(sender, receiver, content, artworkId);
                }
                
            }
            catch (Exception e)
            {
                responseContext.error = true;
            }
            return new JsonResult(responseContext);
        }

        public async Task<JsonResult> Like()
        {
            ResponseContext responseContext = new ResponseContext();
            try
            {
                Request.Headers.TryGetValue("artworkid", out StringValues artworkId);
                Request.Headers.TryGetValue("userid", out StringValues userId);
                if (!StringValueUtils.IsNullOrEmpty(artworkId) && !StringValueUtils.IsNullOrEmpty(userId))
                {
                    await artworkService.Like(artworkId, userId);
                }
                
            }
            catch (Exception e)
            {
                responseContext.error = true;
            }
            return new JsonResult(responseContext);
        }

        public async Task<JsonResult> GetLikeAndComment()
        {
            ArtworkLikes _likes = new ArtworkLikes();
            try
            {
                Request.Headers.TryGetValue("artworkid", out StringValues artworkId);
                Request.Headers.TryGetValue("userid", out StringValues userId);
                if (!StringValueUtils.IsNullOrEmpty(artworkId))
                {
                    _likes = await artworkService.GetArtworkLikesAsync(artworkId);
                    _likes = _likes ?? new ArtworkLikes();
                    await artworkService.Click(artworkId, userId);
                }     
            }
            catch (Exception e)
            {

            }
            return new JsonResult(_likes);
        }

        public async Task<JsonResult> GetPost()
        {
            ResponseContext responseContext = new ResponseContext();
            try
            {
                ArtworkDetails artworkDetails = new ArtworkDetails();
                Request.Headers.TryGetValue("present", out StringValues artworkId);
                Request.Headers.TryGetValue("userId", out StringValues userId);
                if (StringValues.IsNullOrEmpty(userId))
                {
                    artworkDetails = await artworkService.GetArtworkDetailsOneAsync(long.Parse(artworkId));
                }
                else
                {
                    artworkDetails = await artworkService.GetArtworkDetailsOneAsync(long.Parse(artworkId), long.Parse(userId));
                }
                responseContext.post = artworkDetails ?? new ArtworkDetails();
            }
            catch (Exception e)
            {
                responseContext.error = true;
            }
            return new JsonResult(responseContext);
        }

        public async Task<JsonResult> GetPosts()
        {
            ResponseContext responseContext = new ResponseContext();
            try
            {
                int PageNo = 0;
                Request.Headers.TryGetValue("page", out StringValues PageStr);
                if (!StringValues.IsNullOrEmpty(PageStr))
                {
                    PageNo = int.Parse(PageStr.ToString());
                }
                PageRequest pr = new PageRequest(10, PageNo);
                Request.Headers.TryGetValue("present", out StringValues TypeSort);
                Request.Headers.TryGetValue("userid", out StringValues IdStr);
                var details = await artworkService.GetArtworkDetailsAsync(TypeSort, IdStr, pr);
                responseContext.posts = details ?? new List<ArtworkDetails>();
            }
            catch (Exception e)
            {
                responseContext.error = true;
            }
            return new JsonResult(responseContext);
        }
    }
}
