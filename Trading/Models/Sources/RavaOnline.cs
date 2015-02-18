namespace Trading.Models.Sources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using TradingTools;

    public class RavaOnline : Source
    {
        public bool IsAvailable
        {
            get
            {
                var client = new WebClient();
                //MERVAL
                const string key = "e";
                const string identifier = "MERVAL";
                var result = client.DownloadString(Properties.Settings.Default.RavaUrl + "?" + key + "=" + identifier);

                var matchSymbolValues = Regex.Match(result,
                    @"<td>(\d{2})\/(\d{2})\/(\d{2})<\/td><td>(\d[\d.,]+)<\/td><td>(\d[\d.,]+)<\/td><td>(\d[\d.,]+)<\/td><td>(\d[\d.,]+)<\/td><td>(\d[\d.,]+)");

                return matchSymbolValues.Success;
            }
        }

        public override List<PriceRecord> GetPriceRecords(DateTime lastDateTime)
        {
            if (!IsAvailable) return new List<PriceRecord>();

            var priceRecords = new List<PriceRecord>();

            var tickerUrl = Properties.Settings.Default.RavaUrl + "?";

            tickerUrl = Parameters.Aggregate(tickerUrl, (current, parameter) => current + (parameter.Key + "=" + parameter.Value + "&"));

            tickerUrl = tickerUrl.TrimEnd(new []{'&'});

            var client = new WebClient();
            var input = client.DownloadString(tickerUrl);
            var matchSymbolValues = Regex.Matches(input,
                @"<td>(\d{2})\/(\d{2})\/(\d{2})<\/td><td>(\d[\d.,]+)<\/td><td>(\d[\d.,]+)<\/td><td>(\d[\d.,]+)<\/td><td>(\d[\d.,]+)<\/td><td>(\d[\d.,]+)",
                RegexOptions.Singleline);

            foreach (Match match in matchSymbolValues)
            {
                var date = new DateTime(Int32.Parse("20" + match.Groups[3].Value),
                    Int32.Parse(match.Groups[2].Value),
                    Int32.Parse(match.Groups[1].Value));
                if (date <= lastDateTime)
                    break;
                var pr = new PriceRecord(new MetaLibDate(date), float.Parse(match.Groups[4].Value),
                    float.Parse(match.Groups[5].Value),
                    float.Parse(match.Groups[6].Value), float.Parse(match.Groups[7].Value),
                    float.Parse(match.Groups[8].Value));
                priceRecords.Add(pr);
            }

            priceRecords = priceRecords.OrderBy(x => x.Date.Date).ToList();

            return priceRecords;
        }
    }
}
