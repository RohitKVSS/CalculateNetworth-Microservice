using System;
using System.Collections.Generic;
using CalculateNetworthMicroservice.Models;
using CalculateNetworthMicroservice.Repository;
using NUnit.Framework;
using CalculateNetworthMicroservice.Data;
using CalculateNetworthMicroservice.Service;
using CalculateNetworthMicroservice.Controllers;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CalculateNetWorth_Testing
{
    class ControllerTests
    {
        List<PortfolioDetails> data;
        Mock<IPortfolioService> service_mock = new Mock<IPortfolioService>();
        PortfolioDetails saleDetail1;

        public ControllerTests()
        {
            saleDetail1 = new PortfolioDetails()
            {
                PortfolioId = 1,

                StockList = new List<StockDetails>(),

                MutualFundList = new List<MutualFundDetails>()
                {
                    new MutualFundDetails()
                    {MutualFundName="TATA_Fund",MutualFundUnits=1}
                }

            };
        }

        [SetUp]
        public void Setup()
        {
            data = new List<PortfolioDetails>()
            {
            new PortfolioDetails()
            {
                PortfolioId=1,

                StockList=new List<StockDetails>()
                {
                    new StockDetails()
                    {StockName="SUNPHARMA",StockCount=2},
                    new StockDetails()
                    {StockName="CIPLA",StockCount=2}
                },

                MutualFundList=new List<MutualFundDetails>()
                {
                    new MutualFundDetails()
                    {MutualFundName="TATA_Fund",MutualFundUnits=3},
                    new MutualFundDetails()
                    {MutualFundName="DSP_Flexi_Fund",MutualFundUnits=2}
                }
            },

            new PortfolioDetails()
            {
                PortfolioId=2,

                StockList=new List<StockDetails>()
                {
                    new StockDetails()
                    {StockName="TECHM",StockCount=2},
                    new StockDetails()
                    {StockName="SBIN",StockCount=2}
                },

                MutualFundList=new List<MutualFundDetails>()
                {
                    new MutualFundDetails()
                    {MutualFundName="Aditya_Birla_Fund",MutualFundUnits=3},
                    new MutualFundDetails()
                    {MutualFundName="DSP_Flexi_Fund",MutualFundUnits=2}
                }
            },

            new PortfolioDetails()
            {
                PortfolioId=3,

                StockList=new List<StockDetails>()
                {
                    new StockDetails()
                    {StockName="ADANIPORTS",StockCount=2},
                    new StockDetails()
                    {StockName="HINDALCO",StockCount=1}
                },

                MutualFundList=new List<MutualFundDetails>()
                {
                    new MutualFundDetails()
                    {MutualFundName="Aditya_Birla_Fund",MutualFundUnits=3},
                    new MutualFundDetails()
                    {MutualFundName="SBI_Bluechip_Fund",MutualFundUnits=3}
                }
            },

            new PortfolioDetails()
            {
                PortfolioId=4,

                StockList=new List<StockDetails>()
                {
                    new StockDetails()
                    {StockName="SHREECEM",StockCount=2},
                    new StockDetails()
                    {StockName="HINDALCO",StockCount=3}
                },

                MutualFundList=new List<MutualFundDetails>()
                {
                    new MutualFundDetails()
                    {MutualFundName="ICICI_Prudential_Fund",MutualFundUnits=3},
                    new MutualFundDetails()
                    {MutualFundName="SBI_Bluechip_Fund",MutualFundUnits=3}
                }
            },

            new PortfolioDetails()
            {
                PortfolioId=5,

                StockList=new List<StockDetails>()
                {
                    new StockDetails()
                    {StockName="HINDALCO",StockCount=2}
                },

                MutualFundList=new List<MutualFundDetails>()
                {
                    new MutualFundDetails()
                    {MutualFundName="SBI_Banking_Fund",MutualFundUnits=3},
                    new MutualFundDetails()
                    {MutualFundName="SBI_Bluechip_Fund",MutualFundUnits=3}
                }
            },


        };
        }

        [Test]
        public void GetPortfolioDetails_ValidData_ReturnsOkObjectResult()
        {
            //Creating an object of PortfolioController.
            var portfolioController = new PortfolioController(service_mock.Object);

            //Creating a fake PortfolioController's User.
            //This is the user that the function would be getting from the JWT - (Assumption/Mocking).
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,"t1"),
                    new Claim("PortfolioId","1")
                }
                ));

            portfolioController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            service_mock.Setup(s => s.GetPortfolioDetails(1)).Returns(data[0]);

            var portfolioDetails = portfolioController.GetPortfolioDetails() as OkObjectResult;
            Assert.AreEqual(200, portfolioDetails.StatusCode);
        }

        [Test]
        public void GetPortfolioDetails_InvalidData_ReturnsNoContentResult()
        {
            PortfolioDetails portfolio = null;

            //Creating an object of PortfolioController.
            var portfolioController = new PortfolioController(service_mock.Object);

            //Creating a fake PortfolioController's User.
            //This is the user we would be getting from the JWT - (Assumption/Mocking).
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,"rohit"),
                    new Claim("PortfolioId","7")
                }
                ));
            portfolioController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            service_mock.Setup(s => s.GetPortfolioDetails(7)).Returns(portfolio);

            var portfolioDetails = portfolioController.GetPortfolioDetails() as NoContentResult;
            Assert.AreEqual(204, portfolioDetails.StatusCode);
        }

        [Test]
        public void GetPortfolioDetails_AnyData_Exception_ReturnsInternalServerError()
        {
            //Creating an object of PortfolioController.
            var portfolioController = new PortfolioController(service_mock.Object);

            //Creating a fake PortfolioController's User.
            //This is the user we would be getting from the JWT - (Assumption/Mocking).
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,"t1"),
                    new Claim("PortfolioId","1")
                }
                ));
            portfolioController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            service_mock.Setup(s => s.GetPortfolioDetails(1)).Throws<Exception>();

            var portfolioDetails = portfolioController.GetPortfolioDetails() as ObjectResult;
            Assert.AreEqual(500, portfolioDetails.StatusCode);
        }

        [Test]
        public void AddStock_ValidData_ReturnsOkObjectResult()
        {
            //Creating an object of PortfolioController.
            var portfolioController = new PortfolioController(service_mock.Object);

            //Creating a fake PortfolioController's User.
            //This is the user we would be getting from the JWT - (Assumption/Mocking).
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,"t1"),
                    new Claim("PortfolioId","1")
                }
                ));
            portfolioController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            service_mock.Setup(s => s.AddStock(1, data[0].StockList[0])).Returns("Stock was added successfully");

            var portfolioDetails = portfolioController.AddStock(data[0].StockList[0]) as OkObjectResult;
            Assert.AreEqual(200, portfolioDetails.StatusCode);
        }

        [Test]
        public void AddStock_InValidData_ReturnsNoContentResult()
        {
            StockDetails stockDetails = null;

            //Creating an object of PortfolioController.
            var portfolioController = new PortfolioController(service_mock.Object);

            //Creating a fake PortfolioController's User.
            //This is the user we would be getting from the JWT - (Assumption/Mocking).
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,"t1"),
                    new Claim("PortfolioId","1")
                }
                ));
            portfolioController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            var portfolioDetails = portfolioController.AddStock(stockDetails) as NoContentResult;
            Assert.AreEqual(204, portfolioDetails.StatusCode);
        }

        [Test]
        public void AddStock_AnyData_Exception_ReturnsInternalServerError()
        {
            //Creating an object of PortfolioController.
            var portfolioController = new PortfolioController(service_mock.Object);

            //Creating a fake PortfolioController's User.
            //This is the user we would be getting from the JWT -  (Assumption/Mocking).
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,"t1"),
                    new Claim("PortfolioId","1")
                }
                ));
            portfolioController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // service_mock.Setup(s => s.GetPortfolioDetails(1)).Returns(data[0]);
            service_mock.Setup(s => s.AddStock(1, data[0].StockList[0])).Throws<Exception>();

            var message = portfolioController.AddStock(data[0].StockList[0]) as ObjectResult;
            Assert.AreEqual(500, message.StatusCode);
        }

        [TestCase(1)]
        public void CalculateNetWorth_ValidData_ReturnsOkObjectResult(int portfolioId)
        {
            //Creating an object of PortfolioController and mocking the necessary part.
            var portfolioController = new PortfolioController(service_mock.Object);
            portfolioController.ControllerContext = new ControllerContext();
            portfolioController.ControllerContext.HttpContext = new DefaultHttpContext();
            portfolioController.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer abcdef.ghijkl.mnop";

            string jwt_token = "abcdef.ghijkl.mnop";
            service_mock.Setup(s => s.CalculateNetWorth(portfolioId, jwt_token)).Returns(4525);
            var netWorth = portfolioController.CalculateNetWorth(portfolioId) as OkObjectResult;
            Assert.AreEqual(200, netWorth.StatusCode);
        }

        [TestCase(0)]
        public void CalculateNetWorth_InvalidData_ReturnsNoContentResult(int portfolioId)
        {
            var portfolioController = new PortfolioController(service_mock.Object);
            portfolioController.ControllerContext = new ControllerContext();
            portfolioController.ControllerContext.HttpContext = new DefaultHttpContext();
            portfolioController.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer abcdef.ghijkl.mnop";


            var netWorth = portfolioController.CalculateNetWorth(portfolioId) as NoContentResult;
            Assert.AreEqual(204, netWorth.StatusCode);
        }

        [TestCase(2)]
        [TestCase(7)]
        public void CalculateNetWorth_AnyData_ReturnsInternalServerError(int portfolioId)
        {
            //Creating an object of PortfolioController.
            var portfolioController = new PortfolioController(service_mock.Object);

            string jwt_token = "abcdefghijklmnop";

            service_mock.Setup(s => s.CalculateNetWorth(portfolioId, jwt_token)).Throws<Exception>();
            var netWorth = portfolioController.CalculateNetWorth(portfolioId) as ObjectResult;
            Assert.AreEqual(500, netWorth.StatusCode);
        }

        [Test]
        public void SellAssets_ValidData_ReturnsOkObjectResult()
        {

            //Creating an object of PortfolioController and mocking the necessary part.
            var portfolioController = new PortfolioController(service_mock.Object);
            portfolioController.ControllerContext = new ControllerContext();
            portfolioController.ControllerContext.HttpContext = new DefaultHttpContext();
            portfolioController.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer abcdef.ghijkl.mnop";

            string jwt_token = "abcdef.ghijkl.mnop";
            int currentDetail = 1;

            //Creating an object of AssetSaleResponse.
            AssetSaleResponse assetSaleResponse = new AssetSaleResponse();
            assetSaleResponse.NetWorth = 4192;
            assetSaleResponse.SaleStatus = true;

            service_mock.Setup(s => s.SellAssets(currentDetail, saleDetail1, jwt_token)).Returns(assetSaleResponse);
            var assetSaleResponseResult = portfolioController.SellAssets(currentDetail, saleDetail1) as OkObjectResult;
            Assert.AreEqual(200, assetSaleResponseResult.StatusCode);
        }

        [Test]
        public void SellAssets_InvalidData_ReturnsNoContentResult()
        {
            PortfolioDetails saleDetail2 = null;
            int currentDetail = 1;

            //Creating an object of PortfolioController and mocking the necessary part.
            var portfolioController = new PortfolioController(service_mock.Object);
            portfolioController.ControllerContext = new ControllerContext();
            portfolioController.ControllerContext.HttpContext = new DefaultHttpContext();
            portfolioController.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer abcdef.ghijkl.mnop";

            //string jwt_token = "abcdef.ghijkl.mnop";
            var assetSaleResponseResult = portfolioController.SellAssets(currentDetail, saleDetail2) as NoContentResult;
            Assert.AreEqual(204, assetSaleResponseResult.StatusCode);
        }

        [Test]
        public void SellAssets_AnyData_Exception_ReturnsInternalServerError()
        {
            //Creating an object of PortfolioController and mocking the necessary part.
            var portfolioController = new PortfolioController(service_mock.Object);
            portfolioController.ControllerContext = new ControllerContext();
            portfolioController.ControllerContext.HttpContext = new DefaultHttpContext();
            portfolioController.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer abcdef.ghijkl.mnop";

            string jwt_token = "abcdef.ghijkl.mnop";
            int currentDetail = 1;

            service_mock.Setup(s => s.SellAssets(currentDetail, saleDetail1, jwt_token)).Throws<Exception>();
            var assetSaleResponseResult = portfolioController.SellAssets(currentDetail, saleDetail1) as ObjectResult;
            Assert.AreEqual(500, assetSaleResponseResult.StatusCode);
        }

        [Test]
        public void AddMutualFund_ValidData_ReturnsOkObjectResult()
        {
            //Creating an object of PortfolioController.
            var portfolioController = new PortfolioController(service_mock.Object);

            //Creating a fake PortfolioController's User.
            //This is the user we would be getting from the JWT - (Assumption/Mocking).
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,"t1"),
                    new Claim("PortfolioId","1")
                }
                ));
            portfolioController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            service_mock.Setup(s => s.AddMutualFund(1, data[0].MutualFundList[0])).Returns("Mutual Fund was added successfully");

            var portfolioDetails = portfolioController.AddMutualFund(data[0].MutualFundList[0]) as OkObjectResult;
            Assert.AreEqual(200, portfolioDetails.StatusCode);
        }

        [Test]
        public void AddMutualFund_InvalidData_ReturnsNoContentResult()
        {
            MutualFundDetails mutualFundDetails = null;

            //Creating an object of PortfolioController.
            var portfolioController = new PortfolioController(service_mock.Object);

            //Creating a fake PortfolioController's User.
            //This is the user we would be getting from the JWT - (Assumption/Mocking).
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,"t1"),
                    new Claim("PortfolioId","1")
                }
                ));
            portfolioController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            var portfolioDetails = portfolioController.AddMutualFund(mutualFundDetails) as NoContentResult;
            Assert.AreEqual(204, portfolioDetails.StatusCode);
        }

        [Test]
        public void AddMutualFund_AnyData_Exception_ReturnInternalServerError()
        {
            //Creating an object of PortfolioController.
            var portfolioController = new PortfolioController(service_mock.Object);

            //Creating a fake PortfolioController's User.
            //This is the user we would be getting from the JWT - (Assumption/Mocking).
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,"t1"),
                    new Claim("PortfolioId","1")
                }
                ));
            portfolioController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            service_mock.Setup(s => s.AddMutualFund(1, data[0].MutualFundList[0])).Throws<Exception>();

            var message = portfolioController.AddMutualFund(data[0].MutualFundList[0]) as ObjectResult;
            Assert.AreEqual(500, message.StatusCode);
        }


    }





}
