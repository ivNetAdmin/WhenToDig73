
namespace Wtd.Core.Models
{
    public class RepLine
    {
        public int Row { get; internal set; }
        public string Font { get; internal set; }

        public int Col0 { get; internal set; }
        public int Col1 { get; internal set; }
        public int Col2 { get; internal set; }
        public int Col3 { get; internal set; }

        public string Cell0 { get; internal set; }
        public string Cell1 { get; internal set; }
        public string Cell2 { get; internal set; }
        public string Cell3 { get; internal set; }

        public int CellColspan0 { get; internal set; }
        public int CellColspan1 { get; internal set; }
        public int CellColspan2 { get; internal set; }
        public int CellColspan3 { get; internal set; }
    }
}
