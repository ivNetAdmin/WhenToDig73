
namespace Wtd.Core.Helpers
{
    public static class StringHelper
    {
        public static string FullPlantName(string name, string variety)
        {
            return string.Format("{0} {1}", name, string.IsNullOrEmpty(variety) ? string.Empty : string.Format("[{0}]", variety));
        }
    }
}
