using BLL.Utils;
using DAL;
using Microsoft.EntityFrameworkCore.Storage;
using Model.Models;
using OSA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Serv
{
    public class UploadService
    {
        private UnitOfWork _uw;
        private ArtasticContext _db;
        private ObjectStoreAccess osa;

        public UploadService(UnitOfWork uw, ArtasticContext db)
        {
            _uw = uw;
            _db = db;
            osa = new ObjectStoreAccess();
        }

        public async Task<int> UploadIcon(Stream stream, string fileName, long userId)
        {
            if (userId < 0)
            {
                return -1;
            }

            osa.ChangeDir(userId.ToString() + "Icon/");
            string name = osa.UploadFile(stream, fileName);
            string url = osa.GetImgUrl(name);
            return await _uw.UserRepository.UpdateIcon(new users
            {
                User_ID = userId,
                User_icon = url
            });
            //return await _uw.UserRepository.UpdateIcon(userId, url);
        }

        public async Task<int> Upload(Stream stream, string fileName, string title, string tags, string folders, string desc, long clientId)
        {
            if(clientId < 0)
            {
                return -1;
            }
            
            if (StringValueUtils.IsNullOrEmpty(title))
            {
                title = "unkonwn";
            }
            if (!StringValueUtils.IsNullOrEmpty(folders))
            {
                osa.ChangeDir(clientId.ToString() + "/" + folders + "/");
            }
            else
            {
                folders = "";
                osa.ChangeDir(clientId.ToString() + "/");
            }
            string name = osa.UploadFile(stream, fileName);
            string url = osa.GetImgUrl(name);

            if (!StringValueUtils.IsNullOrEmpty(name))
            {
                using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
                {
                    artworks _artworks = new artworks
                    {
                        Artist_ID = clientId,
                        Artwork_description = desc,
                        Artwork_dir = folders,
                        Artwork_name = title,
                        Uploadtime = DateTime.Now,
                        Artdata1 = url
                    };

                    await _uw.ArtworkRepository.Insert(_artworks);

                    long artworkId = _artworks.Artwork_ID;

                    if (!StringValueUtils.IsNullOrEmpty(tags))
                    {
                        string[] tagslist = tags.Split(",");
                        HashSet<string> tagset = new HashSet<string>();
                        foreach (string tagName in tagslist)
                        {
                            tagset.Add(tagName);
                        }
                        foreach (string tagName2 in tagset)
                        {
                            tags tag = new tags
                            {
                                Artwork_ID = artworkId,
                                Tag_name = tagName2
                            };
                            await _uw.TagRepository.Insert(tag);
                        }

                    }

                    transaction.Commit();
                }
            }

            return await _uw.SaveChanges();
        }
    }
}
