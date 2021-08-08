using System;
using System.Collections.Generic;
using CalculateNetworthMicroservice.Models;
using CalculateNetworthMicroservice.Repository;
using NUnit.Framework;
using CalculateNetworthMicroservice.Data;
using CalculateNetworthMicroservice.Service;
using Moq;

namespace CalculateNetWorth_Testing
{
    class ServiceTests
    {
        List<PortfolioDetails> data;
        PortfolioService portfolioService;
        Mock<IPortfolioRepository> mock = new Mock<IPortfolioRepository>();
        Mock<IPortfolioService> service_mock = new Mock<IPortfolioService>();
        PortfolioDetails saleDetail1;

        string message;

        public ServiceTests()
        {
            portfolioService = new PortfolioService(mock.Object);
            
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

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetPortfolioDetails_ValidData_ReturnsPortfolioDetails(int portfolioId)
        {
            mock.Setup(r => r.GetPortfolioDetails(1)).Returns(data[0]);
            mock.Setup(r => r.GetPortfolioDetails(2)).Returns(data[1]);
            mock.Setup(r => r.GetPortfolioDetails(3)).Returns(data[2]);

            var portfolioDetails = portfolioService.GetPortfolioDetails(portfolioId);
            Assert.IsNotNull(portfolioDetails);
        }

        [TestCase(0)]
        [TestCase(99)]
        public void GetPortfolioDetails_InvalidData_ReturnsNull(int portfolioId)
        {
            PortfolioDetails portfolio = null;
            mock.Setup(r => r.GetPortfolioDetails(portfolioId)).Returns(portfolio);

            var portfolioDetails = portfolioService.GetPortfolioDetails(portfolioId);
            Assert.IsNull(portfolioDetails);
        }

        [TestCase(1)]
        [TestCase(99)]
        public void GetPortfolioDetails_AnyData_ReturnsNull(int portfolioId)
        {
            mock.Setup(r => r.GetPortfolioDetails(portfolioId)).Throws<Exception>();

            var portfolioDetails = portfolioService.GetPortfolioDetails(portfolioId);
            Assert.IsNull(portfolioDetails);
        }

        [TestCase(1,"eythdjf.skjfdaskjgadfjbka.jsdlkfalksdjas")]
        public void CalculateNetWorth_ValidData_ReturnsInt(int portfolioId,string jwt_token)
        {
            mock.Setup(r => r.GetPortfolioDetails(1)).Returns(data[0]);
            mock.Setup(r => r.GetStockValue(data[0].StockList[0].StockName, jwt_token)).Returns(774);
            mock.Setup(r => r.GetStockValue(data[0].StockList[1].StockName, jwt_token)).Returns(921);
            mock.Setup(r => r.GetMutualFundValue(data[0].MutualFundList[0].MutualFundName, jwt_token)).Returns(333);
            mock.Setup(r => r.GetMutualFundValue(data[0].MutualFundList[1].MutualFundName, jwt_token)).Returns(68);

            int networth = portfolioService.CalculateNetWorth(portfolioId, jwt_token);
            Assert.AreEqual(4525, networth);
        }

        [TestCase(0, "eythdjf.skjfdaskjgadfjbka.jsdlkfalksdjas")]
        public void CalculateNetWorth_InvalidData_ReturnsInt(int portfolioId, string jwt_token)
        {
            PortfolioDetails portfolio = null;
            mock.Setup(r => r.GetPortfolioDetails(0)).Returns(portfolio);

            int networth = portfolioService.CalculateNetWorth(portfolioId, jwt_token);
            Assert.AreEqual(-1, networth);
        }

        [TestCase(1, "eythdjf.skjfdaskjgadfjbka.jsdlkfalksdjas")]
        [TestCase(0, "eythdjf.skjfdaskjgadfjbka.jsdlkfalksdjas")]
        public void CalculateNetWorth_AnyData_Exception_ReturnsInt(int portfolioId, string jwt_token)
        {
            mock.Setup(r => r.GetPortfolioDetails(portfolioId)).Throws<Exception>();

            int networth = portfolioService.CalculateNetWorth(portfolioId, jwt_token);
            Assert.AreEqual(-1, networth);
        }


        [Test]
        public void SellAssets_ValidData_ReturnsAssetsSaleResponse()
        {
            mock.Setup(r => r.GetPortfolioDetails(1)).Returns(data[0]);
            service_mock.Setup(s => s.CalculateNetWorth(1, "eythdjf.skjfdaskjgadfjbka.jsdlkfalksdjas")).Returns(4192);
            AssetSaleResponse assetSaleResponse=portfolioService.SellAssets(data[0].PortfolioId,saleDetail1,"eythdjf.skjfdaskjgadfjbka.jsdlkfalksdjas");

            Assert.AreEqual(2, data[0].MutualFundList[0].MutualFundUnits);
            Assert.AreEqual(assetSaleResponse.NetWorth, 4192);
            Assert.AreEqual(assetSaleResponse.SaleStatus, true);
        }

        [Test]
        public void SellAssets_InvalidData_ReturnsAssetsSaleResponse()
        {
            PortfolioDetails portfolio = null;
            mock.Setup(r => r.GetPortfolioDetails(99)).Returns(portfolio);
            AssetSaleResponse assetSaleResponse = portfolioService.SellAssets(99, saleDetail1, "eythdjf.skjfdaskjgadfjbka.jsdlkfalksdjas");

            Assert.AreEqual(assetSaleResponse.NetWorth, 0);
            Assert.AreEqual(assetSaleResponse.SaleStatus, false);
        }

        [Test]
        public void SellAssets_AnyData_Exception_ReturnsNull()
        {
            mock.Setup(r => r.GetPortfolioDetails(1)).Throws<Exception>();
            AssetSaleResponse assetSaleResponse = portfolioService.SellAssets(data[0].PortfolioId, saleDetail1, "eythdjf.skjfdaskjgadfjbka.jsdlkfalksdjas");

            Assert.AreEqual(assetSaleResponse.NetWorth, 0);
            Assert.AreEqual(assetSaleResponse.SaleStatus, false);

        }


        [Test]
        public void AddStock_ValidData_ReturnsString()
        {
            mock.Setup(r => r.GetPortfolioDetails(1)).Returns(data[0]);
            mock.Setup(r => r.AddStock(data[0], data[0].StockList[0])).Returns("Stock was added successfully");

            message = portfolioService.AddStock(data[0].PortfolioId, data[0].StockList[0]);
            Assert.AreEqual(message, "Stock was added successfully");
        }

        [Test]
        public void AddStock_InvalidData_ReturnsString()
        {
            PortfolioDetails portfolio = null;
            mock.Setup(r => r.GetPortfolioDetails(6)).Returns(portfolio);

            message = portfolioService.AddStock(6, data[0].StockList[0]);
            Assert.AreEqual(message, "Details couldn't be updated.Please try later.");
        }

        [Test]
        public void AddStock_AnyData_Exception_ReturnsString()
        {
            mock.Setup(r => r.GetPortfolioDetails(1)).Throws<Exception>();
            message = portfolioService.AddStock(data[0].PortfolioId, data[0].StockList[0]);
            Assert.AreEqual(message, "Sorry,an error occurred.Please try later");
        }

        [Test]
        public void AddMutualFund_ValidData_ReturnsString()
        {
            mock.Setup(r => r.GetPortfolioDetails(1)).Returns(data[0]);
            mock.Setup(r => r.AddMutualFund(data[0], data[0].MutualFundList[0])).Returns("Mutual Fund was added successfully");

            message = portfolioService.AddMutualFund(data[0].PortfolioId, data[0].MutualFundList[0]);
            Assert.AreEqual(message, "Mutual Fund was added successfully");
        }

        [Test]
        public void AddMutualFund_InvalidData_ReturnsString()
        {
            PortfolioDetails portfolio = null;
            mock.Setup(r => r.GetPortfolioDetails(6)).Returns(portfolio);

            message = portfolioService.AddMutualFund(6, data[0].MutualFundList[0]);
            Assert.AreEqual(message, "Details couldn't be updated.Please try later.");
        }

        [Test]
        public void AddMutualFund_AnyData_Exception_ReturnsString()
        {
            mock.Setup(r => r.GetPortfolioDetails(1)).Throws<Exception>();
            message = portfolioService.AddMutualFund(data[0].PortfolioId, data[0].MutualFundList[0]);
            Assert.AreEqual(message, "Sorry,an error occurred.Please try later");
        }





    }
}
