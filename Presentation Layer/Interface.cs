using Userr;
using Customer_BusinessObject;
using Business_Logic_Layer;
using Cash_Deposit_And_Withdrawal_BO;
using Cash_Transfer_BO;
namespace Presentation_Layer
{
    //this class contains two functions: 
    //one for displaying admin menu
    //and other for displaying customer menu
    public class RenderMenus
    {
        //1--
        //this function just displays the administrator's menu
        public static void DisplayAdministratorMenu()
        {
            Console.WriteLine("\n\n_____________ADMINISTRATOR MENU_____________\n");
            Console.Write($"1----Create New Account.\n" +
                $"2----Delete Existing Account.\n" +
                $"3----Update Account Information.\n" +
                $"4----Search for Account.\n" +
                $"5----View Reports.\n" +
                $"6----Exit.\n\n" +
                $"Please select one of the above options: ");
        }

        //2--
        //this function just displays the customer's menu
        public static void DisplaycustomerMenu()
        {
            Console.WriteLine("\n\n_____________CUSTOMER MENU_____________\n");
            Console.Write("1----Withdraw Cash.\n" +
                "2----Cash Transfer.\n" +
                "3----Deposit Cash.\n" +
                "4----Display Balance.\n" +
                "5----Exit.\n\n" +
                "Please select one of the above options: ");
        }
    }


    //this class contains one function which executes the login process
    public class Login
    {
        //1-
        //this functions executes the login process
        //using the functions of bsiness logic layer
        public static void LoginProcess()
        {
            string username, password;
            int incPasswordCount = 0;
            User user;

            Console.WriteLine("\n\n\t\t***WELCOME TO ATM MANAGEMENT SOFTWARE***");
            Console.WriteLine("\n\t\t\t***LOGIN SCREEN***");
            Console.WriteLine("\n\t\t\tEnter your credentials");

            while (true)//this loop iterates until the user enters a valid/existing username.
            {
                Console.Write("Username: ");
                username = Console.ReadLine();//input username
                user=Verification.UserExists(username);//checks whether user exists in the table or not

                if (user == null)//if user does not exist
                {
                    Console.WriteLine("Incorrect Username. Please enter correct username.\n");
                }
                else//if user exists
                {
                    //check if the user is customer or admin
                    if (user.Type=="Admin")//if admin
                    {
                        break;
                    }
                    else if (user.Type=="Customer")//if customer
                    {
                        //check if the customer is active or disabled
                        if (Verification.isActive(user))//if active
                        {
                            break;
                        }
                        else//if disabled
                        {
                            Console.WriteLine("Your Account is disabled. Please contact the administrator!!");
                            return;
                        }
                    }
                }
            }


            if (user.Type=="Admin")//if user is admin
            {
                while (true)//this loop willl ask for correct password until the admin enters correct pass
                {
                    Console.Write("Password: ");
                    password = Console.ReadLine();
                    //if entered password is equal to decrypted form of the user's password stored in database
                    if (password == Verification.EncryptionDecryption(user.Password))//if password is correct
                    {
                        Console.WriteLine("Login Successful");
                        AdminFunctionalities.Choose();
                        break;
                    }
                    //else, ask for password again
                    Console.WriteLine("Incorrect Password. Enter corerct password.\n");
                }
            }

            else if (user.Type=="Customer")//is user is customer
            {
                //this loop asks for correct password.
                //if customer enters correct password in 3 attempts,
                //he will login successfully. otherwise, his account willl be disabled
                while (incPasswordCount<3)
                {
                    Console.Write("Password: ");
                    password = Console.ReadLine();
                    //if entered password is equal to decrypted form of the user's password stored in database
                    if (password == Verification.EncryptionDecryption(user.Password))//if password is correct
                    {
                        Console.WriteLine("Login Successful");
                        CustomerFunctionalities.Choose(user);//display customer menu
                        break;
                    }
                    incPasswordCount++;//increment if the password is incorrect
                    if (incPasswordCount<3)//if 3 attempt not done, ask for password again
                    {
                        Console.WriteLine("Incorrect Password. Enter corerct password.\n");
                    }
                }
            }

            if (incPasswordCount>=3)//if 3 attempts done and password is still incorrrect, 
            {
                Verification.DisableUser(user);//disable ths customer
                Console.WriteLine("Your Account is disabled. Contact the administrator.");
                return;
            }


        }
    }


