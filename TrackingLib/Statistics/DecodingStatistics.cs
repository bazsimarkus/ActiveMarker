using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    public class DecodingStatistics //Statisztika osztály
    {
        public int Attempts = 0;
        public int Success = 0;

        public double GetRatio() { return 100 * (double)Success / (double)Attempts; }
        public void AddAttempt() { Attempts++; }
        public void AddSuccess() { Success++; }

        public List<long> DetectTimes = new List<long>();
        public List<long> DecodeTimes = new List<long>();
    }
}
