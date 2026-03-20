using Avalonia;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;

namespace DNCefView.Avalonia
{
    public partial class CefView
    {
        static void ClassInitializeCursor()
        {
        }

        private Cursor? _currentCursor;

        void InitializeCursor()
        {
        }

        void UI_OnCefCursorChanged(int browserId, CefViewCursorType type, CefViewCursorInfo customCursorInfo)
        {
            Cursor? cursor = CreateAvaloniaCursor(type, customCursorInfo);

            RunInUIThread(() =>
            {
                Cursor = cursor;

                _currentCursor?.Dispose();
                _currentCursor = cursor;
            },
            block: false);
        }

        private Cursor CreateAvaloniaCursor(CefViewCursorType type, CefViewCursorInfo customCursorInfo)
        {
            if (type == CefViewCursorType.CT_CUSTOM)
            {
                var customCursor = CreateCustomCursor(customCursorInfo);
                if (customCursor != null)
                {
                    return customCursor;
                }
            }

            var standardType = type switch
            {
                CefViewCursorType.CT_POINTER => StandardCursorType.Arrow,
                CefViewCursorType.CT_CROSS => StandardCursorType.Cross,
                CefViewCursorType.CT_HAND => StandardCursorType.Hand,
                CefViewCursorType.CT_IBEAM => StandardCursorType.Ibeam,
                CefViewCursorType.CT_WAIT => StandardCursorType.Wait,
                CefViewCursorType.CT_HELP => StandardCursorType.Help,

                CefViewCursorType.CT_EASTRESIZE => StandardCursorType.RightSide,
                CefViewCursorType.CT_NORTHRESIZE => StandardCursorType.TopSide,
                CefViewCursorType.CT_NORTHEASTRESIZE => StandardCursorType.TopRightCorner,
                CefViewCursorType.CT_NORTHWESTRESIZE => StandardCursorType.TopLeftCorner,
                CefViewCursorType.CT_SOUTHRESIZE => StandardCursorType.BottomSide,
                CefViewCursorType.CT_SOUTHEASTRESIZE => StandardCursorType.BottomRightCorner,
                CefViewCursorType.CT_SOUTHWESTRESIZE => StandardCursorType.BottomLeftCorner,
                CefViewCursorType.CT_WESTRESIZE => StandardCursorType.LeftSide,
                CefViewCursorType.CT_NORTHSOUTHRESIZE => StandardCursorType.SizeNorthSouth,
                CefViewCursorType.CT_EASTWESTRESIZE => StandardCursorType.SizeWestEast,
                CefViewCursorType.CT_NORTHEASTSOUTHWESTRESIZE => StandardCursorType.TopRightCorner,
                CefViewCursorType.CT_NORTHWESTSOUTHEASTRESIZE => StandardCursorType.TopLeftCorner,
                CefViewCursorType.CT_COLUMNRESIZE => StandardCursorType.SizeWestEast,
                CefViewCursorType.CT_ROWRESIZE => StandardCursorType.SizeNorthSouth,

                CefViewCursorType.CT_MIDDLEPANNING => StandardCursorType.SizeAll,
                CefViewCursorType.CT_EASTPANNING => StandardCursorType.RightSide,
                CefViewCursorType.CT_NORTHPANNING => StandardCursorType.TopSide,
                CefViewCursorType.CT_NORTHEASTPANNING => StandardCursorType.TopRightCorner,
                CefViewCursorType.CT_NORTHWESTPANNING => StandardCursorType.TopLeftCorner,
                CefViewCursorType.CT_SOUTHPANNING => StandardCursorType.BottomSide,
                CefViewCursorType.CT_SOUTHEASTPANNING => StandardCursorType.BottomRightCorner,
                CefViewCursorType.CT_SOUTHWESTPANNING => StandardCursorType.BottomLeftCorner,
                CefViewCursorType.CT_WESTPANNING => StandardCursorType.LeftSide,
                CefViewCursorType.CT_MOVE => StandardCursorType.SizeAll,
                CefViewCursorType.CT_VERTICALTEXT => StandardCursorType.Ibeam,
                CefViewCursorType.CT_CELL => StandardCursorType.Cross,
                CefViewCursorType.CT_CONTEXTMENU => StandardCursorType.Arrow,
                CefViewCursorType.CT_ALIAS => StandardCursorType.DragLink,
                CefViewCursorType.CT_PROGRESS => StandardCursorType.AppStarting,
                CefViewCursorType.CT_NODROP => StandardCursorType.No,
                CefViewCursorType.CT_COPY => StandardCursorType.DragCopy,
                CefViewCursorType.CT_NONE => StandardCursorType.None,
                CefViewCursorType.CT_NOTALLOWED => StandardCursorType.No,
                CefViewCursorType.CT_ZOOMIN => StandardCursorType.Arrow,
                CefViewCursorType.CT_ZOOMOUT => StandardCursorType.Arrow,
                CefViewCursorType.CT_GRAB => StandardCursorType.Hand,
                CefViewCursorType.CT_GRABBING => StandardCursorType.SizeAll,
                CefViewCursorType.CT_MIDDLE_PANNING_VERTICAL => StandardCursorType.SizeNorthSouth,
                CefViewCursorType.CT_MIDDLE_PANNING_HORIZONTAL => StandardCursorType.SizeWestEast,
                CefViewCursorType.CT_DND_NONE => StandardCursorType.No,
                CefViewCursorType.CT_DND_MOVE => StandardCursorType.DragMove,
                CefViewCursorType.CT_DND_COPY => StandardCursorType.DragCopy,
                CefViewCursorType.CT_DND_LINK => StandardCursorType.DragLink,
                _ => StandardCursorType.Arrow
            };

            return new Cursor(standardType);
        }

        private Cursor? CreateCustomCursor(CefViewCursorInfo customCursorInfo)
        {
            int width = customCursorInfo.Size.Width;
            int height = customCursorInfo.Size.Height;

            if (customCursorInfo.Buffer == IntPtr.Zero || width <= 0 || height <= 0)
            {
                return null;
            }

            int hotspotX = Math.Clamp(customCursorInfo.Hotspot.X, 0, width - 1);
            int hotspotY = Math.Clamp(customCursorInfo.Hotspot.Y, 0, height - 1);
            var scale = customCursorInfo.ImageScaleFactor > 0 ? customCursorInfo.ImageScaleFactor : 1.0f;
            try
            {
                var bitmap = new Bitmap(
                    PixelFormat.Bgra8888,
                    AlphaFormat.Premul,
                    customCursorInfo.Buffer,
                    new PixelSize(width, height),
                    new Vector(96.0 * scale, 96.0 * scale),
                    width * 4);

                try
                {
                    return new Cursor(bitmap, new PixelPoint(hotspotX, hotspotY));
                }
                finally
                {
                    bitmap.Dispose();
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
