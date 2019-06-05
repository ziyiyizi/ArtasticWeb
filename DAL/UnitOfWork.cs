using DAL.Repos;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UnitOfWork
    {
        private readonly ArtasticContext _context = null;
        private UserRepository _userRepository = null;
        private TagRepository _tagRepository = null;
        private ArtworkRepository _artworkRepository = null;
        private ClickRepository _clickRepository = null;
        private CommentRepository _commentRepository = null;
        private FollowRepository _followRepository = null;
        private LikeRepository _likeRepository = null;
        private NotificationRepository _notificationRepository = null;

        public UnitOfWork(ArtasticContext context)
        {
            _context = context;
        }
        public UserRepository UserRepository
        {
            get { return _userRepository ?? (_userRepository = new UserRepository(_context)); }
        }
        public TagRepository TagRepository
        {
            get { return _tagRepository ?? (_tagRepository = new TagRepository(_context)); }
        }
        public ArtworkRepository ArtworkRepository
        {
            get { return _artworkRepository ?? (_artworkRepository = new ArtworkRepository(_context)); }
        }
        public ClickRepository ClickRepository
        {
            get { return _clickRepository ?? (_clickRepository = new ClickRepository(_context)); }
        }
        public CommentRepository CommentRepository
        {
            get { return _commentRepository ?? (_commentRepository = new CommentRepository(_context)); }
        }
        public FollowRepository FollowRepository
        {
            get { return _followRepository ?? (_followRepository = new FollowRepository(_context)); }
        }
        public LikeRepository LikeRepository
        {
            get { return _likeRepository ?? (_likeRepository = new LikeRepository(_context)); }
        }
        public NotificationRepository NotificationRepository
        {
            get { return _notificationRepository ?? (_notificationRepository = new NotificationRepository(_context)); }
        }
        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
