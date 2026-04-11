using Blog.Application.Comments.DTO;
using Blog.Domain.Entities;

namespace Blog.Application.UserPost.DTO;

public class ReadUserPostDTO
{
    public string UserName { get; set; }
    public string PostId { get; set; }
    public string PostTitle { get; set; }
    public List<ReadCommentDTO> CommentTexts { get; set; }
}