using System.Collections.Generic;

namespace Trading.Helpers
{
    using System.IO;
    using TradingTools;
    using System;

    public class Metastock
    {
        public static bool UpdateSymbol(string metastockFolder, string symbol, List<PriceRecord> priceRecords)
        {
            if (priceRecords == null) return false;

            try
            {
                var meta = new MetaLib();
                meta.OpenDirectory(metastockFolder, FileAccess.ReadWrite);
                var found = meta.OpenSecuritySymbol(symbol);
                if (!found) return false;
                meta.SeekForLastRecord();

                foreach (var priceRecord in priceRecords)
                {
                    meta.AppendPriceRecord(priceRecord);
                }

                meta.CloseDirectory();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public static string GetLastDateStatus(string metastockFolder, string symbol)
        {
            var meta = new MetaLib();
            meta.OpenDirectory(metastockFolder, FileAccess.ReadWrite);
            var found = meta.OpenSecuritySymbol(symbol);
            if (!found) return "Symbol not found";
            meta.SeekForLastRecord();
            var date = meta.ReadPriceRecord().Date.Date.ToShortDateString();
            meta.CloseDirectory();
            return date;
        }

        public static bool IsValidMetastockFolder(string path)
        {
            var meta = new MetaLib();

            return !String.IsNullOrEmpty(path) && Directory.Exists(path) &&
                meta.IsMetaStockDirectory(path);
        }
    }
}
