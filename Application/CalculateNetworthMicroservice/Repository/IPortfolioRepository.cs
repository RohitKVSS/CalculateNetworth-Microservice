using CalculateNetworthMicroservice.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateNetworthMicroservice.Repository
{
    public interface IPortfolioRepository
    {
        public int GetStockValue(string stockName, string jwt_token);
        public int GetMutualFundValue(string mutualFundName, string jwt_token);
        public PortfolioDetails GetPortfolioDetails(int portfolioId);
        public string AddStock(PortfolioDetails portfolioDetails,StockDetails stockDetails);
        public string AddMutualFund(PortfolioDetails portfolioDetails, MutualFundDetails mutualFundDetails);
       //public void LogResponse_DB(AssetSaleResponse assetSaleResponse);
    }
}
