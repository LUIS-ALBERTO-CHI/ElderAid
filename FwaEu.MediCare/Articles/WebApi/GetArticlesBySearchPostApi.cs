using System;

namespace FwaEu.MediCare.Articles.WebApi
{
    public class GetArticlesBySearchPostApi
    {
        public string SearchExpression { get; set; }
        public ArticleType? ArticleFamily { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
