using CalculateNetworthMicroservice.Data;
using CalculateNetworthMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace CalculateNetworthMicroservice.Repository
{
    public class PortfolioRepository:IPortfolioRepository
    {

        //Default constructor.
        public PortfolioRepository(){}

        /*Creating an object to store responses in DB.
        PortfolioContext _portfolioContext;

        public PortfolioRepository(PortfolioContext portfolioContext)
        {
            _portfolioContext = portfolioContext;
        }
        */

        //Creating an object for logging purposes.
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(PortfolioRepository));


        /// <summary>
        /// To return the corresponding portfolio details object.
        /// </summary>
        /// <param name="portfolioId"></param>
        /// <returns>PortfolioDetails</returns>
        public PortfolioDetails GetPortfolioDetails(int portfolioId)
        {
            try
            {
                var portfolio = (from p in PortfolioData.portfolioDetails
                              where p.PortfolioId == portfolioId
                              select p).FirstOrDefault();
                _log4net.Info("Data fetched in GetPortfolioDetails(Repository) having portfolio id: " + portfolioId);
                return portfolio;
            }
            catch(Exception)
            {
                _log4net.Info("Exception Occurred for GetPortfolioDetails(Repository) having portfolio id: " + portfolioId);
                return null;
            }
        }

        /// <summary>
        /// To get the value of a stock.
        /// </summary>
        /// <param name="stockName"></param>
        /// <returns>Stock's Value</returns>
        public int GetStockValue(string stockName,string jwt_token)
        {
            int stockValue = -1;
            try
            {
                using(var httpClient=new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("https://portfolioauth.azurewebsites.net");
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwt_token);
                    

                    HttpResponseMessage httpResponseMessage = httpClient.GetAsync("/api/Stock/dailySharePrice?stockName=" + stockName).Result;
                    string apiResponse = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    var stockList = JsonConvert.DeserializeObject<List<Stock>>(apiResponse);

                    //Corner case.
                    if (stockList == null)
                        throw new Exception();

                    _log4net.Info("Stock Details fetched in GetStockValue(Repository) is: " + stockList[0].StockName+" "+stockList[0].StockValue);
                    stockValue = stockList[0].StockValue;
                }
            }
            catch(Exception e)
            {
                _log4net.Error("Exception Occurred in GetStockValue(Repository) for stockName: " + stockName+" "+e.Message);
            }
            return stockValue;
        }


        /// <summary>
        /// To get the value of a mutual fund.
        /// </summary>
        /// <param name="mutualFundName"></param>
        /// <param name="jwt_token"></param>
        /// <returns>MutualFund's Value(</returns>
        public int GetMutualFundValue(string mutualFundName, string jwt_token)
        {
            int mutualFundValue = -1;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("https://portfolioauth.azurewebsites.net");
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwt_token);

                    HttpResponseMessage httpResponseMessage = httpClient.GetAsync("/api/MutualFund/mutualFundNav?mutualFundName=" + mutualFundName).Result;
                    string apiResponse = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    var mutualFundList = JsonConvert.DeserializeObject<List<MutualFund>>(apiResponse);

                    //Corner case.
                    if (mutualFundList == null)
                        throw new Exception();

                    _log4net.Info("MutualFund Details fetched in GetMutualFundValue(Repository) is: " + mutualFundList[0].MutualFundName+" "+mutualFundList[0].MutualFundValue);
                    mutualFundValue = mutualFundList[0].MutualFundValue;

                }
            }
            catch (Exception e)
            {
                _log4net.Error("Exception Occurred in GetMutualFundValue(Repository) for mutualFundName: " + mutualFundName+" "+e.Message);
            }
            return mutualFundValue;
        }

        /// <summary>
        /// To add stock to a user portfolio.
        /// </summary>
        /// <param name="portfolioDetails"></param>
        /// <param name="stockDetails"></param>
        /// <returns>Message(string)</returns>
        public string AddStock(PortfolioDetails portfolioDetails, StockDetails stockDetails)
        {
            try
            {
                //stockDetails should be a valid data.
                if (!(stockDetails.StockName == "SUNPHARMA" || stockDetails.StockName == "TECHM" || stockDetails.StockName == "CIPLA" || stockDetails.StockName == "ADANIPORTS" || stockDetails.StockName == "SHREECEM" || stockDetails.StockName == "SBIN" || stockDetails.StockName == "HINDALCO"))
                throw new Exception();
                

                var stock = (from s in portfolioDetails.StockList
                             where s.StockName== stockDetails.StockName
                             select s).FirstOrDefault();

                //If the stock is present already, then we have to update its count.
                if (stock != null)
                    stock.StockCount += stockDetails.StockCount;

                //Else,include it in the portfolio of the user.
                else
                    portfolioDetails.StockList.Add(stockDetails);

                _log4net.Info("Successful Updation in AddStock(Repository) for stockname: " + stockDetails.StockName);
                return "Stock was added successfully";
                
            }
            catch(Exception)
            {
                _log4net.Error("Exception Occurred in AddStock(Repository) for stockname: " + stockDetails.StockName);
                return "Error while adding the Stock.Try again!";
            }
        }


        /// <summary>
        /// To add mutual fund to a user portfolio.
        /// </summary>
        /// <param name="portfolioDetails"></param>
        /// <param name="mutualFundDetails"></param>
        /// <returns>Message(string)</returns>
        public string AddMutualFund(PortfolioDetails portfolioDetails, MutualFundDetails mutualFundDetails)
        {
            try
            {
                //mutualFundDetails must be a valid data.
                if (!(mutualFundDetails.MutualFundName == "ICICI_Prudential_Fund" || mutualFundDetails.MutualFundName == "SBI_Banking_Fund" || mutualFundDetails.MutualFundName == "TATA_Fund" || mutualFundDetails.MutualFundName == "SBI_Bluechip_Fund" || mutualFundDetails.MutualFundName == "DSP_Flexi_Fund" || mutualFundDetails.MutualFundName == "Aditya_Birla_Fund" || mutualFundDetails.MutualFundName == "Quant_Active_Fund"))
                    throw new Exception();


                var mutualFund = (from m in portfolioDetails.MutualFundList
                                  where m.MutualFundName == mutualFundDetails.MutualFundName
                                  select m).FirstOrDefault();

                //If the mutual fund is already present, then just update its count.
                if (mutualFund != null)
                    mutualFund.MutualFundUnits += mutualFundDetails.MutualFundUnits;
                //Else,include it in the portfolio of user.
                else
                    portfolioDetails.MutualFundList.Add(mutualFundDetails);
                _log4net.Info("Successful Updation in AddMutualFund(Repository) for mutualFund name : " + mutualFundDetails.MutualFundName);
                return "MutualFund was added successfully";
            }
            catch(Exception)
            {
                _log4net.Error("Exception Occurred in AddMutualFund(Repository) for mutualFund name: " + mutualFundDetails.MutualFundName);
                return "Error while adding the mutual fund.Try again!";
            }
        }


        /*
        /// <summary>
        /// To store API responses to Database.
        /// </summary>
        /// <param name="assetSaleResponse"></param>
        public void LogResponse_DB(AssetSaleResponse assetSaleResponse)
        {
            try
            {
                _portfolioContext.AssetSaleResponses.Add(assetSaleResponse);
                _portfolioContext.SaveChanges();

            }
            catch(Exception e)
            {
                _log4net.Error("Error message is: " + e.Message);
            }
        }
        */

    }
}
