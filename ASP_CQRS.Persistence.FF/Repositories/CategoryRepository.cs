using ASP_CQRS.Application;
using ASP_CQRS.Application.Common;
using ASP_CQRS.Application.Contracts.Persistence;
using ASP_CQRS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_CQRS.Persistence.FF.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ASP_CQRSContext dbContext): base(dbContext) { }

        public async Task<List<Category>> GetCategoriesWithPost(SearchCategoryOptionsEnum searchCategory)
        {
            var allCategories = await _dbContext.Categories.Include(p => p.Posts).ToListAsync();

            if (searchCategory == SearchCategoryOptionsEnum.FirstBestOallTheTime)
            {

                foreach (var c in allCategories)
                {
                    Post max = null;
                    foreach (var p in c.Posts)
                    {
                        if (max == null)
                        {
                            max = p;
                            break;
                        }

                        if (max.Rate < p.Rate)
                            max = p;

                    }
                    c.Posts = new List<Post>();
                    if (max != null)
                        c.Posts.Add(max);
                }

                return allCategories;
            }
            else if (searchCategory == SearchCategoryOptionsEnum.FirstBestThisMonth)
            {
                DateTime d = DateTime.Now;

                allCategories = allCategories.Where(c =>
                c.Posts.Any(p => (p.Date.Month == d.Month && d.Year == p.Date.Year)))
                    .ToList();

                foreach (var c in allCategories)
                {
                    Post max = null;
                    foreach (var p in c.Posts)
                    {
                        if (max == null)
                        {
                            max = p;
                            break;
                        }

                        if (max.Rate < p.Rate)
                            max = p;

                    }
                    c.Posts = new List<Post>();
                    if (max != null)
                        c.Posts.Add(max);
                }

                return allCategories;
            }

            return allCategories;
        }
    }
}