    public class AdminFunctionalities
    {
        public static void Choose()
        {
            int choice;
            while (true)
            {
                RenderMenus.DisplayAdministratorMenu();//display admin menu
                while (true)
                {
                    choice = int.Parse(Console.ReadLine());
                    if (choice>=1&&choice<=6)
                    {
                        break;
                    }
                    Console.WriteLine("Enter a valid choice.");
                }

                switch (choice)
                {
                    case 1:
                        {
                            CreateNewAccount();
                            break;
                        }
                    case 2:
                        {
                            DeleteExistingAccount();
                            break;
                        }
                    case 3:
                        {
                            UpdateAccountInformation();
                            break;
                        }
                    case 4:
                        {
                            SearchForAccount();
                            break;
                        }
                    case 5:
                        {
                            ViewReports();
                            break;
                        }
                    case 6:
                        {
                            return;
                        }
                }
            }
        }

        public static void CreateNewAccount()
        {
            string username, password, name, accountType, status;
            decimal startingBalance;
            Console.WriteLine("\nEnter following information to create new account.");

            while (true)
            {
                Console.Write("Username: ");
                username=Console.ReadLine();
                if (CrudOperationsManager.AlreadyExists(username))
                {
                    Console.WriteLine("This username already exists. Choose another username.");
                }
                else
                {
                    break;
                }
            }
            Console.Write("Password: ");
            password=Console.ReadLine();
            Console.Write("Holder's Name: ");
            name=Console.ReadLine();

            while (true)
            {
                Console.Write("Type (Savings,Current): ");
                accountType=Console.ReadLine();
                if (accountType=="Savings"||accountType=="Current")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Account type can only be Savings or Current. Choose a valid account type. ");
                }
            }

            Console.Write("Starting Balance: ");
            startingBalance=decimal.Parse(Console.ReadLine());
            Console.Write("Status: ");
            status=Console.ReadLine();

            Customer newCustomer = new Customer(username, password, "Customer", status, startingBalance, name, accountType);
            CrudOperationsManager.CreateNewAccount(newCustomer);
            int accNum = CrudOperationsManager.GetHighestAccountNumber();
            Console.WriteLine($"\nAccount Successfully Created – the account number assigned is: {accNum}");
        }

