using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateNetworthMicroservice.Models
{
    public class AssetSaleResponse
    {
        public bool SaleStatus {get; set;}
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NetWorth {get; set;}
    }
}
