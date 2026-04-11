using Blog.Domain.Entities;

namespace Blog.Application.UserPost.DTO;

public class PageResult
{
    public List<ReadUserPost> list { get; set; }
    public int From { get; set; }
    public int To { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public PageResult(List<ReadUserPost> list, int pageSize, int pageNumber)
    {
       this.list = list;
       this.PageSize = pageSize;
       this.PageNumber = pageNumber;
       this.From = (pageNumber-1) * pageSize + 1;
       this.To = From+pageSize-1;

    }
}