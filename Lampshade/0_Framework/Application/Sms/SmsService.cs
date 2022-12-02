using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IPE.SmsIrClient;
using IPE.SmsIrClient.Models.Requests;
using Microsoft.Extensions.Configuration;
using SmsIrRestful;

namespace _0_Framework.Application.Sms
{
    public class SmsService
    {
        private readonly IConfiguration _configuration;

        public SmsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static async Task SendAsync(string mobile, string message)
        {
            SmsIr smsIr = new SmsIr("PwQwWGVfp0bFbyDDpLWmCrp9h4ieYjS5CAxIs3JBhfQCB6igznpAvctsxqZ6xLdq");

            var bulkSendResult = await smsIr.BulkSendAsync(
                30007732006508, message, new string[] {mobile});
        }
    }
}