namespace Cash_Transfer_BO
{
    public class CashTransfer
    {
        public int CustomerTableId { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public string Date { get; set; }
        public decimal Amount { get; set; }
    }
}