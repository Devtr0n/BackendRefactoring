using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BikeDistributor
{
    public class Order
    {
        private const double TaxRate = .0725d;
        private readonly IList<Line> Lines = new List<Line>();
        private double totalAmount = 0d;
        private double tax = 0d;
        public string Company { get; private set; }

        public Order(string company)
        {
            Company = company;
        }

        public void AddLine(Line line)
        {
            Lines.Add(line);
        }

        public string JsonReceipt()
        {
            var receipt = new Receipt
            {
                Heading = string.Format("Order Receipt for {0}{1}", Company, Environment.NewLine),
                Items = new List<string>()
            };

            foreach (var line in Lines)
            {
                var thisAmount = CalculateThisAmount();
                receipt.Items.Add(string.Format("\t{0} x {1} {2} = {3}", line.Quantity, line.Bike.Brand, line.Bike.Model, thisAmount.ToString("C")));
                totalAmount += thisAmount;
            }

            receipt.SubTotal = string.Format("Sub-Total: {0}", totalAmount.ToString("C"));
            tax = totalAmount * TaxRate;
            receipt.Tax = string.Format("Tax: {0}", tax.ToString("C"));
            receipt.Total = string.Format("Total: {0}", (totalAmount + tax).ToString("C"));

            return JsonConvert.SerializeObject(receipt);
        }

        public string Receipt()
        {
            totalAmount = 0d;
            var result = new StringBuilder(string.Format("Order Receipt for {0}{1}", Company, Environment.NewLine));
            foreach (var line in Lines)
            {
                var thisAmount = CalculateThisAmount();
                result.AppendLine(string.Format("\t{0} x {1} {2} = {3}", line.Quantity, line.Bike.Brand, line.Bike.Model, thisAmount.ToString("C")));
                totalAmount += thisAmount;
            }
            result.AppendLine(string.Format("Sub-Total: {0}", totalAmount.ToString("C")));
            tax = totalAmount * TaxRate;
            result.AppendLine(string.Format("Tax: {0}", tax.ToString("C")));
            result.Append(string.Format("Total: {0}", (totalAmount + tax).ToString("C")));
            return result.ToString();
        }

        public string HtmlReceipt()
        {
            var totalAmount = 0d;
            var result = new StringBuilder(string.Format("<html><body><h1>Order Receipt for {0}</h1>", Company));
            if (Lines.Any())
            {
                result.Append("<ul>");
                foreach (var line in Lines)
                {
                    var thisAmount = CalculateThisAmount();
                    result.Append(string.Format("<li>{0} x {1} {2} = {3}</li>", line.Quantity, line.Bike.Brand, line.Bike.Model, thisAmount.ToString("C")));
                    totalAmount += thisAmount;
                }
                result.Append("</ul>");
            }
            result.Append(string.Format("<h3>Sub-Total: {0}</h3>", totalAmount.ToString("C")));
            tax = totalAmount * TaxRate;
            result.Append(string.Format("<h3>Tax: {0}</h3>", tax.ToString("C")));
            result.Append(string.Format("<h2>Total: {0}</h2>", (totalAmount + tax).ToString("C")));
            result.Append("</body></html>");
            return result.ToString();
        }

        public double CalculateThisAmount()
        {
            var thisAmount = 0d;
            foreach (var line in Lines)
            {
                // reset amount to zero
                thisAmount = 0d;

                switch (line.Bike.Price)
                {
                    // let's make use of the ternary operator
                    case Bike.OneThousand:
                        thisAmount += (line.Quantity >= 20)
                            ? line.Quantity * line.Bike.Price * .9d
                            : thisAmount += line.Quantity * line.Bike.Price;
                        break;
                    case Bike.TwoThousand:
                        thisAmount += (line.Quantity >= 10)
                            ? line.Quantity * line.Bike.Price * .8d
                            : thisAmount += line.Quantity * line.Bike.Price;
                        break;
                    case Bike.FiveThousand:
                        thisAmount += (line.Quantity >= 5)
                            ? line.Quantity * line.Bike.Price * .8d
                            : thisAmount += line.Quantity * line.Bike.Price;
                        break;
                    case Bike.SevenThousand:
                        thisAmount += (line.Quantity >= 3)
                            ? line.Quantity * line.Bike.Price * .7d
                            : thisAmount += line.Quantity * line.Bike.Price;
                        break;
                }
            }

            return thisAmount;
        }

        public string XmlReceipt()
        {
            var node = JsonConvert.DeserializeXNode(JsonReceipt(), "Root");
            return node.ToString();
        }

    }
}