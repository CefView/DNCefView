#ifndef CCEFVIEWTYPES_H
#define CCEFVIEWTYPES_H

#pragma once
#include <include/cef_app.h>

//////////////////////////////////////////////////////////////////////////
// cef enum
typedef cef_window_open_disposition_t CefViewWindowOpenDisposition;

typedef cef_mouse_button_type_t CefViewMouseButtonType;

typedef cef_key_event_type_t CefViewKeyEventType;

typedef cef_paint_element_type_t CefViewPaintElementType;

typedef cef_drag_operations_mask_t CefViewDragOperation;

typedef cef_text_input_mode_t CefViewTextInputMode;

typedef cef_cursor_type_t CefViewCursorType;

typedef cef_log_severity_t CefViewLogLevel;

typedef cef_state_t CefViewPluingState;

typedef cef_event_flags_t CefViewEventFlag;

typedef cef_composition_underline_style_t CefViewCompositionUnderlineStyle;

//////////////////////////////////////////////////////////////////////////
// cef struct
typedef _cef_point_t CefViewPoint;

typedef _cef_size_t CefViewSize;

typedef _cef_rect_t CefViewRect;

typedef _cef_screen_info_t CefViewScreenInfo;

typedef _cef_range_t CefViewRange;

typedef _cef_draggable_region_t CefViewDraggableRegion;

typedef _cef_cursor_info_t CefViewCursorInfo;

typedef _cef_composition_underline_t CefViewCompositionUnderline;

#endif
