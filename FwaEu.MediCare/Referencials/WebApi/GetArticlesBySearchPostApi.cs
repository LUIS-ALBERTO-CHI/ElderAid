﻿using System;

namespace FwaEu.MediCare.Referencials.WebApi
{
    public class GetArticlesBySearchPostApi
    {
        public string SearchExpression { get; set; }
        public int? ArticleFamily { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
