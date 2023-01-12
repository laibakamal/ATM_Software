using Userr;
using Customer_BusinessObject;
using Data_Access_Layer;
using Cash_Deposit_And_Withdrawal_BO;
using Cash_Transfer_BO;

namespace Business_Logic_Layer
{
    //this class contains all logics which are applied on
    //the data accessed by DAL for login process.
    //a source of communication between data access
    //layer and presentation layer
    public class Verification
    {
        //1--
        //this function checks if the user with the given username
        //exists in the users table or not. For this purpose, it takes
        //help of "access credentials" function of data access layer.
        //it returns that user object if it exists and returns null if
        //that user does not exist.
        public static User UserExists(string username)
        {
            List<User> list;
            list=AccessCredentialsForLoginProcess.AccessCredentials();

            foreach (User user in list)
            {
                if (user!=null)
                {
                    if (EncryptionDecryption(user.Username)==username)
                    {
                        return user;
                    }
                }
            }
            return null;
        }



        //2--
        //this function returns true if the given user
        //has status= active and false if status = disabled.
        //For this purpose, this function uses "access active users"
        //of data access layer.
        public static bool isActive(User user)
        {
            List<User> list = AccessCredentialsForLoginProcess.AccessActiveUsers();
            foreach (User userr in list)
            {
                if (user.Username==userr.Username)
                {
                    return true;
                }
            }
            return false;
        }




        //3--
        //this function disables the given user.
        //means, it updates the status of customer in the
        //customer table to "disabled". For this purpose,
        //it uses the "update status to disable" function of
        //data access layer
        public static void DisableUser(User user)
        {
            int id = user.Id;
            AccessCredentialsForLoginProcess.UpdateUserStatusToDisable(id);
        }



        //4--
        //this function encrypts and decryptes string
        public static string EncryptionDecryption(string str)
        {
            string password = "";//It stores the encrypted string
            int x, y;//used in calculations 
            char c;//stores the encrypted character 

            if (str.Length==0) 
            {
                return "";
            }
            for (int i = 0; i < str.Length; i++)//this loop iterates the whole string
            {
                //c=str[i];
                //if c='B' means c=66
                //c-65=1 -->    x=1
                //26-x=25 -->   y=25
                //answer= y+64 = 89 = 'Y'
                if (str[i]>='A'&&str[i]<='Z')
                {
                    x=str[i]-65;
                    y=26-x;
                    c=(char)(y+64);
                    password+=c.ToString();
                }


                //c=str[i];
                //if c='b' means c=98
                //c-97=1 -->    x=1
                //26-x=25 -->   y=25
                //answer= y+96 = 121 = 'y'
                else if (str[i]>='a'&&str[i]<='z')
                {
                    x=str[i]-97;
                    y=26-x;
                    c=(char)(y+96);
                    password+=c.ToString();
                }


                //c=str[i];
                //if c='4'
                //x='9'- c
                //answer = '5'
                else if (str[i]>='0'&&str[i]<='9')
                {
                    x='9'-str[i];
                    password+=x.ToString();
                }

                //space in original string is encrypted as space
                else if (str[i]==' ')
                {
                    password+=' ';
                }
            }

            return password;
        }
    }


    public class CrudOperationsManager
    {
        public static bool AlreadyExists(string username)
        {
            List<Customer> customers;
            customers= CRUD.ReadUsernames();

            List<string> usernames = new List<string>();
            foreach (Customer customer in customers)
            {
                usernames.Add(customer.Username);
            }
            if (usernames.Contains(Verification.EncryptionDecryption(username)))
            {
                return true;
            }
            return false;
        }
        public static void CreateNewAccount(Customer newCustomer)
        {
            if (newCustomer!=null)
            {
                newCustomer.Password=Verification.EncryptionDecryption(newCustomer.Password);
                newCustomer.Username=Verification.EncryptionDecryption(newCustomer.Username);
            }
            //first create new user in the user table
            CRUD.CreateNewUser(newCustomer);
            //then read the id assigned to that user
            //to put that as customer id in the customer table
            newCustomer.CustomerId= CRUD.AccessUserId(newCustomer);
            //then find out the highest account number 
            //already available in the customers table
            int highestAccNum = CRUD.GetHighestAccountNumber();
            newCustomer.AccNum = highestAccNum+1;
            //then create a new customer in the customer table
            CRUD.CreateNewCustomer(newCustomer);
        }

        public static int GetHighestAccountNumber()
        {
            return CRUD.GetHighestAccountNumber();
        }


