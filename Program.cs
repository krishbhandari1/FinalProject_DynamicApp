using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using static FP_DynamicApp.Globals;

namespace FP_DynamicApp
{
    class Program
    {
        private static string userName, password;
        private static string accessToken;
        private static string ssn, dob, estimatedValue, rate, term, amount;
        private static string appId;
        private static string loanId;
        private static string monthlyPayment;
        private static string loanFolder;
        static void Main(string[] args)
        {
            CompleteAuthentication();
            RequestLoan();
            LoanUpdates();
            MoveLoanFolder();
            RetrieveLoanFolder();

        }


        private static void CompleteAuthentication()
        {
            Console.WriteLine("User Name:");
            userName = Console.ReadLine();
            Console.WriteLine("Password:");
            password = Console.ReadLine();
            var task1 = SetAccessToken(userName,password);
            Task.WaitAll(task1);
            accessToken = GetAccessToken();
            Console.WriteLine("The access token is: " + accessToken);
            Console.ReadLine();
        }
        private static void RequestLoan()
        {
            Console.WriteLine("Please Enter Loan GUID:");
            loanId = Console.ReadLine();
            var task2 = RetrieveLoan(loanId);
            Task.WaitAll(task2);
            appId = GetAppId();
            Console.WriteLine("\nThe Application ID (GUID) is: " + appId);
            Console.ReadLine();
        }

        private static void LoanUpdates()
        {
            Console.WriteLine("SSN:");
            ssn = Console.ReadLine();
            Console.WriteLine("DOB:");
            dob = Console.ReadLine();
            Console.WriteLine("Estimated Property Value:");
            estimatedValue = Console.ReadLine();
            Console.WriteLine("Loan Amount:");
            amount= Console.ReadLine();
            Console.WriteLine("Note Rate:");
            rate = Console.ReadLine();
            Console.WriteLine("Loan Term:");
            term = Console.ReadLine();
            var task3 = UpdateLoan(ssn, dob, estimatedValue, amount, rate, term);
            Task.WaitAll(task3);
            monthlyPayment = GetPayment();
            Console.WriteLine("\nThe estimated monthly payment is: " + monthlyPayment);
            Console.ReadLine();
        }
        private static void MoveLoanFolder()
        {
            var task4 = MoveLoan();
            Task.WaitAll(task4);
            Console.WriteLine("Folder move successful! ");
            Console.ReadLine();
        }
        private static void RetrieveLoanFolder()
        {
            var task5 = DisplayLoanFolder();
            Task.WaitAll(task5);
            loanFolder = GetFolder();
            Console.WriteLine("Thank you for everything!");
            Console.ReadLine();
        }

    }
}



