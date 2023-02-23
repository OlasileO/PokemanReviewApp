namespace PokemanReviewApp.Model
{
    public class Reviewer
    {
        public int Id { get; set; }
        public string FristName { get; set; }
        public string LastName { get; set; }
        public ICollection<Review> Reviews { get; set;}
    }
}
