using CalculateNetworthMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateNetworthMicroservice.Data
{
    public class PortfolioData
    {
        //List to store the protfolio details of all users.
        public static List<PortfolioDetails> portfolioDetails = new List<PortfolioDetails>()
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
}
