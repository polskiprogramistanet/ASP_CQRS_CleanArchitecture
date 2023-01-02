using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASP_CQRS.Application.Common;
using ASP_CQRS.Application.Contracts.Persistence;
using ASP_CQRS.Domain.Entities;

namespace ASP_CQRS.Persistence.FF.Repositories
{
    public class WebinarRepository : BaseRepository<Webinar>, IWebinaryRepository
    {
        public WebinarRepository(ASP_CQRSContext dbContext) : base(dbContext) { }
        public async Task<List<Webinar>> GetPagedWebinarsForDate(SearchOptionsWebinarsEnum options, int page, int pageSize, DateTime? date)
        {
            if (options == SearchOptionsWebinarsEnum.MonthAndYear && date.HasValue)
            {
                return await _dbContext.Webinars.Where(x => x.Date.Month == date.Value.Month && x.Date.Year == date.Value.Year)
                    .Skip((page - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
            }
            if (options == SearchOptionsWebinarsEnum.Year && date.HasValue)
            {
                return await _dbContext.Webinars.Where(x => x.Date.Year == date.Value.Year)
                    .Skip((page - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
            }
            if (options == SearchOptionsWebinarsEnum.Month && date.HasValue)
            {
                return await _dbContext.Webinars.Where(x => x.Date.Month == date.Value.Month)
                    .Skip((page - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
            }


            return await _dbContext.Webinars
                .Skip((page - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
        }
        public async Task<int> GetTotalCountOfWebinarsForDate(SearchOptionsWebinarsEnum options, DateTime? date)
        {
            if (options == SearchOptionsWebinarsEnum.MonthAndYear && date.HasValue)
            {
                return await _dbContext.Webinars.CountAsync
                                (x => x.Date.Month == date.Value.Month && x.Date.Year == date.Value.Year);
            }
            if (options == SearchOptionsWebinarsEnum.Year && date.HasValue)
            {
                return await _dbContext.Webinars.CountAsync
                   (x => x.Date.Year == date.Value.Year);
            }
            if (options == SearchOptionsWebinarsEnum.Month && date.HasValue)
            {
                return await _dbContext.Webinars.CountAsync
                  (x => x.Date.Year == date.Value.Year);
            }

            return await _dbContext.Webinars.CountAsync();
        }
    }
}
