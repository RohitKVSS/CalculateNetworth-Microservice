using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateNetworthMicroservice.Models
{
    public class MutualFundDetails
    {
        public string MutualFundName {get; set;}
        public int MutualFundUnits {get; set;}
    }
}
