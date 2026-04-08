using Blog.Application.Comments.DTO;

namespace Blog.Application.UserPost.DTO;

public class ReadUserPost
{
    public string UserName { get; set; }
    public string PostId { get; set; }
    public string PostTitle { get; set; }
    // public List<ReadCommentDTO> CommentTexts { get; set; }
    
    
}