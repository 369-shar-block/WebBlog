using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebBlog.Models
{
    public class BlogPost
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and max {1} characters long.", MinimumLength = 2)]
        public string? Title { get; set; }

        [Required]
        public string? CreatorId { get; set; }

        [Required]
        public string? Content { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? LastUpdated { get; set; }

        // Foreign Key
        public int CategoryId { get; set; }
        public string? Slug { get; set; }
        public string? Abstract { get; set; }
        public bool IsDeleted { get; set; }

        [DisplayName("Published")]
        public bool IsPublished { get; set; }

        public byte[]? ImageData { get; set; }
        public string? ImageType { get; set; }

        [NotMapped]
        public IFormFile? BlogPostImage { get; set; }


        // Navigation Properties

        // Add a relationship to the Category model
        public virtual Category? Category { get; set; }

        // Add a relationship to the Comment model
        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        // Add a relationship to the Tag model
        public virtual ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();

        public virtual BlogUser? Creator { get; set; }
    }
}
