using Userr;
using Microsoft.Data.SqlClient;
using Customer_BusinessObject;
using Cash_Deposit_And_Withdrawal_BO;
using Cash_Transfer_BO;

namespace Data_Access_Layer
{
    //this class accesses, reads and updates the data required in the login process
    //a source of communication between database and business logic layer
    public class AccessCredentialsForLoginProcess
    {
        //1--
        //this function reads all data of users table and store it in the list.
        //finally it returns the list
        public static List<User> AccessCredentials()
        {
            //connecting to database to read the credentials of User
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM Software ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            con.Open();

            //query to read all data of users table
            string query = "SELECT * FROM USERS";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            User user;

            //Reading data of users table and storing it in a list
            List<User> usersList = new List<User>();
            while (dr.Read())
            {
                user = new User();
                user.Id = (int)dr[0];
                user.Username=(string)dr[1];
                user.Password=(string)dr[2];
                user.Type=(string)dr[3];
                usersList.Add(user);
            }
            con.Close();
            return usersList;
        }



        //2--
        //this function reads the credentials of those users from users table
        //who have their status = active in the cutomer table.
        //stores those users in a list and returns the list.
        public static List<User> AccessActiveUsers()
        {
            //connecting to database to read the credentials of User
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM Software ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            con.Open();

            //query to read all data of users table whose status is active in the customers table 
            string query = $"SELECT * FROM Users INNER JOIN Customer ON Customer.CustomerId = Users.Id WHERE Status='{"Active"}'";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            User user;

            //Reading data of users table and storing it in a list
            List<User> activeUsersList = new List<User>();
            while (dr.Read())
            {
                user = new User();
                user.Id=(int)dr[0];
                user.Username=(string)dr[1];
                user.Password=(string)dr[2];
                user.Type=(string)dr[3];
                activeUsersList.Add(user);
            }
            con.Close();
            return activeUsersList;
        }



        //3--
        //this function accesses the customer with the id given and
        //updates that user's status to disable
        public static void UpdateUserStatusToDisable(int id)
        {
            //connecting to database to update status of a specific customer
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM Software ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            con.Open();

            //query to update status of customer
            string query = $"UPDATE Customer SET Customer.Status='Disabled' WHERE Customer.CustomerId='{id}'";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }

    public class CRUD
    {
        public static List<Customer> ReadUsernames()
        {
            List<Customer> usernames = new List<Customer>();
            Customer cus = new Customer();
            //connecting to database to update status of a specific customer
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM Software ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            string query = $"SELECT * from Users";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                cus=new Customer();
                cus.Username=(string)dr[1];
                usernames.Add(cus);
            }
            con.Close();
            return usernames;
        }


        public static void CreateNewUser(Customer newCustomer)
        {
            //connecting to database to update status of a specific customer
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM Software ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            string query = $"INSERT INTO Users(Username, Password, Type) " +
                                    $"VALUES('{newCustomer.Username}', '{newCustomer.Password}', '{newCustomer.Type}')";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static int AccessUserId(Customer temp)
        {
            int id = 0;
            //connecting to database to update status of a specific customer
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM Software ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            string query = $"SELECT Id FROM Users WHERE Username='{temp.Username}'";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                id=(int)dr[0];
            }
            con.Close();
            return id;
        }

        public static int GetHighestAccountNumber()
        {
            List<int> accountNumbers = new List<int>();
            int highest = 0;
            //connecting to database to update status of a specific customer
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM Software ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            string query = $"SELECT * from Customer";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                accountNumbers.Add((int)dr[1]);
            }
            con.Close();

            for (int i = 0; i<accountNumbers.Count; i++)
            {
                if (i==0)
                {
                    highest=accountNumbers[i];
                }
                if (highest<accountNumbers[i])
                {
                    highest=accountNumbers[i];
                }
            }
            return highest;
        }


        public static void CreateNewCustomer(Customer newCustomer)
        {
            //connecting to database to update status of a specific customer
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM Software ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            string query = $"INSERT INTO Customer(status, bankBalance, customerName, AccountType, CustomerId, AccountNumber) " +
                $"VALUES('{newCustomer.Status}','{newCustomer.BankBalance}','{newCustomer.CustomerName}','{newCustomer.AccountType}','{newCustomer.CustomerId}','{newCustomer.AccNum}')";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }


