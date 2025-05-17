using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyConnect.Data.Entities;

public class ForumLikes
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid LikeId { get; set; }

    [Required]
    public Guid UserId { get; set; }

    public Guid? ForumPostId { get; set; }

    public Guid? ForumCommentId { get; set; }

    public DateTime LikedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    [ForeignKey("ForumPostId")]
    public virtual ForumPost? ForumPost { get; set; }

    [ForeignKey("ForumCommentId")]
    public virtual ForumComment? ForumComment { get; set; }
}
