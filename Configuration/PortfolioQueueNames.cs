using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.AspNetCore.Middleware.Configuration
{
    public class PortfolioQueueNames
    {
        public string PortfolioCreated { get; set; }
        public string PortfolioUpdated { get; set; }
        public string PortfolioDeleted { get; set; }
    }
}
