using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Blog.Domain.Entities;

public class CommentText
{
    [Key]
    public string CommentId { get; set; }=Guid.NewGuid().ToString();
    public string Comment { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public string PostId { get; set; }
    public UserPost Post { get; set; }
}