using CalculateNetworthMicroservice.Models;
using CalculateNetworthMicroservice.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CalculateNetworthMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PortfolioController : ControllerBase
    {
        //Creating an object for logging purposes.
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(PortfolioController));

        //Creating object to access the service.
        private IPortfolioService _portfolioService;

        public PortfolioController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }


        /// <summary>
        /// To get the portfolio details of a user.
        /// </summary>
        /// <returns>PortfolioDetails</returns>
        [HttpGet]
        [Route("portfolioDetails/")]
        public IActionResult GetPortfolioDetails()
        {
            try
            {
                //Fetching the portfolio id from JWT token.
                List<Claim> claims = User.Claims.ToList();
                int portfolioId = Convert.ToInt32(claims.First(claim => claim.Type == "PortfolioId").Value);

                var portfolioDetails = _portfolioService.GetPortfolioDetails(portfolioId);

                if (portfolioDetails == null)
                {
                    _log4net.Info("Data not fetched in GetPortfolioDetails(Controller) having portfolio Id: " + portfolioId);
                    return new NoContentResult();
                }
                else
                {
                    _log4net.Info("Data fetched in GetPortfolioDetails(Controller) having portfolio Id: " + portfolioId);
                    return new OkObjectResult(portfolioDetails);
                }
                
            }
            catch(Exception)
            {
                _log4net.Error("Exception Occurred in GetPortfolioDetails(Controller)");
                return BadRequest(new { message = "Server Busy. Please try again later." });
            }
        }


        /// <summary>
        /// To add a stock to the user's portfolio.
        /// </summary>
        /// <param name="stockDetails"></param>
        /// <returns>Message(string)</returns>
        [HttpPost]
        [Route("addStock/")]
        public IActionResult AddStock([FromBody]StockDetails stockDetails)
        {
            try
            {
                //Fetching the portfolio id from JWT token.
                List<Claim> claims = User.Claims.ToList();
                int portfolioId = Convert.ToInt32(claims.First(claim => claim.Type == "PortfolioId").Value);

                if (!(ModelState.IsValid) || stockDetails == null)
                {
                    _log4net.Info("Invalid input in AddStock(Controller) having portfolio Id: " + portfolioId);
                    return new NoContentResult();
                }
                else
                {
                    _log4net.Info("Valid input in AddStock(Controller) having portfolio Id: " + portfolioId);
                    return new OkObjectResult(new { message = _portfolioService.AddStock(portfolioId, stockDetails) });
                }

            }
            catch(Exception)
            {
                _log4net.Error("Exception Occurred in AddStock(Controller)");
                return BadRequest(new { message = "Server Busy. Please try again later." });
            }
        }


        /// <summary>
        /// To add mutual fund to a user's portfolio.
        /// </summary>
        /// <param name="mutualFundDetails"></param>
        /// <returns>Message(string)</returns>
        [HttpPost]
        [Route("addMutualFund/")]
        public IActionResult AddMutualFund([FromBody]MutualFundDetails mutualFundDetails)
        {
            try
            {
                //Fetching the portfolio id from JWT token.
                List<Claim> claims = User.Claims.ToList();
                int portfolioId = Convert.ToInt32(claims.First(claim => claim.Type == "PortfolioId").Value);

                if(!(ModelState.IsValid)||mutualFundDetails==null)
                {
                    _log4net.Info("Invalid input in AddMutualFund(Controller) having portfolio Id: " + portfolioId);
                    return new NoContentResult();
                }
                else
                {
                    _log4net.Info("Valid input in AddMutualFund(Controller) having portfolio Id: " + portfolioId);
                    return new OkObjectResult(new { message = _portfolioService.AddMutualFund(portfolioId, mutualFundDetails) });
                }

            }
            catch(Exception)
            {
                _log4net.Error("Exception Occurred in AddMutualFund(Controller)");
                return BadRequest(new { message = "Server Busy. Please try again later." });
            }
        }


        /// <summary>
        /// To calculate the networth of a user's portfolio.
        /// </summary>
        /// <param name="portfolioId"></param>
        /// <returns>Networth</returns>
        [HttpGet]
        [Route("calculateNetWorth/{portfolioId}")]
        public IActionResult CalculateNetWorth(int portfolioId)
        {
            try
            {
                //Fetching out the data from the token.
                var jwt_token = Request.Headers["Authorization"].ToString().Substring(7);

                if (portfolioId<1||portfolioId>5)
                {
                    _log4net.Info("Invalid input in  CalculateNetWorth(Controller) having portfolio Id: " + portfolioId);
                    return new NoContentResult();
                }
                else
                {
                    _log4net.Info("Valid input in AddMutualFund(Controller) having portfolio Id: " + portfolioId);

                    int netWorth = _portfolioService.CalculateNetWorth(portfolioId, jwt_token);

                    if (netWorth == -1)
                        return new NoContentResult();
                    else
                        return new OkObjectResult(new { price = netWorth });
                }
            }
            catch (Exception e)
            {
                _log4net.Error("Exception Occurred in CalculateNetWorth(Controller) having portfolio id: "+portfolioId+" "+e.Message);
                return BadRequest(new { message = "Server Busy. Please try again later." });
            }

        }


        /// <summary>
        /// To sell certain portfolio of a user.
        /// </summary>
        /// <param name="currentDetail"></param>
        /// <param name="saleDetail"></param>
        /// <returns>AssestSaleResponse</returns>
        [HttpPost]
        [Route("sellAssets/{currentDetail}")]
        public IActionResult SellAssets([FromRoute]int currentDetail, [FromBody]PortfolioDetails saleDetail)
        {
            try
            {
                //Fetching out the data from the token.
                var jwt_token = Request.Headers["Authorization"].ToString().Substring(7);

                if(!(ModelState.IsValid)||saleDetail==null)
                {
                    _log4net.Info("Invalid input in  SellAssets(Controller) having portfolio Id: " + currentDetail);
                    return new NoContentResult();
                }
                else
                {
                    _log4net.Info("Valid input in  SellAssets(Controller) having portfolio Id: " + currentDetail);
                    AssetSaleResponse assetSaleResponse = _portfolioService.SellAssets(currentDetail, saleDetail, jwt_token);

                    if (assetSaleResponse.SaleStatus == false)
                        return new NoContentResult();
                    else
                        return new OkObjectResult(assetSaleResponse);
                }
            }
            catch(Exception)
            {
                _log4net.Error("Exception happened in SellAssets(Contoller) having portfolio id: " + currentDetail);
                return BadRequest(new { message = "Server Busy. Please try again later." });
            }
        }


    }
}
