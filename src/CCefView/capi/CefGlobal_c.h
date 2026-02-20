// Auto-generated file. Do not modify.
// clang-format off
#ifndef CefGlobal_H_
#define CefGlobal_H_
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


#if defined(__cplusplus)
}
#endif


#endif // CefGlobal_H_