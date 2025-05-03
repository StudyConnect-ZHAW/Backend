

namespace StudyConnect.Core.Models;

public class ForumPost
{
    public required string Title;
    public required string Content;
    public required ForumCategory Category;
    public required User Author;
}
