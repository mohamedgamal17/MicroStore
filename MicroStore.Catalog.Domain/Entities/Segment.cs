namespace MicroStore.Catalog.Domain.Entities
{
    public struct Segment 
    {
        public Segment(int startX, int endX, int startY, int endY)
        {
            StartX = startX;
            EndX = endX;
            StartY = startY;
            EndY = endY;
        }

        public int StartX { get;  }
        public int EndX { get;  }
        public int StartY { get; }
        public int EndY { get; }
    }
}
