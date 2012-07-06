using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TocomDataParse
{
    public class KDataConverter
    {
        public static KData ToKDataObject(string[] dataArray)
        {
            KData kData = new KData();
            kData.Date = dataArray[0];
            kData.TradingMethod = dataArray[1];
            kData.Commodity = dataArray[2];
            kData.ContractMonth = dataArray[3];
            kData.StrikePrice = dataArray[4];
            kData.OpeningPrice = dataArray[5];
            kData.HighestPrice = dataArray[6];
            kData.LowestPrice = dataArray[7];
            kData.ClosingPrice = dataArray[8];
            kData.SettlementPrice = dataArray[9];
            kData.TradingVolume = dataArray[10];
            kData.OpenInterest = dataArray[11];
            return kData;
        }
        public static string ToCSVObject(KData kData)
        {
            string result = string.Format("{0},{1},{2},{3},{4},{5},{6}"
                , kData.Date, kData.OpeningPrice, kData.HighestPrice, kData.LowestPrice
                , kData.ClosingPrice, kData.TradingVolume, kData.OpenInterest);
            return result;
        }
    }
}
