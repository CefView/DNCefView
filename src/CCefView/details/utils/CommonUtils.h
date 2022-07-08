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
