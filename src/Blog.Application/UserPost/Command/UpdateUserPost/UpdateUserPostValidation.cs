using FluentValidation;

namespace Blog.Application.UserPost.Command.UpdateUserPost;

public class UpdateUserPostValidation:AbstractValidator<UpdateUserPostCommand>
{
    public UpdateUserPostValidation()
    {
        RuleFor(x => x.PostTitle).NotEmpty().WithMessage("Title is required");
    }
}