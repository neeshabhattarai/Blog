using FluentValidation;

namespace Blog.Application.UserPost.Command.AddUserPost;

public class AddUserPostValidation:AbstractValidator<AddUserPostCommand>
{
    public AddUserPostValidation()
    {
        RuleFor(x => x.PostTitle).NotEmpty().WithMessage("Please enter a post title ").Length(3,100).WithMessage("Please enter a post title");
    }
}