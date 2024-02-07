using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesDataAccess.Criteria
{
    public class GamesCriteria
    {
        public DateTime? PurchaseDateFrom { get; set; }
        public DateTime? PurchaseDateTo { get; set; }
        public bool? IsVirtual { get; set; }
        public string? StoreName { get; set; }
        public string? StoreDescription { get; set; }

        public string? PlatformName { get; set; }
        public string? PlatformDescription { get; set; }

        public string? GameName { get; set; }
        public string? GameDescription { get; set; }
        public string? GameTags { get; set; }

        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
    }
}
