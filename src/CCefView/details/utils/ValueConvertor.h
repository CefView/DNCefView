#pragma once

#include <string>

#pragma region cef_headers
#include <include/cef_app.h>
#include <include/cef_values.h>
#pragma endregion cef_headers

class ValueConvertor
{
public:
  /// <summary>
  ///
  /// </summary>
  /// <param name="jsonString"></param>
  /// <param name="cValue"></param>
  /// <returns></returns>
  static bool CefValueToJsonString(std::string& jsonString, CefValue* cValue);

  /// <summary>
  ///
  /// </summary>
  /// <param name="cValue"></param>
  /// <param name="qVariant"></param>
  /// <returns></returns>
  static bool JsonStringToCefValue(CefValue* cValue, const std::string& qVariant);
};
