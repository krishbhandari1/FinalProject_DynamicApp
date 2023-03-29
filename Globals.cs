using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Web.UI.WebControls;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace FP_DynamicApp
{

    internal class Globals
    {
        private static string token;
        private static string lnId = "";
        private static string aId = "";
        private static string monthPayment="";
        private static string foldName = "";

        public static string GetAccessToken()
        {
            return token;
        }

        public static async Task SetAccessToken(string un, string pw)
        {
            var options = new RestClientOptions("https://api.elliemae.com")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/oauth2/v1/token", Method.Post);
            request.AddParameter("grant_type", "password");
            request.AddParameter("username", un+"@encompass:BE11166969");
            request.AddParameter("password", pw);
            request.AddParameter("client_id", "ruc51qr");
            request.AddParameter("client_secret", "ysli#MSdlbyHngOyctiJmK$0l82^9!o$dzSwvhW612!GPoi38loF4CFQ8ZiJTu$@");
            RestResponse response = await client.ExecuteAsync(request);
            //Console.WriteLine(response.Content);
            //Console.ReadLine();
            JObject respContent = JObject.Parse(response.Content);
            //Instantiate a JToken object with the extracted access token
            JToken aToken = respContent.SelectToken("$.access_token");
            //Set the token variable to the extracted access token as a string
            token = aToken.ToString();
        }
        public static string GetLoanId()
        {
            return lnId;
        }
        public static void SetLoanId(string id)
        {
            id = lnId;
        }
        public static string GetAppId()
        {
            return aId;
        }
        public static void SetAppId(string appId)
        {
            appId=aId;
        }
        public static async Task RetrieveLoan(string loanId)
        {
            var options = new RestClientOptions("https://api.elliemae.com")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/encompass/v3/loans/{loan}", Method.Get)
                .AddUrlSegment("loan", loanId);
            request.AddHeader("Authorization", "Bearer " + token);
            //request.AddHeader("Content-Type", "application/json");
            RestResponse response = await client.ExecuteAsync(request);
            Console.WriteLine(response.Content);
            //Parse the response content
            JObject respContent = JObject.Parse(response.Content);
            JToken appGuid = respContent.SelectToken("$.applications[0].id");    
            aId = appGuid.ToString();
            JToken loanGuid = respContent.SelectToken("$.id");
            //Set the lnId variable to the extracted lnId as a string
            lnId = loanGuid.ToString();
        }

        ///Task 3
        public static string GetPayment()
        {
            return monthPayment;
        }
        public static void SetPayment(string pay)
        {
            pay = monthPayment;
        }
        public static async Task UpdateLoan(string ssn, string dob, string estimatedValue, string amount, string rate, string term)
        {
            var options = new RestClientOptions("https://api.elliemae.com")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/encompass/v3/loans/{loan}?view=entity", Method.Patch)
                .AddUrlSegment("loan", GetLoanId());
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + token);
            var app= GetAppId();
            var body = @"{" + "\n" +
            @"    ""applications"": [" + "\n" +
            @"        {" + "\n" +
            @"            ""id"":""" + app + @"""," + "\n" +
            @"            ""borrower"": {" + "\n" +
            @"                ""birthDate"": """ + dob + @"""," + "\n" +
            @"                ""taxIdentificationIdentifier"": """ + ssn + @"""" + "\n" +
            @"            }" + "\n" +
            @"        }" + "\n" +
            @"    ]," + "\n" +
            @"    ""propertyEstimatedValueAmount"": """ + estimatedValue+ @"""," + "\n" +
            @"    ""requestedInterestRatePercent"": """ + rate + @"""," + "\n" +
            @"    ""borrowerRequestedLoanAmount"": """ + amount + @"""," + "\n" +
            @"    ""loanAmortizationTermMonths"": """ + term + @"""" + "\n" +
            @"}";
            request.AddStringBody(body, DataFormat.Json);
            RestResponse response = await client.ExecuteAsync(request);
            Console.WriteLine(response.Content);
            JObject respContent = JObject.Parse(response.Content);
            JToken monPay = respContent.SelectToken("$.freddieMac.allMonthlyPayments");
            monthPayment = monPay.ToString();
        }
        //task 4
        public static async Task MoveLoan()
        {
            var options = new RestClientOptions("https://api.elliemae.com")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/encompass/v1/loanfolders/DevEssentialsCert/loans/?action=add", Method.Patch);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddHeader("Content-Type", "application/json");
            var body = @"{ " + "\n" +
            @"""loanGuid"": """ + lnId + @""" " + "\n" +
            @"}";
            request.AddStringBody(body, DataFormat.Json);
            RestResponse response = await client.ExecuteAsync(request);
            Console.WriteLine(response.Content);

        }

        //task 5
        public static string GetFolder()
        {
            return foldName;
        }
        public static void SetFolder(string fol)
        {
            fol = foldName;
        }
        public static async Task DisplayLoanFolder()
        {
            var options = new RestClientOptions("https://api.elliemae.com")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/encompass/v1/loanPipeline", Method.Post);
            request.AddHeader("Authorization", "Bearer 0004TllIkwemLAtKnahH6rsk6aMv");
            request.AddHeader("Content-Type", "application/json");
            var body = @"{" + "\n" +
            @"  ""loanGuids"":[" + "\n" +
            @"    """ + lnId + @"""" + "\n" +
            @"  ]," + "\n" +
            @"    ""fields"": [" + "\n" +
            @"        ""Loan.LoanFolder""" + "\n" +
            @"    ]" + "\n" +
            @"}";
            request.AddStringBody(body, DataFormat.Json);
            RestResponse response = await client.ExecuteAsync(request);
            Console.WriteLine(response.Content);
            JObject respContent = JObject.Parse(response.Content);
            JToken myFolder = respContent.SelectToken("$.loanFolder");
            foldName = myFolder.ToString();
        }
    }
}

