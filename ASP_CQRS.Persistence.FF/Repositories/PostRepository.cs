using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASP_CQRS.Application.Contracts.Persistence;
using ASP_CQRS.Domain.Entities;

namespace ASP_CQRS.Persistence.FF.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(ASP_CQRSContext dbContext) : base(dbContext) { }
        public Task<bool> IsNameAndAuthorAlreadyExist(string title, string author)
        {
            var matches = _dbContext.Posts.Any(a => a.Title.Equals(title) && a.Author.Equals(author));
            return Task.FromResult(matches);
        }
    }
}