        public static Customer GetCustomerName(Customer temp)
        {
            Customer customer = CRUD.GetCustomerName(temp);
            return customer;
        }

        public static void DeleteExistingAccount(Customer customer)
        {
            int customerId;
            //firstly we need to read the customer id from
            //customer table which is to be deleted
            customerId= CRUD.GetCustomerId(customer);
            //delete the customer from customer table
            CRUD.DeleteExistingAccount(customer);
            //delete the customer from user table using customer id
            customer.CustomerId=customerId;
            CRUD.RemoveCustomerFromUserTable(customer);
        }


        public static Customer ReadCustomerInformation(Customer temp)
        {
            return (CRUD.ReadCustomerInformation(temp));
        }


        public static void UpdateCustomerInformation(Customer oldCustomer, Customer updatedCustomer)
        {
            if (updatedCustomer.Username=="")
            {
                updatedCustomer.Username=oldCustomer.Username;
            }
            if (updatedCustomer.Password=="")
            {
                updatedCustomer.Password=oldCustomer.Password;
            }
            if (updatedCustomer.Status=="")
            {
                updatedCustomer.Status=oldCustomer.Status;
            }
            if (updatedCustomer.CustomerName=="")
            {
                updatedCustomer.CustomerName=oldCustomer.CustomerName;
            }


            updatedCustomer.Username=Verification.EncryptionDecryption(updatedCustomer.Username);
            updatedCustomer.Password=Verification.EncryptionDecryption(updatedCustomer.Password);

            CRUD.UpdateCustomerInformation(updatedCustomer);
        }


        public static List<Customer> SearchAccounts(Customer customer)
        {
            int check = 0;
            string query = "SELECT * FROM Customer WHERE ";
            if (customer.AccNum!=0)
            {
                query+=$"AccountNumber='{customer.AccNum}'";
                check++;
            }
            if (customer.Status!="")
            {
                if (check!=0)
                {
                    query+="AND ";
                }
                query+=$"Status='{customer.Status}'";
                check++;
            }
            if (customer.AccountType!="")
            {
                if (check!=0)
                {
                    query+="AND ";
                }
                query+=$"AccountType='{customer.AccountType}'";
                check++;
            }
            if (customer.BankBalance!=0.0M)
            {
                if (check!=0)
                {
                    query+="AND ";
                }
                query+=$"BankBalance='{customer.BankBalance}'";
                check++;
            }
            if (customer.CustomerName!="")
            {
                if (check!=0)
                {
                    query+="AND ";
                }
                query+=$"CustomerName='{customer.CustomerName}'";
                check++;
            }
            if (customer.CustomerId!=0)
            {
                if (check!=0)
                {
                    query+="AND ";
                }
                query+=$"CustomerId='{customer.CustomerId}'";
                check++;
            }


            List<Customer> list = CRUD.SearchAccounts(query);
            if (list==null)
            {
                return null;
            }
            return list;
        }


        public static List<Customer> ViewReportsByAccounts(int minAmount, int maxAmount)
        {
            List<Customer> customers = CRUD.ViewReportsByAccounts();
            List<Customer> inRangeCustomers = new List<Customer>();
            if (customers==null)
                return null;
            foreach (Customer customer in customers)
            {
                if (customer.BankBalance>=minAmount&&customer.BankBalance<=maxAmount)
                {
                    inRangeCustomers.Add(customer);
                }
            }

            if (inRangeCustomers.Count==0)
                return null;
            return inRangeCustomers;
        }


        public static Customer GetCustomerByAccountNumber(int accNum)
        {
            Customer customer = CRUD.GetCustomerByAccountNumber(accNum);
            if (customer==null)
            {
                return null;
            }
            else
            {
                return customer;
            }
        }


        public static List<CashDepositAndWithdrawal> GetDepositAndWithdrawalInformation(Customer customer, string startingDate, string endingDate)
        {
            List<CashDepositAndWithdrawal> cashDepositAndWithdrawals = CRUD.GetDepositAndWithdrawalInformation(customer, startingDate, endingDate) ;
            if (cashDepositAndWithdrawals==null)
            {
                return null;
            }

            return cashDepositAndWithdrawals;
        }



        public static List<CashTransfer> GetCashTransferInformation(Customer customer, string startingDate, string endingDate)
        {
            Customer customer1 = CRUD.GetCustomerByAccountNumber(customer.AccNum);
            List<CashTransfer> cashTransfers = CRUD.GetCashTransferInformation(customer1, startingDate, endingDate);
            if (cashTransfers==null)
            {
                return null;
            }

            return cashTransfers;
        }
    }



