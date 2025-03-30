namespace StudyConnect.API
{
    public class ForumDiscussion
    {
        public Guid DiscussionId { get; private set; }
        public Guid Author  { get; private set; }
        public DateTime MadeAt { get; private set; }
        public List<String>? Tags { get; private set; }
        public List<ForumPost> Posts { get; set; }
        public string Title { get; set; }

        public ForumDiscussion (string content, Guid author, string title, string[] tags )
        {
            this.DiscussionId = Guid.NewGuid();
            this.Author = author;
            this.Title = title;
            this.Tags = new List<String>();
            this.Posts = new List<ForumPost>();
            this.MadeAt = DateTime.Now;
            foreach (var tag in tags) {
                AddTag(tag);
            }
            this.Posts.Add(new ForumPost(this.DiscussionId,author, content));
        }

        public void AddTag(string tag)
        {
            if (!(Tags == null || Tags.Contains(tag))) {
                this.Tags.Add(tag);
            }
        }

        public void AddPost(Guid author, string content)
        {
            this.Posts.Add(new ForumPost(this.DiscussionId, author, content));
        }
    }
    
    public class ForumPost
    {
        public Guid PostId { get; private set; }
        public Guid DiscussionId { get; private set; }
        public Guid Author { get; private set; }
        public string Content { get; private set; }
        public DateTime MadeAt { get; private set; }
        private HashSet<Guid> likedByUsers;

        public ForumPost(Guid discussion, Guid author, string content)
        {
            this.DiscussionId = discussion;
            this.PostId = Guid.NewGuid();
            this.Author = author;
            this.Content = content;
            this.MadeAt = DateTime.Now;
            this.likedByUsers = new HashSet<Guid>();
        }

        public int GetLikes()
        {
            return likedByUsers.Count;
        }

        public void SetLike(Guid author)
        {
            if(!likedByUsers.Contains(author) && author != this.Author)
            {
                likedByUsers.Add(author);
            }
        }

        public void RemoveLike(Guid author)
        {
            if (likedByUsers.Contains(author))
            {
                likedByUsers.Remove(author);
            }
        }
    }
}
