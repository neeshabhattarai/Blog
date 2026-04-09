using System.Text.Json.Serialization;
using Blog.Application.Comments.DTO;
using Blog.Domain.Entities;
using MediatR;

namespace Blog.Application.Comments.Command.UpdateComment;

public class UpdateCommentCommand:IRequest<ReadCommentDTO?>
{
    [JsonIgnore]
    public string? CommentId{get;set;}
    public string Comment { get; set; }
}