using System;
using System.Collections.Generic;
using CalculateNetworthMicroservice.Models;
using CalculateNetworthMicroservice.Repository;
using NUnit.Framework;
using CalculateNetworthMicroservice.Data;

namespace CalculateNetWorth_Testing
{
    class RepositoryTests
    {
        List<PortfolioDetails> data;
        PortfolioRepository portfolioRepository;
        string message;

        StockDetails invalidStockDetails;
        MutualFundDetails invalidMutualFundDetails;

        public RepositoryTests()
        {
            portfolioRepository = new PortfolioRepository();

            invalidStockDetails = new StockDetails() { StockName = "TCS", StockCount = 3 };

            invalidMutualFundDetails = new MutualFundDetails(){MutualFundName = "Capegemini_Fund",MutualFundUnits = 2 };

        }

       [SetUp]
       public void Setup()
       {
            data=new List<PortfolioDetails>()
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
        [TestCase(4)]
        public void GetPortfolioDetails_ValidData_ReturnsPortfolioDetails(int portfolioId)
        {
            var portfolioDetails = portfolioRepository.GetPortfolioDetails(portfolioId);
            Assert.IsNotNull(portfolioDetails);
        }

        [TestCase(55)]
        [TestCase(6)]
        [TestCase(-1)]
        [TestCase(0)]
        public void GetPortfolioDetails_InvalidData_ReturnsNull(int portfolioId)
        {
            var portfolioDetails = portfolioRepository.GetPortfolioDetails(portfolioId);
            Assert.IsNull(portfolioDetails);
        }

        [Test]
        public  void AddStock_ExistingValidData_ReturnsString()
        {
            //Note- Here AddStock(Repository) function makes changes in data list.
            //Thus changes would be reflected in data list.
            //In real time it would be making changes in our original list and thus changes would be reflected there.
            message = portfolioRepository.AddStock(data[0], data[0].StockList[0]);
            Assert.AreEqual("Stock was added successfully", message);

            //Checking wheather count was correctly updated,since existing data.
            Assert.AreEqual(4, data[0].StockList[0].StockCount);
        }

        [Test]
        public void AddStock_NewValidData_ReturnsString()
        {
            //Note- Here AddStock(Repository) function makes changes in data list.
            //Thus changes would be reflected in data list.
            //In real time it would be making changes in our original list and thus changes would be reflected there.
            message = portfolioRepository.AddStock(data[0], data[1].StockList[0]);
            Assert.AreEqual("Stock was added successfully", message);

            //Checking whether new data was added.
            Assert.AreEqual(3, data[0].StockList.Count);
            Assert.AreEqual(data[1].StockList[0], data[0].StockList[2]);
        }

        [Test]
        public void AddStock_InvalidData_ReturnsString()
        {
            message = portfolioRepository.AddStock(data[0], invalidStockDetails);
            Assert.AreEqual("Error while adding the Stock.Try again!", message);
        }

        [Test]
        public void AddMutualFund_ExistingValidData_ReturnsString()
        {
            //Note- Here AddMutualFund(Repository) function makes changes in data list.
            //Thus changes would be reflected in data list.
            //In real time it would be making changes in our original list and thus changes would be reflected there.
            message = portfolioRepository.AddMutualFund(data[0], data[0].MutualFundList[0]);
            Assert.AreEqual("MutualFund was added successfully",message);

            //Checking wheather count was correctly updated,since existing data.
            Assert.AreEqual(6, data[0].MutualFundList[0].MutualFundUnits);
        }

        [Test]
        public void AddMutualFund_NewValidData_ReturnsString()
        {
            //Note- Here AddMutualFund(Repository) function makes changes in data list.
            //Thus changes would be reflected in data list.
            //In real time it would be making changes in our original list and thus changes would be reflected there.
            message = portfolioRepository.AddMutualFund(data[0], data[1].MutualFundList[0]);
            Assert.AreEqual("MutualFund was added successfully", message);

            //Checking whether new data was added.
            Assert.AreEqual(3, data[0].MutualFundList.Count);
            Assert.AreEqual(data[1].MutualFundList[0], data[0].MutualFundList[2]);
        }

        [Test]
        public void AddMutualFund_InvalidData_ReturnsString()
        {
            message = portfolioRepository.AddMutualFund(data[0], invalidMutualFundDetails);
            Assert.AreEqual("Error while adding the mutual fund.Try again!",message);
        }
    }
}
