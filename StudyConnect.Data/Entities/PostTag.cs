using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyConnect.Data.Entities;

public class PostTag
{
    [Key]
    [Required]
    public Guid ForumPostId { get; set; }

    [Key]
    [Required]
    public Guid TagId { get; set; }

    [Required]
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(ForumPostId))]
    public virtual ForumPost? ForumPost { get; set; }

    [ForeignKey(nameof(TagId))]
    public virtual Tag? Tag { get; set; }
}