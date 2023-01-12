using Userr;
namespace Customer_BusinessObject
{
    //this class is made to store all properties of customers as mentioned below.
    public class Customer : User
    {
        public int AccNum { get; set; }
        public string Status { get; set; }
        public decimal BankBalance { get; set; }
        public string CustomerName { get; set; }
        public string AccountType { get; set; }
        public int CustomerId { get; set; }

        //parameterized constructor to initialize all properties with
        //the user given values or of our own choices.
        public Customer(string username, string password, string type, string status, decimal bankBalance, string customerName, string accountType)
        {
            Username=username;
            Password=password;
            Type=type;
            Status=status;
            BankBalance=bankBalance;
            CustomerName=customerName;
            AccountType=accountType;
        }

        public Customer()
        {
        }
    }
}