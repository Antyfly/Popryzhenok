using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Popryzhenok.DatabaseModel
{
    public partial class Agent
    {
        public string Sales
        {
            get
            {

                List<ProductSale> productSales = ProductSale.Where(p => p.AgentID == ID && p.SaleDate > DateTime.Today.AddDays(-365)).ToList();
                int countSales = 0;
                foreach (var i in productSales)
                {
                    countSales += i.ProductCount;
                }
                return countSales.ToString();
            }
        }

        public string Discount
        {
            get
            {
                List<ProductSale> productSales = ProductSale.Where(ps => ps.AgentID == ID).ToList();
                List<Product> productCost = DatabaseHelper.GetProduct();

                decimal sumProduct = 0;
                foreach (var i in productSales)
                {
                    sumProduct += i.ProductCount * productCost.Where(w => w.ID == i.ProductID).Select(s => s.MinCostForAgent).FirstOrDefault();
                }


                if (sumProduct >= 10000 && sumProduct <= 50000)
                {
                    return "5 %";
                }
                else if (sumProduct >= 50000 && sumProduct <= 150000)
                {
                    return "10 %";
                }
                else if (sumProduct >= 150000 && sumProduct <= 500000)
                {
                    return "20 %";
                }
                else if (sumProduct >= 500000)
                {
                    return "25 %";
                }
                else
                {
                    return "0 %";
                }
            }
        }

        public object Background
        {
            get
            {
                var background = Discount;
                if (background == "25 %")
                {
                    return new System.Windows.Media.BrushConverter().ConvertFrom("#FF53E059");
                }
                else
                {
                    return new System.Windows.Media.BrushConverter().ConvertFrom("#ffffff");
                }
            }
        }

        public string LogoPath
        {
            get
            {
                var logo = Logo;
                if (logo is null)
                {
                    return "/Image/picture.png";
                }
                else
                {
                    return Directory.GetCurrentDirectory() + Logo.ToString();
                }
            }
        }
    }
}
