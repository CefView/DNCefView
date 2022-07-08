#ifndef CefQuery_H_
#define CefQuery_H_
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

  typedef struct CCefQuery ccefquery_class;
  CCEFVIEW_EXPORT void CCefQuery_Delete(ccefquery_class * thiz);
  CCEFVIEW_EXPORT ccefquery_class * CCefQuery_new0();
  CCEFVIEW_EXPORT ccefquery_class * CCefQuery_new1(const char * req, const int64_t query);
  CCEFVIEW_EXPORT const char * CCefQuery_getRequest(ccefquery_class * thiz);
  CCEFVIEW_EXPORT const int64_t CCefQuery_getId(ccefquery_class * thiz);
  CCEFVIEW_EXPORT const char * CCefQuery_getResponse(ccefquery_class * thiz);
  CCEFVIEW_EXPORT const bool CCefQuery_getResult(ccefquery_class * thiz);
  CCEFVIEW_EXPORT const int CCefQuery_getError(ccefquery_class * thiz);
  CCEFVIEW_EXPORT void CCefQuery_setResponseResult(ccefquery_class * thiz, bool success, const char * response, int error);

#if defined(__cplusplus)
}
#endif


#endif // CefQuery_H_