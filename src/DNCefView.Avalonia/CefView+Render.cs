using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;

namespace DNCefView.Avalonia
{
    public partial class CefView
    {
        static void ClassInitializeRender()
        {
        }

        private Rect _cefViewRect = new Rect(0, 0, 1, 1);
        private WriteableBitmap? _cefViewImage;

        private Rect _cefPopupRect = new Rect(0, 0, 1, 1);
        private WriteableBitmap? _cefPopupImage;

        void InitializeRender()
        {
        }

        void UI_OnCefGetRootScreenRect(int browserId, ref CefViewRect rect)
        {
            PixelRect bounds = new PixelRect(0, 0, 1, 1);

            RunInUIThread(() =>
            {
                bounds = (VisualRoot as Window)?.Screens?.ScreenFromVisual(this)?.Bounds ?? new PixelRect(0, 0, 1, 1);
            });

            rect.X = bounds.X;
            rect.Y = bounds.Y;
            rect.Width = bounds.Width;
            rect.Height = bounds.Height;
        }

        void UI_OnCefGetViewRect(int browserId, ref CefViewRect rect)
        {
            rect.X = 0;
            rect.Y = 0;
            rect.Width = (int)Bounds.Width;
            rect.Height = (int)Bounds.Height;
            if (rect.Width <= 0) rect.Width = 1;
            if (rect.Height <= 0) rect.Height = 1;
        }

        bool UI_OnCefGetScreenPoint(int browserId, int viewX, int viewY, ref int screenX, ref int screenY)
        {
            PixelPoint p = new PixelPoint(0, 0);
            bool success = false;

            RunInUIThread(() =>
            {
                p = this.PointToScreen(new Point(viewX, viewY));
                success = true;
            });

            if (success)
            {
                screenX = p.X;
                screenY = p.Y;
                return true;
            }

            return false;
        }

        bool UI_OnCefGetScreenInfo(int browserId, ref CefViewScreenInfo info)
        {
            var scale = VisualRoot?.RenderScaling ?? 1.0;
            PixelRect bounds = new PixelRect(0, 0, 1, 1);
            PixelRect workingArea = new PixelRect(0, 0, 1, 1);

            RunInUIThread(() =>
            {
                var screen = (VisualRoot as Window)?.Screens?.ScreenFromVisual(this);
                if (screen != null)
                {
                    bounds = screen.Bounds;
                    workingArea = screen.WorkingArea;
                }
            });

            info.Rect.X = bounds.X;
            info.Rect.Y = bounds.Y;
            info.Rect.Width = bounds.Width;
            info.Rect.Height = bounds.Height;
            info.AvailableRect.X = workingArea.X;
            info.AvailableRect.Y = workingArea.Y;
            info.AvailableRect.Width = workingArea.Width;
            info.AvailableRect.Height = workingArea.Height;
            info.Depth = 32;
            info.DepthPerComponent = 8;
            info.IsMonochrome = 0;
            info.DeviceScaleFactor = (float)scale;

            return true;
        }

        void UI_OnCefPopupSize(int browserId, CefViewRect rect)
        {
            _cefPopupRect = new Rect(rect.X, rect.Y, rect.Width, rect.Height);
        }

        void UI_OnCefPaint(int browserId, CefViewPaintElementType type, CefViewRect[] dirtyRects, int dirtyRectCount, IntPtr imageBytesBuffer, int imageBytesCount, int width, int height)
        {
            RunInUIThread(() =>
            {
                WriteableBitmap? targetBitmap = (type == CefViewPaintElementType.PET_VIEW) ? _cefViewImage : _cefPopupImage;

                if (targetBitmap == null || targetBitmap.PixelSize.Width != width || targetBitmap.PixelSize.Height != height)
                {
                    targetBitmap?.Dispose();
                    targetBitmap = new WriteableBitmap(new PixelSize(width, height), new Vector(96, 96), PixelFormat.Bgra8888, AlphaFormat.Premul);
                    var scale = VisualRoot?.RenderScaling ?? 1.0;

                    if (type == CefViewPaintElementType.PET_VIEW)
                    {
                        _cefViewRect = _cefViewRect.WithWidth(width / scale).WithHeight(height / scale);
                        _cefViewImage = targetBitmap;
                    }
                    else
                    {
                        _cefPopupRect = _cefPopupRect.WithWidth(width / scale).WithHeight(height / scale);
                        _cefPopupImage = targetBitmap;
                    }
                }

                using (var fb = targetBitmap.Lock())
                {
                    unsafe
                    {
                        Buffer.MemoryCopy((void*)imageBytesBuffer, (void*)fb.Address, imageBytesCount, imageBytesCount);
                    }
                }

                InvalidateVisual();
            });
        }

        void UI_OnCefAcceleratedPaint(int browserId, CefViewPaintElementType type, CefViewRect[] dirtyRects, int dirtyRectCount, IntPtr sharedHandle, int planeBytesCount)
        {
        }

        void UI_OnCefImeTextSelectionChanged(int browserId, string selectedText, CefViewRange selectedRange)
        {
        }

        public override void Render(DrawingContext context)
        {
            if (_cefViewImage != null)
            {
                var srcRect = new Rect(0, 0, _cefViewImage.PixelSize.Width, _cefViewImage.PixelSize.Height);
                context.DrawImage(_cefViewImage, srcRect, _cefViewRect);
            }

            if (_isShowPopup && _cefPopupImage != null)
            {
                var srcRect = new Rect(0, 0, _cefPopupImage.PixelSize.Width, _cefPopupImage.PixelSize.Height);
                context.DrawImage(_cefPopupImage, srcRect, _cefPopupRect);
            }

            base.Render(context);
        }
    }
}
