using System;

namespace FwaEu.MediCare.Referencials
{
    public class GetArticlesBySearchPost
    {
        public string SearchExpression { get; set; }
        public int? ArticleFamily { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
