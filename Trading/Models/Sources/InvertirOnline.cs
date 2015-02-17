using System.Linq;

namespace Trading.Models.Sources
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text.RegularExpressions;
    using TradingTools; 
    #endregion

    public class InvertirOnline : Source
    {
        #region Methods
        public bool IsAvailable
        {
            get
            {
                var client = new WebClient();
                //YPF Symbol ID
                const string symbolId = "2846";
                var result = client.DownloadString(Properties.Settings.Default.IOLCsvUrl + symbolId);

                var matchSymbolValues = Regex.Match(result,
                    @"(\d{4}-\d{2}-\d{2}) \d{2}:\d{2}:\d{2};([\d,]+);([\d,]+);([\d,]+);([\d,]+);([\d,.]+)",
                    RegexOptions.Singleline);

                return matchSymbolValues.Success;
            }
        }

        public override List<PriceRecord> GetPriceRecords(DateTime lastDateTime)
        {
            if (!IsAvailable) return new List<PriceRecord>();

            var symbolId = Parameters["SymbolId"];

            var priceRecords = new List<PriceRecord>();

            var tickerUrl = Properties.Settings.Default.IOLCsvUrl + symbolId;
            var client = new WebClient();
            var symbolCsv = client.DownloadString(tickerUrl);
            var matchSymbolValues = Regex.Matches(symbolCsv,
                @"(\d{4}-\d{2}-\d{2}) \d{2}:\d{2}:\d{2};([\d,]+);([\d,]+);([\d,]+);([\d,]+);([\d,.]+)",
                RegexOptions.Singleline);

            foreach (Match match in matchSymbolValues)
            {
                var date = DateTime.Parse(match.Groups[1].Value);
                if (date <= lastDateTime)
                    break;
                var pr = new PriceRecord(new MetaLibDate(date), float.Parse(match.Groups[2].Value),
                    float.Parse(match.Groups[3].Value),
                    float.Parse(match.Groups[4].Value), float.Parse(match.Groups[5].Value),
                    float.Parse(match.Groups[6].Value));
                priceRecords.Add(pr);
            }

            priceRecords = priceRecords.OrderBy(x => x.Date.Date).ToList();

            return priceRecords;
        } 
        #endregion
    }
}
