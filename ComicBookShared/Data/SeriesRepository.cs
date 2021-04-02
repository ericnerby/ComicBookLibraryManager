using ComicBookShared.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicBookShared.Data
{
    public class SeriesRepository : BaseRepository<Series>
    {
        public SeriesRepository(Context context)
            : base(context)
        { }

        public override Series Get(int id, bool includeRelatedEntities = true)
        {
            var seriesList = Context.Series.AsQueryable();

            if (includeRelatedEntities)
            {
                seriesList = seriesList
                    .Include(s => s.ComicBooks);
            }

            return seriesList
                .Where(cb => cb.Id == id)
                .SingleOrDefault();
        }

        public override IList<Series> GetList()
        {
            return Context.Series
                .OrderBy(s => s.Title)
                .ToList();
        }

        public bool SeriesHasTitle(int seriesId, string seriesTitle)
        {
            return Context.Series
                .Any(s => s.Id != seriesId && s.Title == seriesTitle);
        }
    }
}
