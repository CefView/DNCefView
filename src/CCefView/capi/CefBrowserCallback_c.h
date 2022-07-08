#ifndef CefBrowserCallback_H_
#define CefBrowserCallback_H_
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

#include "CefQuery_c.h"
#include "CefTypes_c.h"

#if defined(__cplusplus)
extern "C"
{
#endif

  typedef struct CefBrowserCallback cefbrowsercallback_struct;

#if defined(__cplusplus)
}
#endif


#endif // CefBrowserCallback_H_