namespace Cash_Deposit_And_Withdrawal_BO
{
    public class CashDepositAndWithdrawal
    {
        public int CustomerTableId { get; set; }
        public string Date { get; set; }
        public decimal Amount { get; set; }
        public int AccountNumber { get; set; }
        public string TransactionType { get; set; }
    }
}