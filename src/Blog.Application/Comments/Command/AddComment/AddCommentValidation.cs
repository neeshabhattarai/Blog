using FluentValidation;

namespace Blog.Application.Comments.Command.AddComment;

public class AddCommentValidation:AbstractValidator<AddCommentCommand>
{
    public AddCommentValidation()
    {
        RuleFor(x=>x.Comment).NotEmpty().WithMessage("Comment is required").Length(3,100).WithMessage("Comment length must be greater than 3 characters");
    }
}