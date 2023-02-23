using PokemanReviewApp.Interfaces;
using PokemanReviewApp.Model;

namespace PokemanReviewApp.Repository
{
    public class ReviewRepository: IReviewRepository
    {
        private readonly DataContext _context;

        public ReviewRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateReview(Review review)
        {
            _context.Add(review);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteReview(Review review)
        {
            _context.Remove(review);
            _context.SaveChanges();
            return true;
        }

        public ICollection<Review> GetAllReviewOfPokemon(int pokeid)
        {
            return _context.Reviews.Where(r => r.Pokemon.Id == pokeid).ToList();    
        }

        public Review GetReview(int id)
        {
            return _context.Reviews.Where(r => r.Id == id).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.ToList();
        }

        public bool ReviewExists(int id)
        {
           return _context.Reviews.Any(r => r.Id == id);    
        }

        public bool UpdateReview(Review review)
        {
            _context.Update(review);
            _context.SaveChanges();
            return true;
        }
    }
}
