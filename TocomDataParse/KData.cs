using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TocomDataParse
{
    public class KData
    {
        /// <summary>
        /// 日期 YYYYMMDD
        /// </summary>
        public virtual string Date { get; set; }

        /// <summary>
        /// Trading method
        /// </summary>
        public virtual string TradingMethod { get; set; }

        /// <summary>
        /// Commodity
        /// </summary>
        public virtual string Commodity { get; set; }

        /// <summary>
        /// ContractMonth YYYYMM
        /// </summary>
        public virtual string ContractMonth { get; set; }

        /// <summary>
        /// StrikePrice
        /// </summary>
        public virtual string StrikePrice { get; set; }

        /// <summary>
        /// OpeningPrice
        /// </summary>
        public virtual string OpeningPrice { get; set; }

        /// <summary>
        /// HighestPrice
        /// </summary>
        public virtual string HighestPrice { get; set; }

        /// <summary>
        /// LowestPrice
        /// </summary>
        public virtual string LowestPrice { get; set; }

        /// <summary>
        /// ClosingPrice
        /// </summary>
        public virtual string ClosingPrice { get; set; }

        /// <summary>
        /// SettlementPrice
        /// </summary>
        public virtual string SettlementPrice { get; set; }

        /// <summary>
        /// TradingVolume
        /// </summary>
        public virtual string TradingVolume { get; set; }

        /// <summary>
        /// OpenInterest
        /// </summary>
        public virtual string OpenInterest { get; set; }
    }
}