        public static void DeleteExistingAccount()
        {
            Customer customer = new Customer();
            Console.Write("Enter the account number which you want to delete: ");
            while (true)
            {
                customer.AccNum=int.Parse(Console.ReadLine());

                customer = CrudOperationsManager.GetCustomerName(customer);

                if (customer.CustomerName==null)
                {
                    Console.Write("\nThis account does not exist. Enter an existing account number to delete.");
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine($"You wish to delete the account held by Mr/Ms {customer.CustomerName}; If this information is correct, please re-enter the account number: ");
            int verificationAccNum = int.Parse(Console.ReadLine());

            if (verificationAccNum==customer.AccNum)
            {
                CrudOperationsManager.DeleteExistingAccount(customer);
                Console.WriteLine("\nAccount Deleted Successfully.\n");
            }
        }

        public static void UpdateAccountInformation()
        {
            int accNum;
            Customer oldCustomer;
            Customer updatedCustomer = new Customer();
            Customer temp = new Customer();

            Console.Write("Enter the Account Number: ");
            while (true)
            {
                accNum=int.Parse(Console.ReadLine());
                temp.AccNum=accNum;
                oldCustomer= CrudOperationsManager.ReadCustomerInformation(temp);
                if (oldCustomer==null)
                {
                    Console.Write("The Customer with this account number does not exist in the table. Enter a valid account number: ");
                    continue;
                }
                else
                {
                    Console.WriteLine($"Account # {oldCustomer.AccNum}\n" +
                        $"Type: {oldCustomer.AccountType}\n" +
                        $"Holder: {oldCustomer.CustomerName}\n" +
                        $"Balance: {oldCustomer.BankBalance}\n" +
                        $"Status: {oldCustomer.Status}\n");
                    break;
                }
            }



            Console.WriteLine("\nPlease enter in the fields you wish to update (leave blank otherwise):\n");
            Console.Write("Login: ");
            updatedCustomer.Username= Console.ReadLine();
            Console.Write("Password: ");
            updatedCustomer.Password= Console.ReadLine();
            Console.Write("Status: ");
            updatedCustomer.Status= Console.ReadLine();
            Console.Write("Holder Name: ");
            updatedCustomer.CustomerName= Console.ReadLine();

            updatedCustomer.CustomerId=oldCustomer.CustomerId;

            CrudOperationsManager.UpdateCustomerInformation(oldCustomer, updatedCustomer);
            Console.WriteLine("Your account has been successfully been updated.");
        }


        public static void SearchForAccount()
        {
            Customer customer = new Customer("", "", "", "", -1, "", "");

            string accNum, userId, customerName, accType, status, balance;
            Console.WriteLine("\nSEARCH MENU: \n");
            Console.Write("Account Id: ");
            accNum=Console.ReadLine();
            Console.Write("User Id: ");
            userId=Console.ReadLine();
            Console.Write("Holder's Name: ");
            customerName=Console.ReadLine();
            Console.Write("Type (Savings, Current): ");
            accType=Console.ReadLine();
            Console.Write("Balance: ");
            balance= Console.ReadLine();
            Console.Write("Status (Active, Disabled): ");
            status=Console.ReadLine();

            if (accNum!="")
            {
                customer.AccNum=int.Parse(accNum);
            }
            else
            {
                customer.AccNum=0;
            }
            if (userId!="")
            {
                customer.CustomerId=int.Parse(userId);
            }
            else
            {
                customer.CustomerId = 0;
            }
            if (customerName!="")
            {
                customer.CustomerName=customerName;
            }
            if (accType!="")
            {
                customer.AccountType=accType;
            }
            if (balance!="")
            {
                customer.BankBalance=decimal.Parse(balance);
            }
            else
            {
                customer.BankBalance=0.0M;
            }
            if (status!="")
            {
                customer.Status=status;
            }

            List<Customer> customers = new List<Customer>();
            Console.WriteLine("\n\n=====SEARCH RESULTS=====\n");
            customers= CrudOperationsManager.SearchAccounts(customer);
            if (customers==null)
            {
                Console.WriteLine("\nNo accounts match the search criteria.\n");
            }
            else
            {
                Console.WriteLine("_____________________________________________________________________________________________________________________________________________________________");
                Console.WriteLine("\n{0,30}{1,20}{2,20}{3,20}{4,20}{5,20}", "Account ID", "User ID", "Holder's Name", "Type", "Balance", "Status");
                foreach (Customer customerr in customers)
                {
                    Console.WriteLine();
                    Console.WriteLine("{0,30}{1,20}{2,20}{3,20}{4,20}{5,20}", customerr.AccNum, customerr.CustomerId, customerr.CustomerName, customerr.AccountType, customerr.BankBalance, customerr.Status);
                }
                Console.WriteLine("_____________________________________________________________________________________________________________________________________________________________");
            }
        }




        public static void ViewReports()
        {
            Console.WriteLine("Choose one option to view reports.");
            int choice, minAmount = 0, maxAmount = 0, accNum = 0;
            string startingDate = "", endingDate = "";
            while (true)
            {
                Console.Write("\n1---Accounts By Amount.\n" +
               "2---Accounts By Date.\n" +
               "Choice: ");
                choice=int.Parse(Console.ReadLine());

                if (choice==1)
                {
                    Console.Write("Enter the minimum amount: ");
                    minAmount=int.Parse(Console.ReadLine());
                    Console.Write("Enter the maximum amount: ");
                    maxAmount=int.Parse(Console.ReadLine());
                    break;
                }
                else if (choice==2)
                {
                    Console.WriteLine("Enter the account number of user whose transactions you wanna see: ");
                    accNum=int.Parse(Console.ReadLine());
                    Console.Write("Enter the starting date: ");
                    startingDate=Console.ReadLine();
                    Console.Write("Enter the ending date: ");
                    endingDate=Console.ReadLine();
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid choice! Choose a valid choice. ");
                }
            }


            if (choice==1)
            {
                List<Customer> customers;
                Console.WriteLine("\n\n=====SEARCH RESULTS=====\n");
                customers= CrudOperationsManager.ViewReportsByAccounts(minAmount, maxAmount);
                if (customers==null)
                {
                    Console.WriteLine("\nNo accounts have Bank Balance in your entered range.\n");
                }
                else
                {
                    Console.WriteLine("_____________________________________________________________________________________________________________________________________________________________");
                    Console.WriteLine("\n{0,30}{1,20}{2,20}{3,20}{4,20}{5,20}", "Account ID", "User ID", "Holder's Name", "Type", "Balance", "Status");
                    foreach (Customer customerr in customers)
                    {
                        Console.WriteLine();
                        Console.WriteLine("{0,30}{1,20}{2,20}{3,20}{4,20}{5,20}", customerr.AccNum, customerr.CustomerId, customerr.CustomerName, customerr.AccountType, customerr.BankBalance, customerr.Status);
                    }
                    Console.WriteLine("_____________________________________________________________________________________________________________________________________________________________");
                }
            }





            else if (choice==2)
            {
                Customer customer = CrudOperationsManager.GetCustomerByAccountNumber(accNum);
                if (customer==null)
                {
                    Console.WriteLine("No accounts with this account number exists.\n");
                    return;
                }
                customer.AccNum=accNum;

                Console.WriteLine("\n\n\n\t\t\t\t*******Withdrawal and Deposit Records********");
                
                List<CashDepositAndWithdrawal> depositAndWithdrawalTransactions = CrudOperationsManager.GetDepositAndWithdrawalInformation(customer, startingDate, endingDate);
                if (depositAndWithdrawalTransactions==null)
                {
                    Console.WriteLine("NO transaction record for cash withdrawal and deposit found between the defined range for this account.\n");
                    return;
                }
                
                Console.WriteLine("_____________________________________________________________________________________________________________________________________________________________");
                Console.WriteLine("\n{0,30}{1,20}{2,20}{3,20}{4,20}", "Transaction Type", "User ID", "Holder's Name", "Amount", "Date");
                foreach (CashDepositAndWithdrawal cashDepositAndWithdrawal in depositAndWithdrawalTransactions)
                {
                    Console.WriteLine();
                    Console.WriteLine("{0,30}{1,20}{2,20}{3,20}{4,20}", cashDepositAndWithdrawal.TransactionType, customer.CustomerId, customer.CustomerName, cashDepositAndWithdrawal.Amount, cashDepositAndWithdrawal.Date);
                }
                Console.WriteLine("_____________________________________________________________________________________________________________________________________________________________");


                

                Console.WriteLine("\n\n\n\t\t\t\t*******Cash Transfer Records********");
                List<CashTransfer> cashTransfers = CrudOperationsManager.GetCashTransferInformation(customer, startingDate, endingDate);
                if (cashTransfers==null)
                {
                    Console.WriteLine("NO transaction record for cash withdrawal and deposit found between the defined range for this account.\n");
                    return;
                }

                Console.WriteLine("_____________________________________________________________________________________________________________________________________________________________");
                Console.WriteLine("\n{0,30}{1,20}{2,30}{3,20}{4,20}{5,20}", "Transaction Type", "User ID", "Sender/ Holder's Name", "Receiver Name","Amount", "Date");
                foreach (CashTransfer cashTransfer in cashTransfers)
                {
                    Console.WriteLine();
                    Console.WriteLine("{0,30}{1,20}{2,30}{3,20}{4,20}{5,20}", "Cash Transfer", customer.CustomerId, cashTransfer.SenderName, cashTransfer.ReceiverName, cashTransfer.Amount, cashTransfer.Date);
                }
                Console.WriteLine("_____________________________________________________________________________________________________________________________________________________________");
            }

        }
    }










    public class CustomerFunctionalities
    {
        public static void Choose(User user)
        {
            int choice;
            while (true)
            {
                RenderMenus.DisplaycustomerMenu();//display admin menu
                while (true)
                {
                    choice = int.Parse(Console.ReadLine());
                    if (choice>=1&&choice<=5)
                    {
                        break;
                    }
                    Console.WriteLine("Enter a valid choice.");
                }

                switch (choice)
                {
                    case 1:
                        {
                            WithdrawCash(user);
                            break;
                        }
                    case 2:
                        {
                            CashTransfer(user);
                            break;
                        }
                    case 3:
                        {
                            DepositCash(user) ;
                            break;
                        }
                    case 4:
                        {
                            DisplayBalance(user) ;
                            break;
                        }
                    case 5:
                        {
                            return;
                        }
                }
            }
        }


        public static void WithdrawCash(User user)
        {
            int choice;
            Console.WriteLine("\nPlease select a mode of withdrawal.") ;
            while (true)
            {
                Console.Write("\n1-Fast Cash.\n" +
                    "2-Normal Cash.\n" +
                    "Your choice here (1/2)? ");
                choice=int.Parse(Console.ReadLine());
                if (choice!=1&&choice!=2) 
                {
                    Console.WriteLine("\nInvalid choice. Please make a valid choice.");
                }
                else
                {
                    break;
                }
            }

            if(choice==1)
            {
                FastCash(user);
            }
            else if(choice==2)
            {
                NormalCash(user) ;
            }
        }



        public static void FastCash(User user)
        {
            int cashToBeWithdrawn;
            int denominationChoice;
            char confirmation;
            int checkIfEnoughBalance;
            

            while(true)
            {
                Console.WriteLine("\n\nSelect one of the denominations from given options.\n");
                Console.WriteLine("1----500" +
                    "\n2----1000" +
                    "\n3----2000" +
                    "\n4----5000" +
                    "\n5----10000" +
                    "\n6----15000" +
                    "\n7----20000\n");
                Console.Write("Your Choice: ");
                denominationChoice=int.Parse(Console.ReadLine());

                if(denominationChoice==1)
                {
                    cashToBeWithdrawn=500;
                    break;
                }
                else if(denominationChoice==2)
                {
                    cashToBeWithdrawn=1000;
                    break;
                }
                else if( denominationChoice==3)
                {
                    cashToBeWithdrawn=2000;
                    break;
                }
                else if(denominationChoice==4)
                {
                    cashToBeWithdrawn=5000;
                    break;
                }
                else if(denominationChoice==5)
                {
                    cashToBeWithdrawn=10000;
                    break;
                }
                else if(denominationChoice==6)
                {
                    cashToBeWithdrawn=15000;
                    break;
                }
                else if(denominationChoice==7)
                {
                    cashToBeWithdrawn=20000;
                    break;
                }
                else
                {
                    Console.WriteLine("Invaid Input!\n");
                }
            }



            Console.Write($"Are you sure you want to withdraw Rs.{cashToBeWithdrawn} (Y/N)? ");
            confirmation=char.Parse(Console.ReadLine()) ;

            if (confirmation=='Y')
            {
                //check if there is enough balance in the account
                checkIfEnoughBalance= TransactionsManagement.CheckIfEnoughBalance(user, cashToBeWithdrawn);
                if(checkIfEnoughBalance==0)
                {
                    Console.WriteLine("\nYour bank balance is NOT enough for this withdrawal.\n");
                    return;
                }
                else if(checkIfEnoughBalance==1)
                {
                    bool withdrawalIsPossible = TransactionsManagement.AmountWithdrawnToday(user, cashToBeWithdrawn);
                    if (withdrawalIsPossible==true) 
                    {
                        TransactionsManagement.WithdrawCash(user, cashToBeWithdrawn);
                        Console.Write("\nCash Successfully Withdrawn!\n" +
                            "\nDo you wish to print a receipt (Y/N)? ");
                        confirmation=char.Parse(Console.ReadLine());
                        if (confirmation=='Y')
                        {
                            //print receipt
                            Customer customer = TransactionsManagement.GetPropertiesToPrintReceipt(user);
                            Console.WriteLine($"\n\nAccount # {customer.AccNum}" +
                                $"\nDate: {DateTime.Today.ToString("dd/MM/yyyy")}" +
                                $"\nWithdrawn: {cashToBeWithdrawn}" +
                                $"\nBalance: {customer.BankBalance}");
                            return;
                        }
                        else if (confirmation=='N')
                        {
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("You can not withdraw this amount. If you do so, the total amount withdrawn today will exceed 20,000 and that's against the rules.");
                        return;
                    }
                }
            }
            else if (confirmation=='N')
            {
                return;
            }
        }


        public static void NormalCash(User user)
        {
            int withdrawalAmount;
            char confirmation;
            int checkIfEnoughBalance;
            Console.Write("Enter the withdrawal Amount: ");
            while (true)
            {
                withdrawalAmount=int.Parse(Console.ReadLine());
                if (withdrawalAmount>0 &&withdrawalAmount<=20000)
                {
                    break;
                }
                else
                    Console.Write("Please enter a valid withdrawal amount: ");
            }




            //check if there is enough balance in the account
            checkIfEnoughBalance= TransactionsManagement.CheckIfEnoughBalance(user, withdrawalAmount);
            if (checkIfEnoughBalance==0)
            {
                Console.WriteLine("\nYour bank balance is NOT enough for this withdrawal.\n");
                return;
            }
            else if (checkIfEnoughBalance==1)
            {
                bool withdrawalIsPossible = TransactionsManagement.AmountWithdrawnToday(user, withdrawalAmount);
                if (withdrawalIsPossible==true)
                {
                    TransactionsManagement.WithdrawCash(user, withdrawalAmount);
                    Console.Write("\nCash Successfully Withdrawn!\n" +
                        "\nDo you wish to print a receipt (Y/N)? ");
                    confirmation=char.Parse(Console.ReadLine());
                    if (confirmation=='Y')
                    {
                        //print receipt
                        Customer customer = TransactionsManagement.GetPropertiesToPrintReceipt(user);
                        Console.WriteLine($"\n\nAccount # {customer.AccNum}" +
                            $"\nDate: {DateTime.Today.ToString("dd/MM/yyyy")}" +
                            $"\nWithdrawn: {withdrawalAmount}" +
                            $"\nBalance: {customer.BankBalance}\n\n");
                        return;
                    }
                    else if (confirmation=='N')
                    {
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("You can not withdraw this amount. If you do so, the total amount withdrawn today will exceed 20,000 and that's against the rules.");
                    return;
                }
            }
        }




        public static void CashTransfer(User user)
        {
            int cashToBeTransfered;
            char confirmation;
            Customer customer = new Customer();
            while (true)
            {
                Console.Write("\nEnter the amount you wanna transfer in multiples of 500: ");
                cashToBeTransfered=int.Parse(Console.ReadLine());
                if (cashToBeTransfered%500==0)
                {
                    break;
                }
                else
                    Console.WriteLine("Invalid input!");
            }

            Console.Write("\nEnter the account number to which you want to transfer: ");
            customer.AccNum=int.Parse(Console.ReadLine());

            Customer temp = CrudOperationsManager.GetCustomerName(customer);
            if (temp==null)
            {
                Console.WriteLine("Account Not Found!");
                return;
            }
            Console.Write($"\n\nYou wish to deposit Rs. {cashToBeTransfered} in account held by \nMr./Ms. {temp.CustomerName}; If this information is correct \nplease re-enter the account number: ");
            int confirmAccNum = int.Parse(Console.ReadLine());
            if (confirmAccNum!=customer.AccNum)
            {
                Console.WriteLine("Transaction cancelled because you entered a different account number. Please try again.\n");
                return;
            }

            //check if there is enough balance in the account
            int checkIfEnoughBalance = TransactionsManagement.CheckIfEnoughBalance(user, cashToBeTransfered);
            if (checkIfEnoughBalance==0) 
            {
                Console.WriteLine("\nYour bank balance is NOT enough for this transaction.\n");
                return;
            }
            else if (checkIfEnoughBalance==1)
            {
                TransactionsManagement.TransferCash(user, confirmAccNum, cashToBeTransfered);
            }

                Console.WriteLine("\nTransaction confirmed.\n");
            Console.Write("Do you wish to print a receipt (Y/N)? ");
            confirmation=char.Parse(Console.ReadLine());
            if (confirmation=='Y')
            {
                //print receipt
                Customer cus = TransactionsManagement.GetPropertiesToPrintReceipt(user);
                Console.WriteLine($"\n\nAccount # {cus.AccNum}" +
                    $"\nDate: {DateTime.Today.ToString("dd/MM/yyyy")}" +
                    $"\nTransferred: {cashToBeTransfered}" +
                    $"\nBalance: {cus.BankBalance}\n\n");
                return;
            }
            else if (confirmation=='N')
            {
                return;
            }
        }


        public static void DepositCash(User user)
        {
            char confirmation;
            int amountToBeDeposited;
            Console.Write("Enter the cash amount to deposit: ");
            amountToBeDeposited = int.Parse(Console.ReadLine());
            TransactionsManagement.DepositCash(user, amountToBeDeposited);


            Console.WriteLine("\nCash Deposited Successfully.\n");
            Console.Write("Do you wish to print a receipt (Y/N)? ");
            confirmation=char.Parse(Console.ReadLine());
            if (confirmation=='Y')
            {
                //print receipt
                Customer cus = TransactionsManagement.GetPropertiesToPrintReceipt(user);
                Console.WriteLine($"\n\nAccount # {cus.AccNum}" +
                    $"\nDate: {DateTime.Today.ToString("dd/MM/yyyy")}" +
                    $"\nDeposited: {amountToBeDeposited}" +
                    $"\nBalance: {cus.BankBalance}\n\n");
                return;
            }
            else if (confirmation=='N')
            {
                return;
            }
        }

        public static void DisplayBalance(User user)
        {
            Customer customer = TransactionsManagement.GetPropertiesToPrintReceipt(user);
            Console.WriteLine($"\n\nAccount # {customer.AccNum}" +
                    $"\nDate: {DateTime.Today.ToString("dd/MM/yyyy")}" +
                    $"\nBalance: {customer.BankBalance}\n\n");
        }
    }


}