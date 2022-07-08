namespace DNCef
{
    public partial struct CefViewPoint
    {
        public int X;
        public int Y;
    }

    public partial struct CefViewSize
    {
        public int Width;
        public int Height;
    }

    public partial struct CefViewRect
    {
        public CefViewRect(int x, int y, int w, int h)
        {
            X = x;
            Y = y;
            Width = w;
            Height = h;
        }

        public int X;
        public int Y;
        public int Width;
        public int Height;
    }

    public partial struct CefViewScreenInfo
    {
        public float DeviceScaleFactor;
        public int Depth;
        public int DepthPerComponent;
        public int IsMonochrome;
        public CefViewRect Rect;
        public CefViewRect AvailableRect;
    }

    public partial struct CefViewRange
    {
        public int From;
        public int To;

        public CefViewRange(int from, int to)
        {
            From = from;
            To = to;
        }
    }

    public partial struct CefViewDraggableRegion
    {
        public CefViewRect Bounds;
        public int Draggable;
    }

    public partial struct CefViewCursorInfo
    {
        public CefViewPoint Hotspot;
        public float ImageScaleFactor;
        public byte[] Buffer;
        public CefViewSize Size;
    }

    public partial struct CefViewCompositionUnderline
    {
        public CefViewRange Range;
        public uint Color;
        public uint BackgroundColor;
        public int Thick;
        public CefViewCompositionUnderlineStyle Style;

        public CefViewCompositionUnderline(CefViewRange range, uint color, uint backGroundColor, int thick)
            : this()
        {
            Range = range;
            Color = color;
            BackgroundColor = backGroundColor;
            Thick = thick;
        }

        public CefViewCompositionUnderline(CefViewRange range, uint color, uint backGroundColor, int thick, CefViewCompositionUnderlineStyle style)
            : this()
        {
            Range = range;
            Color = color;
            BackgroundColor = backGroundColor;
            Thick = thick;
            Style = style;
        }
    }
}
