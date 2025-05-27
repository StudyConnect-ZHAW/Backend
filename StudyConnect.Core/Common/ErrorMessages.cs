namespace StudyConnect.Core.Common
{
    public static class ErrorMessages
    {
        // General
        public const string UnknownError = "An unexpected error occurred.";
        public const string NotFound = "The requested resource was not found.";
        public const string NotAuthorized = "Not authorized to perform this action.";
        public const string NameRequired = "Name cannot be null, empty or whitespace.";

        public const string GeneralTaken = "in use.";
        public const string TitleTaken = "Title already in use.";
        public const string NameTaken = "Name already in use.";
        public const string LikeExists = "Like already exists.";

        public const string QueryFailure = "Malformed or invalid query.";

        public const string InvalidInput = "Input is invalid.";

        public const string InvalidUserId = "Invalid user ID.";
        public const string InvalidPostId = "Invalid post ID.";
        public const string InvalidCommentId = "Invalid comment ID.";
        public const string InvalidCategoryId = "Invalid category ID.";
        public const string InvalidGroupId = "Invalid group ID.";

        public const string CommentContentEmpty = "Missing comment content.";
        public const string PostContentEmpty = "Missing post content.";

        public const string GeneralNotFound = "does not exist.";
        public const string CategoryNotFound = "Category with specified ID does not exist.";
        public const string CommentNotFound = "Comment with specified ID does not exist.";
        public const string CommentsNotFound = "Comments with specified ID does not exist.";
        public const string PostNotFound = "Post with specified ID does not exist.";
        public const string UserNotFound = "User with specified ID does not exist.";
        public const string ParentCommentNotFound = "Parent comment with specified ID does not exist.";
        public const string LikeNotFound = "Like with specified ID does not exist.";
        public const string GroupNotFound = "Group with specified ID does not exist.";
        public const string MemberNotFound = "GroupMember with specified ID does not exist.";
    }
}

