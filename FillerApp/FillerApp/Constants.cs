using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FillerApp
{
    public static class Constants
    {
        public static string RootPath { get; set; } = "E:\\dev\\boring_online_cource_helper";

        public static string HistoryBaseFilename { get; set; } = "historyBase.txt";

        public static string HistoryWeightsPath => $"{RootPath}\\historyWeights.txt";

        public static string HistoryBasePath => $"{RootPath}\\{HistoryBaseFilename}";
    }
}
