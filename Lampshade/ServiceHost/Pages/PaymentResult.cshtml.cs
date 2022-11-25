using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0_Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PaymentResult = _0_Framework.Application.ZarinPal.PaymentResult;

namespace ServiceHost.Pages
{
    public class PaymentResultModel : PageModel
    {
        public PaymentResult Result;
        public void OnGet(PaymentResult result)
        {
            Result = result;
        }
    }
}
