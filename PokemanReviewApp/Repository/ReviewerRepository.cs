using Microsoft.EntityFrameworkCore;
using PokemanReviewApp.Interfaces;
using PokemanReviewApp.Model;

namespace PokemanReviewApp.Repository
{
    public class ReviewerRepository: IReviewerRepository
    {
        private readonly DataContext _context;

        public ReviewerRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateReviwer(Reviewer reviewer)
        {
            _context.Add(reviewer);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            _context.Remove(reviewer);
            _context.SaveChanges();
            return true;
        }

        public Reviewer GetReviewer(int id)
        {
            return _context.Reviewers.Where(r => r.Id == id).Include(r => r.Reviews).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _context.Reviewers.ToList();
        }

        public ICollection<Review> GetReviwByAReviewer(int reviewerid)
        {
            return _context.Reviews.Where(r=> r.Reviewer.Id == reviewerid).ToList();
        }

        public bool ReviewerExist(int id)
        {
            return _context.Reviewers.Any(r => r.Id == id);
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _context.Update(reviewer);
            _context.SaveChanges();
            return true;
        }
    }
}
