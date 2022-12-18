using System.ComponentModel.DataAnnotations;

namespace WebBlog.Models
{
    public class Comment
    {
        // Primary Key
        public int Id { get; set; }

        // Foreign Key
        public int BlogPostId { get; set; }

        // Foreign Key
        [Required]
        public string? AuthorId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? LastUpdated { get; set; }

        public string? UpdatedReason { get; set; }

        [Required]
        [StringLength(5000, ErrorMessage = "The {0} must be at least {2} and max {1} characters long.", MinimumLength = 2)]
        public string? Body { get; set; }

        // Navigation Properties
        public virtual BlogPost? BlogPost { get; set; }

        // Add the relationship to the BlogUser
        public virtual BlogUser? Author { get; set; }
    }
}
