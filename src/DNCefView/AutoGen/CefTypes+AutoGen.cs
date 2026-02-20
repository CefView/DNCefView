#pragma warning disable CS8603
using System;
using System.Runtime.InteropServices;

namespace DNCefView
{
    public enum CefViewWindowOpenDisposition
    {
        CEF_WOD_UNKNOWN = 0,
        CEF_WOD_CURRENT_TAB = 1,
        CEF_WOD_SINGLETON_TAB = 2,
        CEF_WOD_NEW_FOREGROUND_TAB = 3,
        CEF_WOD_NEW_BACKGROUND_TAB = 4,
        CEF_WOD_NEW_POPUP = 5,
        CEF_WOD_NEW_WINDOW = 6,
        CEF_WOD_SAVE_TO_DISK = 7,
        CEF_WOD_OFF_THE_RECORD = 8,
        CEF_WOD_IGNORE_ACTION = 9,
        CEF_WOD_SWITCH_TO_TAB = 10,
        CEF_WOD_NEW_PICTURE_IN_PICTURE = 11,
        CEF_WOD_NUM_VALUES = 12,
    };

    public enum CefViewMouseButtonType
    {
        MBT_LEFT = 0,
        MBT_MIDDLE = 1,
        MBT_RIGHT = 2,
    };

    public enum CefViewKeyEventType
    {
        KEYEVENT_RAWKEYDOWN = 0,
        KEYEVENT_KEYDOWN = 1,
        KEYEVENT_KEYUP = 2,
        KEYEVENT_CHAR = 3,
    };

    public enum CefViewPaintElementType
    {
        PET_VIEW = 0,
        PET_POPUP = 1,
    };

    public enum CefViewDragOperation
    {
        DRAG_OPERATION_NONE = 0,
        DRAG_OPERATION_COPY = 1,
        DRAG_OPERATION_LINK = 2,
        DRAG_OPERATION_GENERIC = 4,
        DRAG_OPERATION_PRIVATE = 8,
        DRAG_OPERATION_MOVE = 16,
        DRAG_OPERATION_DELETE = 32,
        DRAG_OPERATION_EVERY = -1,
    };

    public enum CefViewTextInputMode
    {
        CEF_TEXT_INPUT_MODE_DEFAULT = 0,
        CEF_TEXT_INPUT_MODE_NONE = 1,
        CEF_TEXT_INPUT_MODE_TEXT = 2,
        CEF_TEXT_INPUT_MODE_TEL = 3,
        CEF_TEXT_INPUT_MODE_URL = 4,
        CEF_TEXT_INPUT_MODE_EMAIL = 5,
        CEF_TEXT_INPUT_MODE_NUMERIC = 6,
        CEF_TEXT_INPUT_MODE_DECIMAL = 7,
        CEF_TEXT_INPUT_MODE_SEARCH = 8,
        CEF_TEXT_INPUT_MODE_NUM_VALUES = 9,
    };

    public enum CefViewCursorType
    {
        CT_POINTER = 0,
        CT_CROSS = 1,
        CT_HAND = 2,
        CT_IBEAM = 3,
        CT_WAIT = 4,
        CT_HELP = 5,
        CT_EASTRESIZE = 6,
        CT_NORTHRESIZE = 7,
        CT_NORTHEASTRESIZE = 8,
        CT_NORTHWESTRESIZE = 9,
        CT_SOUTHRESIZE = 10,
        CT_SOUTHEASTRESIZE = 11,
        CT_SOUTHWESTRESIZE = 12,
        CT_WESTRESIZE = 13,
        CT_NORTHSOUTHRESIZE = 14,
        CT_EASTWESTRESIZE = 15,
        CT_NORTHEASTSOUTHWESTRESIZE = 16,
        CT_NORTHWESTSOUTHEASTRESIZE = 17,
        CT_COLUMNRESIZE = 18,
        CT_ROWRESIZE = 19,
        CT_MIDDLEPANNING = 20,
        CT_EASTPANNING = 21,
        CT_NORTHPANNING = 22,
        CT_NORTHEASTPANNING = 23,
        CT_NORTHWESTPANNING = 24,
        CT_SOUTHPANNING = 25,
        CT_SOUTHEASTPANNING = 26,
        CT_SOUTHWESTPANNING = 27,
        CT_WESTPANNING = 28,
        CT_MOVE = 29,
        CT_VERTICALTEXT = 30,
        CT_CELL = 31,
        CT_CONTEXTMENU = 32,
        CT_ALIAS = 33,
        CT_PROGRESS = 34,
        CT_NODROP = 35,
        CT_COPY = 36,
        CT_NONE = 37,
        CT_NOTALLOWED = 38,
        CT_ZOOMIN = 39,
        CT_ZOOMOUT = 40,
        CT_GRAB = 41,
        CT_GRABBING = 42,
        CT_MIDDLE_PANNING_VERTICAL = 43,
        CT_MIDDLE_PANNING_HORIZONTAL = 44,
        CT_CUSTOM = 45,
        CT_DND_NONE = 46,
        CT_DND_MOVE = 47,
        CT_DND_COPY = 48,
        CT_DND_LINK = 49,
        CT_NUM_VALUES = 50,
    };

