#ifndef CefTypes_H_
#define CefTypes_H_
#pragma once

#if defined _WIN32 || defined __CYGWIN__
#ifdef __GNUC__
#define CCEFVIEW_EXPORT __attribute__((dllexport))
#else
#define CCEFVIEW_EXPORT __declspec(dllexport)
#endif
#else
#if __GNUC__ >= 4
#define CCEFVIEW_EXPORT __attribute__((visibility("default")))
#else
#define CCEFVIEW_EXPORT
#endif
#endif

#include <stdint.h>


#if defined(__cplusplus)
extern "C"
{
#endif

  typedef int cefviewwindowopendisposition_enum;
  typedef int cefviewmousebuttontype_enum;
  typedef int cefviewkeyeventtype_enum;
  typedef int cefviewpaintelementtype_enum;
  typedef int cefviewdragoperation_enum;
  typedef int cefviewtextinputmode_enum;
  typedef int cefviewcursortype_enum;
  typedef int cefviewloglevel_enum;
  typedef int cefviewpluingstate_enum;
  typedef int cefvieweventflag_enum;
  typedef int cefviewcompositionunderlinestyle_enum;
  typedef struct _cef_point_t cefviewpoint_struct;
  typedef struct _cef_size_t cefviewsize_struct;
  typedef struct _cef_rect_t cefviewrect_struct;
  typedef struct _cef_screen_info_t cefviewscreeninfo_struct;
  typedef struct _cef_range_t cefviewrange_struct;
  typedef struct _cef_draggable_region_t cefviewdraggableregion_struct;
  typedef struct _cef_cursor_info_t cefviewcursorinfo_struct;
  typedef struct _cef_composition_underline_t cefviewcompositionunderline_struct;

#if defined(__cplusplus)
}
#endif


#endif // CefTypes_H_