using Blog.Domain.Constant;
using Blog.Domain.Entities;

namespace Blog.Application.UserPost.DTO;

public class PageResult<T>
{
    public List<T> list { get; set; }
    public int From { get; set; }
    public int To { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public string OrderBy { get; set; }
    public string SortDirection { get; set; }
    public PageResult(List<T> list, int pageSize, int pageNumber,string orderBy, string sortDirection)
    {
       this.list = list;
       this.PageSize = pageSize;
       this.PageNumber = pageNumber;
       this.From = (pageNumber-1) * pageSize + 1;
       this.To = From+pageSize-1;
       this.OrderBy = orderBy;
       this.SortDirection=SortingDirection.desc==sortDirection?"desc":"asc";
       

    }
}