namespace DNCef
{
    public partial struct CefViewPoint
    {
        public CefViewPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public partial struct CefViewSize
    {
        public CefViewSize(int w, int h)
        {
            Width = w;
            Height = h;
        }
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
    }

    public partial struct CefViewScreenInfo
    {
    }

    public partial struct CefViewRange
    {
        public CefViewRange(uint from, uint to)
        {
            From = from;
            To = to;
        }
    }

    public partial struct CefViewDraggableRegion
    {
    }

    public partial struct CefViewCursorInfo
    {
    }

    public partial struct CefViewCompositionUnderline
    {
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
