using System.Collections.Generic;

namespace NVHplatform.Interfaces
{
    public interface IChartExportable
    {
        List<double> GetXData();
        List<double> GetYData();
    }
}
