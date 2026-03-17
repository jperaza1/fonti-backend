using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Common.CheckBook
{
    public class CreateNewUserModel
    {
        public string name { get; set; }
        public string user_id { get; set; }
    }

    public class ResponseCreateUser
    {
        public string id { get; set; }
        public string key { get; set; }
        public string secret { get; set; }
        public string user_id { get; set; }
    }
    public class ResponseCreateZelleAccount
    {
        public string date { get; set; }
        public string id { get; set; }
        public string name { get; set; }
    }

    public class ResponsePaymentResponse
    {
        public double amount { get; set; }
        public string comment { get; set; }
        public DateTime date { get; set; }
        public string description { get; set; }
        public string id { get; set; }
        public string imageUri { get; set; }
        public string name { get; set; }
        public int number { get; set; }
        public string recipient { get; set; }
        public string status { get; set; }
    }

    public class CreateZelleAccount
    {
        public string username { get; set; }
    }

    public class CreateZellePayment
    {
        public string[] deposit_options { get; set; }
        public string account { get; set; }
        public double amount { get; set; }
        public string recipient { get; set; }
        public string name { get; set; }
    }
}
