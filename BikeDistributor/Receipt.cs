using System.Collections.Generic;

namespace BikeDistributor
{
    public class Receipt
    {
        public string Heading { get; set; }

        public List<string> Items { get; set; }

        public string SubTotal { get; set; }

        public string Tax { get; set; }

        public string Total { get; set; }
    }
}