        public static Customer GetCustomerName(Customer temp)
        {
            //connecting to database to update status of a specific customer
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM Software ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            string query = $"SELECT CustomerName from Customer WHERE AccountNumber='{temp.AccNum}'";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    temp.CustomerName=(string)dr[0];
                }
            }
            else
            {
                con.Close();
                return null;
            }
            con.Close();
            return temp;
        }


        public static int GetCustomerId(Customer temp)
        {
            int CustomerId = 0;
            //connecting to database to update status of a specific customer
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM Software ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            string query = $"SELECT CustomerId from Customer WHERE AccountNumber='{temp.AccNum}'";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                CustomerId=(int)dr[0];
            }
            con.Close();
            return CustomerId;
        }


        public static void RemoveCustomerFromUserTable(Customer temp)
        {
            //connecting to database to update status of a specific customer
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM Software ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            string query = $"DELETE from Users WHERE Id='{temp.CustomerId}'";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static void DeleteExistingAccount(Customer temp)
        {
            //connecting to database to update status of a specific customer
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM Software ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);



            Customer customer = GetCustomerByAccountNumber(temp.AccNum);
            int id = customer.Id;
           string query = $"DELETE from CashTransferRecords WHERE CustomerTableId='{id}'";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            query = $"DELETE from CashDepositAndWithdrawalRecords WHERE AccountNumber='{temp.AccNum}'";
            cmd = new SqlCommand(query, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();


             query = $"DELETE from Customer WHERE AccountNumber='{temp.AccNum}'";
            cmd = new SqlCommand(query, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }


        public static Customer ReadCustomerInformation(Customer temp)
        {
            Customer customer = new Customer();
            //connecting to database to update status of a specific customer
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM Software ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            string query = $"SELECT * from Customer WHERE AccountNumber='{temp.AccNum}'";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    customer.Id=(int)dr[0];
                    customer.AccNum=(int)dr[1];
                    customer.Status=(string)dr[2];
                    customer.BankBalance=(decimal)dr[3];
                    customer.CustomerName=(string)dr[4];
                    customer.AccountType=(string)dr[5];
                    customer.CustomerId=(int)dr[6];
                }
            }
            else
            {
                con.Close();
                return null;
            }
            con.Close();



            query = $"SELECT Username,Password from Users WHERE Id='{customer.CustomerId}'";
            cmd = new SqlCommand(query, con);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    customer.Username=(string)dr[0];
                    customer.Password=(string)dr[1];
                }
            }
            else
            {
                con.Close();
                return null;
            }

            con.Close();
            return customer;
        }


        public static void UpdateCustomerInformation(Customer customer)
        {
            //connecting to database to update status of a specific customer
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM Software ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            con.Open();

            //query to update status of customer
            string query = $"UPDATE Customer SET Customer.Status='{customer.Status}',Customer.CustomerName='{customer.CustomerName}' WHERE Customer.CustomerId='{customer.CustomerId}'";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            con.Close();


            con.Open();

            //query to update status of customer
            query = $"UPDATE Users SET Users.Username='{customer.Username}',Users.Password='{customer.Password}' WHERE Users.Id='{customer.CustomerId}'";
            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }


        public static List<Customer> SearchAccounts(string query)
        {
            Customer customer = new Customer();
            List<Customer> customers = new List<Customer>();
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM Software ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    customer = new Customer();
                    customer.AccNum=(int)dr[1];
                    customer.CustomerId=(int)dr[6];
                    customer.CustomerName=(string)dr[4];
                    customer.AccountType=(string)dr[5];
                    customer.BankBalance=(decimal)dr[3];
                    customer.Status=(string)dr[2];

                    customers.Add(customer);
                }
            }
            else
            {
                con.Close();
                return null;
            }
            con.Close();
            return customers;
        }


        public static List<Customer> ViewReportsByAccounts()
        {
            Customer customer;
            List<Customer> customers = new List<Customer>();
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM Software ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            string query = $"SELECT * FROM Customer";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    customer = new Customer();
                    customer.AccNum=(int)dr[1];
                    customer.CustomerId=(int)dr[6];
                    customer.CustomerName=(string)dr[4];
                    customer.AccountType=(string)dr[5];
                    customer.BankBalance=(decimal)dr[3];
                    customer.Status=(string)dr[2];

                    customers.Add(customer);

                }
            }
            else
            {
                con.Close();
                return null;
            }
            con.Close();
            return customers;
        }


        public static Customer GetCustomerByAccountNumber(int accNum)
        {
            Customer customer = new Customer();
            //connecting to database to update status of a specific customer
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM Software ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            string query = $"SELECT Id,CustomerId,CustomerName from Customer WHERE AccountNumber='{accNum}'";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    customer.Id=(int)dr[0];
                    customer.CustomerId=(int)dr[1];
                    customer.CustomerName=(string)dr[2];
                }
            }
            else
            {
                con.Close();
                return null;
            }
            con.Close();
            return customer;
        }


        public static List<CashDepositAndWithdrawal> GetDepositAndWithdrawalInformation(Customer customer, string startingDate, string endingDate)
        {
           CashDepositAndWithdrawal cashDepositAndWithdrawal;
            List<CashDepositAndWithdrawal> cashDepositAndWithdrawals = new List<CashDepositAndWithdrawal>();
            //connecting to database to update status of a specific customer
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM Software ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            string query = $"SELECT * from CashDepositAndWithdrawalRecords WHERE AccountNumber='{customer.AccNum}' AND Date BETWEEN '{startingDate}' AND '{endingDate}'";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    cashDepositAndWithdrawal= new CashDepositAndWithdrawal();
                    cashDepositAndWithdrawal.CustomerTableId=(int)dr[1];
                    cashDepositAndWithdrawal.Date=(string)dr[2];
                    cashDepositAndWithdrawal.Amount=(decimal)dr[3];
                    cashDepositAndWithdrawal.AccountNumber=(int)dr[4];
                    cashDepositAndWithdrawal.TransactionType=(string)dr[5];

                    cashDepositAndWithdrawals.Add(cashDepositAndWithdrawal);
                }
            }
            else
            {
                con.Close();
                return null;
            }
            con.Close();
            return cashDepositAndWithdrawals;
        }




        public static List<CashTransfer> GetCashTransferInformation(Customer customer, string startingDate, string endingDate)
        {
            CashTransfer cashTransfer;
            List<CashTransfer> cashTransfers = new List<CashTransfer>();
            //connecting to database to update status of a specific customer
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM Software ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            string query = $"SELECT * from CashTransferRecords WHERE CustomerTableId='{customer.Id}' AND Date BETWEEN '{startingDate}' AND '{endingDate}'";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    cashTransfer= new CashTransfer();
                    cashTransfer.SenderName=(string)dr[2];
                    cashTransfer.ReceiverName=(string)dr[3];
                    cashTransfer.Date=(string)dr[4];
                    cashTransfer.Amount=(decimal)dr[5];

                    cashTransfers.Add(cashTransfer);
                }
            }
            else
            {
                con.Close();
                return null;
            }
            con.Close();
            return cashTransfers;
        }
    }

    public class AccessRecordsForTransactions
    {
        public static decimal AccessBankBalanceRecords(User user)
        {
            decimal bankBalance = 0;
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM Software ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            string query = $"SELECT BankBalance from Customer WHERE CustomerId ='{user.Id}'";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    bankBalance=(decimal)dr[0];
                }
            }
            else
            {
                con.Close();
                return -1;
            }
            con.Close();
            return bankBalance;
        }

        public static Customer GetCustomerTableId(User user)
        {
            Customer customer = new Customer();
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM Software ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            string query = $"SELECT Id from Customer WHERE CustomerId ='{user.Id}'";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    customer.Id=(int)dr[0];
                }
            }
            con.Close();
            return customer;
        }


        public static List<decimal> GetAmountWithdrawnToday(Customer customer)
        {
            List<decimal> amounts = new List<decimal>();
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM Software ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            string query = $"SELECT Amount from CashDepositAndWithdrawalRecords WHERE CustomerTableId ='{customer.Id}' AND Date ='{DateTime.Today.ToString("dd/MM/yyyy")}' AND TransactionType='{"Withdraw"}'";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    amounts.Add((decimal)dr[0]);
                }
            }
            con.Close();
            return amounts;
        }


        public static void UpdateBankBalance(User user, int updatedBankBalance)
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM Software ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            string query = $"UPDATE Customer SET Customer.BankBalance='{updatedBankBalance}' WHERE Customer.CustomerId='{user.Id}'";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            cmd.ExecuteNonQuery();
        }


        public static void CreateNewWithdrawalEntry(Customer customer, int amountToBeWithdrawn)
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM Software ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            string query = $"INSERT INTO CashDepositAndWithdrawalRecords(CustomerTableId, Date, Amount,AccountNumber, TransactionType) " +
                                    $"VALUES('{customer.Id}', '{DateTime.Today.ToString("dd/MM/yyyy")}', '{amountToBeWithdrawn}','{customer.AccNum}', '{"Withdraw"}')";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static Customer AccessPropertiesToPrintReceipt(User user)
        {
            Customer customer = new Customer();
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM Software ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            string query = $"SELECT AccountNumber,BankBalance from Customer WHERE CustomerId='{user.Id}'";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    customer.AccNum=(int)dr[0];
                    customer.BankBalance=(decimal)dr[1];
                }
            }
            else
            {
                con.Close();
                return null;
            }
            con.Close();
            return customer;
        }

        public static string GetNameByCustomerId(Customer customer)
        {
            string name = "";
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM Software ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            string query = $"SELECT CustomerName from Customer WHERE CustomerId='{customer.CustomerId}'";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    name=(string)dr[0];
                }
            }
            else
            {
                con.Close();
                return null;
            }
            con.Close();
            return name;
        }



        public static void CreateNewCashTransferEntry(Customer sender, Customer receiverr, int cashToBeTransfered)
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM Software ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            string query = $"INSERT INTO CashTransferRecords(CustomerTableId, SenderName, ReceiverName, Date, Amount) " +
                                    $"VALUES('{sender.Id}', '{sender.CustomerName}', '{receiverr.CustomerName}','{DateTime.Today.ToString("dd/MM/yyyy")}','{cashToBeTransfered}')";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }




        public static void CreateNewCashDepositEntry(Customer customer, int cashToBeDeposited)
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM Software ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            string query = $"INSERT INTO CashDepositAndWithdrawalRecords(CustomerTableId, Date, Amount, AccountNumber, TransactionType) " +
                                    $"VALUES('{customer.Id}', '{DateTime.Today.ToString("dd/MM/yyyy")}', '{cashToBeDeposited}','{customer.AccNum}', '{"Deposit"}')";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}