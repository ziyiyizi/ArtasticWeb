﻿using Model.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Model.IRepos
{
    public interface IArtwork : IRepository<artworks>
    {
        Task<IEnumerable<artworks>> GetAllByArtistId(long artistId);
        Task<artworks> GetById(long artworkId);
        Task<string> GetNameById(long artworkId);

        Task<IEnumerable<artworks>> GetAllPage(PageRequest page);

        Task<IEnumerable<artworks>> GetAllPageTimeSort(PageRequest page);
        Task<IEnumerable<long>> GetIdAllPageCommentedSort(PageRequest page);
        Task<IEnumerable<long>> GetIdAllPageCommentedSortBetweenTime(PageRequest page, DateTime start, DateTime end);
        Task<IEnumerable<long>> GetIdAllPageLikedSort(PageRequest page);
        Task<IEnumerable<long>> GetIdAllPageLikedSortBetweenTime(PageRequest page, DateTime start, DateTime end);
        Task<IEnumerable<artworks>> GetAllPageRandSort(PageRequest page);
        Task<IEnumerable<artworks>> GetFollowing(long userId, PageRequest page);

        Task<IEnumerable<artworks>> GetAllPageByName(string artworkName, PageRequest page);
        Task<IEnumerable<artworks>> GetAllPageByUserName(string userName, PageRequest page);
        Task<IEnumerable<artworks>> GetAllPageByUserNameEX(string userName, PageRequest page);
        Task<IEnumerable<artworks>> GetAllPageByTag(string tagName, PageRequest page);
        Task<IEnumerable<artworks>> GetAllPageByTagEX(string tagName, PageRequest page);
    }
}
