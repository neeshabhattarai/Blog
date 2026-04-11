using FluentValidation;

namespace Blog.Application.UserPost.Queries.GetAllUserPost;

public class GetAllUserPostValidator:AbstractValidator<GetAllUserPostCommand>
{
    public GetAllUserPostValidator()
    {
        RuleFor(x=>x.pageIndex).GreaterThanOrEqualTo(1).WithMessage("PageIndex greater than or equal to 1");
        RuleFor(x=>x.pageSize).GreaterThanOrEqualTo(1).WithMessage("PageSize greater than or equal to 1");
    }
}