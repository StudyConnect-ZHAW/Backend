namespace StudyConnect.API
{
    /// <summary>
    /// a mock object for the post endpoints
    /// </summary>
    public class ForumDiscussion
    {
        /// <summary> the unique id of the post </summary>
        public Guid DiscussionId { get; private set; }
        /// <summary> the unique creator of the post </summary>
        public Guid Author  { get; private set; }
        /// <summary> the time at which the post is created </summary>
        public DateTime MadeAt { get; private set; }
        /// <summary> the different tags of this post </summary>
        public List<String>? Tags { get; private set; }
        /// <summary> a list of Comments to the post </summary>
        public List<ForumPost> Posts { get; set; }
        /// <summary> the modul the post belongs to </summary>
        /// <summary> the title defining the post </summary>
        public string Title { get; set; }

        /// <summary>
        /// the constuctor of this mock object
        /// </summary>
        /// <param name="tags"> a list of tags the post should have </param>
        /// <param name="modul"> the modul to be assigned to </param>
        /// <param name="title"> the title defining the topic of the post </param> 
        /// <param name="author"> the unique author of this post </param>
        /// <param name="content"> the content of the post </param>
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

        /// <summary> a function to add a tag to the post, if it does not exist </summary>
        public void AddTag(string tag)
        {
            if (!(Tags == null || Tags.Contains(tag))) {
                this.Tags.Add(tag);
            }
        }

        /// <summary> a function to add a Comment to the post </summary> 
        public void AddComment(Guid author, string content)
        {
            this.Posts.Add(new ForumPost(this.DiscussionId, author, content));
        }
    }
    
    /// <summary>
    /// mock object for the Comments endpoint
    /// </summary>
    public class ForumPost
    {
        /// <summary> the unique id of the Comment </summary>
        /// <summary> the id of the post the comment belongs to </summary>
        public Guid PostId { get; private set; }
        /// <summary> the unique author of the comment </summary>
        public Guid DiscussionId { get; private set; }
        public Guid Author { get; private set; }
        /// <summary> the content of the comment, mainly text </summary>
        public string Content { get; private set; }
        /// <summary> the time of creation of the comment </summary> 
        public DateTime MadeAt { get; private set; }
        /// <summary> the list of users, who have left a like to this comment </summary>j
        private HashSet<Guid> likedByUsers;

        /// <summary>
        /// the constuctor of this mock Comment
        /// </summary>
        /// <param name="post"> the post the comment belongs to </param>
        /// <param name="author"> the author of ths comment </param>
        /// <param name="content"> the content of the comment </param>
        public ForumPost(Guid discussion, Guid author, string content)
        {
            this.DiscussionId = discussion;
            this.PostId = Guid.NewGuid();
            this.Author = author;
            this.Content = content;
            this.MadeAt = DateTime.Now;
            this.likedByUsers = new HashSet<Guid>();
        }

        /// <summary> a function to get the number of likes for the comment </summary>
        public int GetLikes()
        {
            return likedByUsers.Count;
        }

        /// <summary> a function to set a like for this comment </summary>
        public void SetLike(Guid author)
        {
            if(!likedByUsers.Contains(author) && author != this.Author)
            {
                likedByUsers.Add(author);
            }
        }

        /// <summary> a function to remove a like from this comment </summary>
        public void RemoveLike(Guid author)
        {
            if (likedByUsers.Contains(author))
            {
                likedByUsers.Remove(author);
            }
        }
    }
}
