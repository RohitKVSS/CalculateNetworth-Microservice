using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateNetworthMicroservice.Models
{
    public class PortfolioDetails
    {
        [Key]
        public int PortfolioId {get; set;}
        public List<StockDetails> StockList {get; set;}
        public List<MutualFundDetails> MutualFundList {get; set;}

    }
}
