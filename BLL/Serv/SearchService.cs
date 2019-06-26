using BLL.Models;
using DAL;
using Model;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Serv
{
    public class SearchService
    {
        private UnitOfWork _uw;

        public SearchService(UnitOfWork uw)
        {
            _uw = uw;
        }

        public async Task<IEnumerable<artworks>> Search(string type, string key, int pageNo)
        {
            IEnumerable<artworks> _artworks = null;
            if (String.IsNullOrEmpty(type) || String.IsNullOrEmpty(key))
            {
                return _artworks;
            }
            PageRequest page = new PageRequest(12, pageNo);
            if (type.Equals("thismember"))
            {
                _artworks = await _uw.ArtworkRepository.GetAllPageByUserNameEX(key, page);
            }
            else if (type.Equals("thistag"))
            {
                _artworks = await _uw.ArtworkRepository.GetAllPageByTagEX(key, page);
            }
            else if (type.Equals("tag"))
            {
                _artworks = await _uw.ArtworkRepository.GetAllPageByTag(key, page);
            }
            else
            {
                _artworks = await _uw.ArtworkRepository.GetAllPageByName(key, page);
            }
            return _artworks ?? new List<artworks>();

        }
    }
}
