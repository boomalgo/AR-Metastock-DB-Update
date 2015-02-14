namespace Trading.Helpers
{
    using System.IO;
    using TradingTools;
    using System;

    public class Metastock
    {
        public static string GetLastDateStatus(string metastockFolder, string symbol)
        {
            var meta = new MetaLib();
            meta.OpenDirectory(metastockFolder, FileAccess.ReadWrite);
            var found = meta.OpenSecuritySymbol(symbol);
            if (!found) return "Symbol not found";
            meta.SeekForLastRecord();
            return meta.ReadPriceRecord().Date.Date.ToShortDateString();
        }

        public static bool IsValidMetastockFolder(string path)
        {
            var meta = new MetaLib();

            return !String.IsNullOrEmpty(path) && Directory.Exists(path) &&
                meta.IsMetaStockDirectory(path);
        }
    }
}
