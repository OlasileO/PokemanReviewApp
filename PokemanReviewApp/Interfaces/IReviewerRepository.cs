using PokemanReviewApp.Model;

namespace PokemanReviewApp.Interfaces
{
    public interface IReviewerRepository
    {
        ICollection<Reviewer> GetReviewers();
        Reviewer GetReviewer(int id);
        ICollection<Review> GetReviwByAReviewer(int reviewerid);
        bool ReviewerExist(int id);
        bool CreateReviwer(Reviewer reviewer);
        bool UpdateReviewer(Reviewer reviewer);
        bool DeleteReviewer(Reviewer reviewer);
     
    }
}
