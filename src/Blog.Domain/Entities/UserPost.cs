using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Domain.Entities;

public class UserPost
{
    [Key]
    public string PostId { get; set; }=Guid.NewGuid().ToString();
    public string UserId { get; set; }
    public User User { get; set; }
    public string PostTitle { get; set; }
    public ICollection<CommentText> CommentTexts { get; set; }=new List<CommentText>();
}