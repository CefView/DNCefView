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
        CEF_WOD_MAX_VALUE = 11,
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
        CEF_TEXT_INPUT_MODE_MAX = 8,
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
    };

    public enum CefViewCompositionUnderlineStyle
    {
        CEF_CUS_SOLID = 0,
        CEF_CUS_DOT = 1,
        CEF_CUS_DASH = 2,
        CEF_CUS_NONE = 3,
    };

    [StructLayout(LayoutKind.Sequential)]
    public partial struct CefViewPoint
    {
        // int x;
        public int X;

        // int y;
        public int Y;

    }

    [StructLayout(LayoutKind.Sequential)]
    public partial struct CefViewSize
    {
        // int width;
        public int Width;

        // int height;
        public int Height;

    }

    [StructLayout(LayoutKind.Sequential)]
    public partial struct CefViewRect
    {
        // int x;
        public int X;

        // int y;
        public int Y;

        // int width;
        public int Width;

        // int height;
        public int Height;

    }

    [StructLayout(LayoutKind.Sequential)]
    public partial struct CefViewScreenInfo
    {
        // float device_scale_factor;
        public float DeviceScaleFactor;

        // int depth;
        public int Depth;

        // int depth_per_component;
        public int DepthPerComponent;

        // int is_monochrome;
        public int IsMonochrome;

        // _cef_rect_t rect;
        public CefViewRect Rect;

        // _cef_rect_t available_rect;
        public CefViewRect AvailableRect;

    }

    [StructLayout(LayoutKind.Sequential)]
    public partial struct CefViewRange
    {
        // unsigned int from;
        public uint From;

        // unsigned int to;
        public uint To;

    }

    [StructLayout(LayoutKind.Sequential)]
    public partial struct CefViewDraggableRegion
    {
        // _cef_rect_t bounds;
        public CefViewRect Bounds;

        // int draggable;
        public int Draggable;

    }

    [StructLayout(LayoutKind.Sequential)]
    public partial struct CefViewCursorInfo
    {
        // _cef_point_t hotspot;
        public CefViewPoint Hotspot;

        // float image_scale_factor;
        public float ImageScaleFactor;

        // void * buffer;
        public IntPtr Buffer;

        // _cef_size_t size;
        public CefViewSize Size;

    }

    [StructLayout(LayoutKind.Sequential)]
    public partial struct CefViewCompositionUnderline
    {
        // _cef_range_t range;
        public CefViewRange Range;

        // unsigned int color;
        public uint Color;

        // unsigned int background_color;
        public uint BackgroundColor;

        // int thick;
        public int Thick;

        // cef_composition_underline_style_t style;
        public CefViewCompositionUnderlineStyle Style;

    }

}