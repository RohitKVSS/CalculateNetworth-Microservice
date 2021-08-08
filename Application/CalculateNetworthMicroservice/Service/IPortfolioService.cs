using CalculateNetworthMicroservice.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateNetworthMicroservice.Service
{
    public interface IPortfolioService
    {
        public int CalculateNetWorth(int portfolioId,string jwt_token);
        public AssetSaleResponse SellAssets(int currentDetail, PortfolioDetails saleDetail, string jwt_token);
        public PortfolioDetails GetPortfolioDetails(int portfolioId);
        public string AddStock(int portfolioId,StockDetails stockDetails);
        public string AddMutualFund(int portfolioId, MutualFundDetails mutualFundDetails);
       //public void LogResponse_DB(AssetSaleResponse assetSaleResponse);
    }
}
