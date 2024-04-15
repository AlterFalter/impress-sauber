namespace SauberData
{
    public class CursorPositionData
    {
        public int CursorPositionX { get; set; }
        public int CursorPositionY { get; set; }
        public DateTime DateTime { get; set; }

        public CursorPositionData(int x, int y)
            : this(x, y, DateTime.Now)
        {
        }

        public CursorPositionData(int x, int y, DateTime dateTime)
        {
            this.CursorPositionX = x;
            this.CursorPositionY = y;
            this.DateTime = dateTime;
        }
    }
}
