using System;

namespace FwaEu.ElderAid.Stock.WebApi
{
    public class GetAllArticlesInStockPostApi
    {
        public int? CabinetId { get; set; }
        public string SearchTerm { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