    public class TransactionsManagement
    {
        public static int CheckIfEnoughBalance(User user, int denominationChoice)
        {
           decimal bankBalance = AccessRecordsForTransactions.AccessBankBalanceRecords(user);
            if (bankBalance==-1)
            {
                return -1;
            }
            else if (bankBalance>=denominationChoice)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }


        public static bool AmountWithdrawnToday(User user, int amountToBeWithdrawn)
        {
            int totalCashWithdrawnToday = 0;
           Customer customer = AccessRecordsForTransactions.GetCustomerTableId(user);
            List<decimal> amounts = AccessRecordsForTransactions.GetAmountWithdrawnToday(customer);
            foreach (decimal amount in amounts)
            {
                totalCashWithdrawnToday+=(int)amount;
            }

            if (totalCashWithdrawnToday+amountToBeWithdrawn>20000)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public static void WithdrawCash(User user, int amountToBeWithdrawn)
        {
            //update bank balance in customer table
            decimal bankBalance = AccessRecordsForTransactions.AccessBankBalanceRecords(user);
            decimal updatedBankBalance = bankBalance-amountToBeWithdrawn;
            AccessRecordsForTransactions.UpdateBankBalance(user, (int)updatedBankBalance);

            //create a new transaction entry in withdrawal table
            Customer custTableId = AccessRecordsForTransactions.GetCustomerTableId(user);
            Customer custAccNum = AccessRecordsForTransactions.AccessPropertiesToPrintReceipt(user);
            Customer customer = new Customer();
            customer.AccNum=custAccNum.AccNum;
            customer.Id=custTableId.Id;

            AccessRecordsForTransactions.CreateNewWithdrawalEntry(customer, amountToBeWithdrawn);
        }


        public static Customer GetPropertiesToPrintReceipt(User user)
        {
            Customer customer = AccessRecordsForTransactions.AccessPropertiesToPrintReceipt(user);
            return customer;
        }
 
        public static void TransferCash(User user, int accNum, int cashToBeTransfered)
        {
            //update sender's bank balance
            decimal senderBankBalance = AccessRecordsForTransactions.AccessBankBalanceRecords(user);
            senderBankBalance-=cashToBeTransfered;
            AccessRecordsForTransactions.UpdateBankBalance(user, (int)senderBankBalance);

            Customer customer = new Customer();
            customer.AccNum=accNum;

            User receiver = new User();
            receiver.Id=CRUD.GetCustomerId(customer) ;

            //update receiver's bank balance
            decimal receiverBankBalance= AccessRecordsForTransactions.AccessBankBalanceRecords(receiver) ;
            receiverBankBalance+=cashToBeTransfered;
            AccessRecordsForTransactions.UpdateBankBalance(receiver, (int)receiverBankBalance);


            //create new entry in cash transfer records table
            //1- get names of sender and receiver from customer table to put them in cash transfer table
            Customer sender = new Customer();
            sender.CustomerId=user.Id;
            Customer receiverr = new Customer();
            receiverr.CustomerId=receiver.Id;

            sender.CustomerName=AccessRecordsForTransactions.GetNameByCustomerId(sender);
            receiverr.CustomerName=AccessRecordsForTransactions.GetNameByCustomerId(receiverr);

            //2- get customer table id of sender
            Customer toGetCustTableId = AccessRecordsForTransactions.GetCustomerTableId(user) ;
            sender.Id=toGetCustTableId.Id;

            //3- create new entry in cash transfer records table
            AccessRecordsForTransactions.CreateNewCashTransferEntry(sender, receiverr, cashToBeTransfered); ;
        }



        public static void DepositCash(User user, int cashToBeDeposited)
        {
            decimal senderBankBalance = AccessRecordsForTransactions.AccessBankBalanceRecords(user);
            senderBankBalance+=cashToBeDeposited;
            AccessRecordsForTransactions.UpdateBankBalance(user, (int)senderBankBalance);

            Customer custAccNum = AccessRecordsForTransactions.AccessPropertiesToPrintReceipt(user);
            Customer custTableId = AccessRecordsForTransactions.GetCustomerTableId(user);

            Customer customer=new Customer();
            customer.Id=custTableId.Id;
            customer.AccNum=custAccNum.AccNum;

            AccessRecordsForTransactions.CreateNewCashDepositEntry(customer, cashToBeDeposited);
        }


    }

}