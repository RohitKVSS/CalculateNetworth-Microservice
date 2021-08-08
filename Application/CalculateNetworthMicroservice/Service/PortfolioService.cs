using CalculateNetworthMicroservice.Data;
using CalculateNetworthMicroservice.Models;
using CalculateNetworthMicroservice.Repository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateNetworthMicroservice.Service
{
    public class PortfolioService:IPortfolioService
    {
        //Creating an object for logging purposes.
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(PortfolioService));


        //Creating an object to access the repository functions.
        private IPortfolioRepository _portfolioRepository;

        public PortfolioService(IPortfolioRepository portfolioRepository)
        {
            _portfolioRepository = portfolioRepository;
        }


        /// <summary>
        /// To get the portfolio details.
        /// </summary>
        /// <param name="portfolioId"></param>
        /// <returns>PortfolioDetails</returns>
        public PortfolioDetails GetPortfolioDetails(int portfolioId)
        {
            try
            {
                var portfolioDetails = _portfolioRepository.GetPortfolioDetails(portfolioId);
                if(portfolioDetails!=null)
                _log4net.Info("Data found in GetPortfolioDetails(Service) having portfolio id: " + portfolioId);
                else
                _log4net.Info("No Data found in GetPortfolioDetails(Service) having portfolio id: " + portfolioId);
                return portfolioDetails;
            }
            catch(Exception)
            {
                _log4net.Error("Exception occurred in GetPortfolioDetails(Service) having portfolio id: " + portfolioId);
                return null;
            }
        }


        /// <summary>
        /// To add stock details to a user's portfolio.
        /// </summary>
        /// <param name="portfolioId"></param>
        /// <param name="stockDetails"></param>
        /// <returns>Message(string)</returns>
        public string AddStock(int portfolioId, StockDetails stockDetails)
        {
            try
            {
                var portfolioDetails = _portfolioRepository.GetPortfolioDetails(portfolioId);
                
                if (portfolioDetails != null)
                {
                    _log4net.Info("Data found in AddStock(Service) having portfolio id: " + portfolioId);
                    return _portfolioRepository.AddStock(portfolioDetails, stockDetails);
                }
                else
                {
                    _log4net.Info("No Data found in AddStock(Service) having portfolio id: " + portfolioId);
                    return "Details couldn't be updated.Please try later.";
                }
            }
            catch(Exception)
            {
                _log4net.Error("Exception occurred in AddStock(Service) having portfolio id: " + portfolioId);
                return "Sorry,an error occurred.Please try later";
            }
        }


        /// <summary>
        /// To add mutual fund details to a user's portfolio.
        /// </summary>
        /// <param name="portfolioId"></param>
        /// <param name="mutualFundDetails"></param>
        /// <returns>Message(string)</returns>
        public string AddMutualFund(int portfolioId, MutualFundDetails mutualFundDetails)
        {
            try
            {
                var portfolioDetails = _portfolioRepository.GetPortfolioDetails(portfolioId);
                
                if (portfolioDetails != null)
                {
                    _log4net.Info("Data found in AddMutualFund(Service) having portfolio id: " + portfolioId);
                    return _portfolioRepository.AddMutualFund(portfolioDetails, mutualFundDetails);
                }
                else
                {
                    _log4net.Info("No Data found in AddMutualFund(Service) having portfolio id: " + portfolioId);
                    return "Details couldn't be updated.Please try later.";
                }
            }
            catch (Exception)
            {
                _log4net.Error("Exception occurred in AddMutualFund(Service) having portfolio id: " + portfolioId);
                return "Sorry,an error occurred.Please try later";
            }
        }


        /// <summary>
        /// To calculate the networth value of a user's portfolio. 
        /// </summary>
        /// <param name="portfolioId"></param>
        /// <param name="jwt_token"></param>
        /// <returns>NetWorthValue(int)</returns>
        public int CalculateNetWorth(int portfolioId,string jwt_token)
        {
            int stockNetWorth = 0;
            int mutualFundNetWorth = 0;
            try
            {
                var portfolioDetails = _portfolioRepository.GetPortfolioDetails(portfolioId);

                //If the user has no portfolio then his networth is -1,that means certain error is happening in backend.
                if (portfolioDetails != null)
                {
                    //In case his stock list is empty, then no need to calculate stockNetWorth.
                    if (portfolioDetails.StockList.Count != 0)
                    {
                        foreach (var stock in portfolioDetails.StockList)
                        {
                            //If there is no units of stock, no need of API CALL.
                            if (stock.StockCount != 0)
                            {
                                var stockValue = _portfolioRepository.GetStockValue(stock.StockName, jwt_token);
                                
                                if (stockValue == -1)
                                    throw new Exception();

                                stockNetWorth += (stockValue * stock.StockCount);
                            }
                            else
                                stockNetWorth += 0;

                        }
                    }

                    //In case his mutualfund list is empty, then no need to calculate mutualFundNetWorth.
                    if (portfolioDetails.MutualFundList.Count != 0)
                    {
                        foreach (var mutualFund in portfolioDetails.MutualFundList)
                        {
                            //If there is no units of mutual fund,no need of API call.
                            if (mutualFund.MutualFundUnits != 0)
                            {
                                var mutualFundValue = _portfolioRepository.GetMutualFundValue(mutualFund.MutualFundName, jwt_token);
                                
                                if (mutualFundValue == -1)
                                    throw new Exception();

                                mutualFundNetWorth += (mutualFundValue * mutualFund.MutualFundUnits);
                            }
                            else
                                mutualFundNetWorth += 0;
                        }
                    }
                    _log4net.Info("NetWorth successfully calculated in CalculateNetWorth(Service) having portfolio id: " + portfolioId);
                    return (stockNetWorth + mutualFundNetWorth);
                }
                else
                {
                    _log4net.Info("No Data found in CalculateNetWorth(Service) having portfolio id: " + portfolioId);
                    return -1;
                }
                    
            }
            catch(Exception)
            {
                _log4net.Info("Exception occurred in CalculateNetWorth(Service) having portfolio id: " + portfolioId+" while calculating NetWorth");
                return -1;
            }
        }

        /// <summary>
        /// To sell the assets of an user.
        /// currentDetail represents current portfolio of user, while saleDetail represents selling details(stock/mutualfund) of user.
        /// </summary>
        /// <param name="currentDetail"></param>
        /// <param name="saleDetail"></param>
        /// <param name="jwt_token"></param>
        /// <returns>AssetSaleResponse</returns>
        public AssetSaleResponse SellAssets(int currentDetail, PortfolioDetails saleDetail, string jwt_token)
        {
            int netWorth;
            AssetSaleResponse assetSaleResponse = new AssetSaleResponse();
            try
            {
                var portfolioDetails = _portfolioRepository.GetPortfolioDetails(currentDetail);
                
                if(portfolioDetails!=null)
                {
                    //User prompted to buy mutual funds.
                    if(saleDetail.StockList.Count==0)
                    {
                        //Extracting out the to-be-sold mutual fund name and its count.
                        var mutualFundName = saleDetail.MutualFundList[0].MutualFundName;
                        var mutualFundUnits = saleDetail.MutualFundList[0].MutualFundUnits;

                        
                        //Finding that mutual fund.
                        var mutualFund = (from m in portfolioDetails.MutualFundList
                                          where m.MutualFundName == mutualFundName
                                          select m).First();

                        //Updating the count.
                        if (mutualFundUnits > mutualFund.MutualFundUnits)
                            throw new Exception();
                        else
                            mutualFund.MutualFundUnits -= mutualFundUnits;

                        _log4net.Info(mutualFundName + " shares sold successfully in SellAssets(Service)");


                    }
                    else if(saleDetail.MutualFundList.Count==0)
                    {
                        //Extracting out the to-be-sold stock name and its count.
                        var stockName = saleDetail.StockList[0].StockName;
                        var stockUnits = saleDetail.StockList[0].StockCount;

                        //Finding that stock.
                        var stock = (from s in portfolioDetails.StockList
                                     where s.StockName == stockName
                                     select s).First();


                        //Updating the count.
                        if (stockUnits > stock.StockCount)
                            throw new Exception();
                        else
                            stock.StockCount -= stockUnits;

                        _log4net.Info(stockName + " shares sold successfully in SellAssets(Service)");
                    }

                   
                    netWorth = CalculateNetWorth(currentDetail, jwt_token);
                    assetSaleResponse.NetWorth = netWorth;
                    assetSaleResponse.SaleStatus = true;

                    //LogResponse_DB(assetSaleResponse);
                }
                else
                {
                    _log4net.Info("Data not found for SellAssets(Service) having portfolio id: " + currentDetail);
                    assetSaleResponse.SaleStatus = false;
                    
                }

            }
            catch(Exception)
            {
                _log4net.Error("Exception occurred in SellAssets(Service) having portfolio id: " + currentDetail);
                assetSaleResponse.SaleStatus = false;
            }

            return assetSaleResponse;
        }

        /*
        /// <summary>
        /// To store the API responses to DB.
        /// </summary>
        /// <param name="assetSaleResponse"></param>
        public void LogResponse_DB(AssetSaleResponse assetSaleResponse)
        {
            try
            {
                _portfolioRepository.LogResponse_DB(assetSaleResponse);
            }
            catch(Exception e)
            {
                _log4net.Error("Error message is:" + e.Message);
            }
        }
        */


    }
}
