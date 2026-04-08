using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Blog.Domain.Entities;

public class UserPost
{
    [Key]
    public string PostId { get; set; }
    public string UserId { get; set; }
    public IdentityUser User { get; set; }
    public string PostTitle { get; set; }
    public ICollection<CommentText> CommentTexts { get; set; }=new List<CommentText>();
}