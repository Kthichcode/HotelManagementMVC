namespace HotelManagementMVC.Models
{
    public class PaymentResultViewModel
    {
        public bool Success { get; set; }
        public string PaymentMethod { get; set; } = "VnPay";
        public string OrderDescription { get; set; }
        public string OrderId { get; set; }
        public string TransactionId { get; set; }
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; }
    }
}
