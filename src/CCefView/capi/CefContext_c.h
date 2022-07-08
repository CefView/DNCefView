#ifndef CefContext_H_
#define CefContext_H_
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

#include "CefConfig_c.h"

#if defined(__cplusplus)
extern "C"
{
#endif

  typedef struct CCefContext ccefcontext_class;
  CCEFVIEW_EXPORT void CCefContext_Delete(ccefcontext_class * thiz);
  CCEFVIEW_EXPORT ccefcontext_class * CCefContext_new0(const ccefconfig_class * config);
  CCEFVIEW_EXPORT void CCefContext_addFolderResource(ccefcontext_class * thiz, const char * path, const char * url, int priority);
  CCEFVIEW_EXPORT void CCefContext_addArchiveResource(ccefcontext_class * thiz, const char * path, const char * url, const char * password, int priority);
  CCEFVIEW_EXPORT bool CCefContext_addCookie(ccefcontext_class * thiz, const char * name, const char * value, const char * domain, const char * url);
  CCEFVIEW_EXPORT void CCefContext_doCefMessageLoopWork(ccefcontext_class * thiz);
  CCEFVIEW_EXPORT bool CCefContext_isSafeToShutdown(ccefcontext_class * thiz);

#if defined(__cplusplus)
}
#endif


#endif // CefContext_H_