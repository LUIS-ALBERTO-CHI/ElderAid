namespace FwaEu.ElderAid.Stock.WebApi
{
    public class UpdateStockPostApi
    {
        public int StockId { get; set; }
        public int Quantity { get; set; }
        public int PatientId { get; set; }
    }
}
