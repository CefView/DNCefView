#pragma once

#include <iostream>
#include <string>

class FunctionLogger
{
public:
  FunctionLogger(const std::string& fn)
    : functionName_(fn)
  {
    std::cout << "+++" << functionName_;
  }

  ~FunctionLogger() { std::cout << "---" << functionName_; }

  std::string functionName_;
};

#if defined(QT_DEBUG)
#define FLog() FunctionLogger __fl__(__FUNCTION__);
#else
#define FLog()
#endif

template<typename T>
inline std::string
FrameIdC2X(const T& id)
{
#if CEF_VERSION_MAJOR < 122
  return std::to_string(id);
#else
  return id.ToString();
#endif
}

template<typename T>
inline T
FrameIdX2C(const std::string& id)
{
#if CEF_VERSION_MAJOR < 122
  return std::atoll(id);
#else
  return CefString(id);
#endif
}
