using ComicBookShared.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicBookShared.Data
{
    public class Repository
    {
        private Context _context = null;

        public Repository(Context context)
        {
            _context = context;
        }

        public IList<ComicBook> GetComicBooks()
        {
            return _context.ComicBooks
                .Include(cb => cb.Series)
                .OrderBy(cb => cb.Series.Title)
                .ThenBy(cb => cb.IssueNumber)
                .ToList();
        }

        public ComicBook GetComicBook(int id, bool includeRelatedEntities = true)
        {
            var comicBooks = _context.ComicBooks.AsQueryable();
            if (includeRelatedEntities)
            {
                comicBooks = comicBooks
                    .Include(cb => cb.Series)
                    .Include(cb => cb.Artists.Select(a => a.Artist))
                    .Include(cb => cb.Artists.Select(a => a.Role));
            }
            return comicBooks
                .Where(cb => cb.Id == id)
                .SingleOrDefault();
        }

        public ComicBookArtist GetComicBookArtist(int id)
        {
            return _context.ComicBookArtists
                .Include(cba => cba.Artist)
                .Include(cba => cba.Role)
                .Include(cba => cba.ComicBook.Series)
                .Where(cba => cba.Id == id)
                .SingleOrDefault(); ;
        }

        public void AddComicBook(ComicBook comicBook)
        {
            _context.ComicBooks.Add(comicBook);

            if (comicBook.Series != null && comicBook.Series.Id > 0)
            {
                _context.Entry(comicBook.Series).State = EntityState.Unchanged;
            }

            foreach (ComicBookArtist artist in comicBook.Artists)
            {
                if (artist.Artist != null && artist.Artist.Id > 0)
                {
                    _context.Entry(artist.Artist).State = EntityState.Unchanged;
                }

                if (artist.Role != null && artist.Role.Id > 0)
                {
                    _context.Entry(artist.Role).State = EntityState.Unchanged;
                }
            }

            _context.SaveChanges();
        }

        public void AddComicBookArtist(ComicBookArtist comicBookArtist)
        {
            _context.ComicBookArtists.Add(comicBookArtist);
            _context.SaveChanges();
        }

        public IList<Series> GetSeriesList() => _context.Series.OrderBy(s => s.Title).ToList();

        public IList<Artist> GetArtists() => _context.Artists.OrderBy(a => a.Name).ToList();

        public IList<Role> GetRoles() => _context.Roles.OrderBy(r => r.Name).ToList();

        public void UpdateComicBook(ComicBook comicBook)
        {
            _context.ComicBooks.Attach(comicBook);
            var comicBookEntry = _context.Entry(comicBook);
            comicBookEntry.State = EntityState.Modified;

            _context.SaveChanges();
        }

        public void DeleteComicBook(int id)
        {
            var comicBook = new ComicBook() { Id = id };
            _context.Entry(comicBook).State = EntityState.Deleted;

            _context.SaveChanges();
        }

        public void DeleteComicBookArtist(int id)
        {
            var comicBookArtist = new ComicBookArtist() { Id = id };
            _context.Entry(comicBookArtist).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public bool ComicBookSeriesHasIssueNumber(int id, int seriesId, int issueNumber)
        {
            return _context.ComicBooks
                .Any(cb => cb.Id != id &&
                           cb.SeriesId == seriesId &&
                           cb.IssueNumber == issueNumber);
        }

        public bool ComicBookHasArtistRoleCombination(int comicBookId, int artistId, int roleId)
        {
            return _context.ComicBookArtists
                .Any(cba => cba.ComicBookId == comicBookId &&
                            cba.ArtistId == artistId &&
                            cba.RoleId == roleId);
        }
    }
}
