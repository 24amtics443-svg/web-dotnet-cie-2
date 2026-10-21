// Namespace used for model classes.
namespace Task_2.Models
{
    // Post class represents data received from JSONPlaceholder API.
    public class Post
    {
        // Stores the ID of the post.
        public int Id { get; set; }

        // Stores the title of the post.
        public string Title { get; set; } = "";
    }
}