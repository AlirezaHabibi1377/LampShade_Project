namespace DiscountManagement.Application.Contracts.ColleagueDiscount
{
    public class ColleagueDiscountViewModel
    {
        public string Product { get; set; }
        public long ProductId { get; set; }
        public int DiscountRate { get; set; }
        public string CreationDate { get; set; }
        public long Id { get; set; }
    }
}