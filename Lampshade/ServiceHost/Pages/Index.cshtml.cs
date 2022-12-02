using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0_Framework.Application.Email;

namespace ServiceHost.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public void OnGet()
        {
            _emailService.SendEmail("Buy Grocery...", "Amirhosein Habibi Thanks for your Buying","habibi.alireza@ut.ac.ir");
        }
    }
}