    public enum CefViewLogLevel
    {
        LOGSEVERITY_DEFAULT = 0,
        LOGSEVERITY_VERBOSE = 1,
        LOGSEVERITY_DEBUG = 1,
        LOGSEVERITY_INFO = 2,
        LOGSEVERITY_WARNING = 3,
        LOGSEVERITY_ERROR = 4,
        LOGSEVERITY_FATAL = 5,
        LOGSEVERITY_DISABLE = 99,
    };

    public enum CefViewPluingState
    {
        STATE_DEFAULT = 0,
        STATE_ENABLED = 1,
        STATE_DISABLED = 2,
    };

    public enum CefViewEventFlag
    {
        EVENTFLAG_NONE = 0,
        EVENTFLAG_CAPS_LOCK_ON = 1,
        EVENTFLAG_SHIFT_DOWN = 2,
        EVENTFLAG_CONTROL_DOWN = 4,
        EVENTFLAG_ALT_DOWN = 8,
        EVENTFLAG_LEFT_MOUSE_BUTTON = 16,
        EVENTFLAG_MIDDLE_MOUSE_BUTTON = 32,
        EVENTFLAG_RIGHT_MOUSE_BUTTON = 64,
        EVENTFLAG_COMMAND_DOWN = 128,
        EVENTFLAG_NUM_LOCK_ON = 256,
        EVENTFLAG_IS_KEY_PAD = 512,
        EVENTFLAG_IS_LEFT = 1024,
        EVENTFLAG_IS_RIGHT = 2048,
        EVENTFLAG_ALTGR_DOWN = 4096,
        EVENTFLAG_IS_REPEAT = 8192,
        EVENTFLAG_PRECISION_SCROLLING_DELTA = 16384,
        EVENTFLAG_SCROLL_BY_PAGE = 32768,
    };

    public enum CefViewCompositionUnderlineStyle
    {
        CEF_CUS_SOLID = 0,
        CEF_CUS_DOT = 1,
        CEF_CUS_DASH = 2,
        CEF_CUS_NONE = 3,
        CEF_CUS_NUM_VALUES = 4,
    };

    // Source: CefViewPoint 
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CefViewPoint
    {
        // Source: int x
        public int X;

        // Source: int y
        public int Y;

    }

    // Source: CefViewSize 
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CefViewSize
    {
        // Source: int width
        public int Width;

        // Source: int height
        public int Height;

    }

    // Source: CefViewRect 
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CefViewRect
    {
        // Source: int x
        public int X;

        // Source: int y
        public int Y;

        // Source: int width
        public int Width;

        // Source: int height
        public int Height;

    }

    // Source: CefViewScreenInfo 
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CefViewScreenInfo
    {
        // Source: size_t size
        public ulong Size;

        // Source: float device_scale_factor
        public float DeviceScaleFactor;

        // Source: int depth
        public int Depth;

        // Source: int depth_per_component
        public int DepthPerComponent;

        // Source: int is_monochrome
        public int IsMonochrome;

        // Source: cef_rect_t rect
        public CefViewRect Rect;

        // Source: cef_rect_t available_rect
        public CefViewRect AvailableRect;

    }

    // Source: CefViewRange 
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CefViewRange
    {
        // Source: uint32_t from
        public uint From;

        // Source: uint32_t to
        public uint To;

    }

    // Source: CefViewDraggableRegion 
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CefViewDraggableRegion
    {
        // Source: cef_rect_t bounds
        public CefViewRect Bounds;

        // Source: int draggable
        public int Draggable;

    }

    // Source: CefViewCursorInfo 
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CefViewCursorInfo
    {
        // Source: cef_point_t hotspot
        public CefViewPoint Hotspot;

        // Source: float image_scale_factor
        public float ImageScaleFactor;

        // Source: void * buffer
        public IntPtr Buffer;

        // Source: cef_size_t size
        public CefViewSize Size;

    }

    // Source: CefViewCompositionUnderline 
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CefViewCompositionUnderline
    {
        // Source: size_t size
        public ulong Size;

        // Source: cef_range_t range
        public CefViewRange Range;

        // Source: cef_color_t color
        public uint Color;

        // Source: cef_color_t background_color
        public uint BackgroundColor;

        // Source: int thick
        public int Thick;

        // Source: cef_composition_underline_style_t style
        public CefViewCompositionUnderlineStyle Style;

    }

}