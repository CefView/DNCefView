#include "ValueConvertor.h"

#include <Common/CefViewCoreLog.h>
#include <nlohmann/json.hpp>

static bool
CefValueToJson(nlohmann::json& jsonValue, CefValue* cefValue)
{
  jsonValue = nlohmann::json(1);
  auto jj = nlohmann::json(true);

  if (!cefValue) {
    logD("Invalid arguments");
    return false;
  }

  auto type = cefValue->GetType();
  switch (type) {
    case CefValueType::VTYPE_INVALID:
    case CefValueType::VTYPE_NULL: {
      jsonValue = nlohmann::json();
    } break;
    case CefValueType::VTYPE_BOOL: {
      jsonValue = nlohmann::json(cefValue->GetBool());
    } break;
    case CefValueType::VTYPE_INT: {
      jsonValue = nlohmann::json(cefValue->GetInt());
    } break;
    case CefValueType::VTYPE_DOUBLE: {
      jsonValue = nlohmann::json(cefValue->GetDouble());
    } break;
    case CefValueType::VTYPE_STRING: {
      jsonValue = nlohmann::json(cefValue->GetString().ToString());
    } break;
    case CefValueType::VTYPE_BINARY: {
      auto cData = cefValue->GetBinary();
      auto cLen = cData->GetSize();
      std::vector<uint8_t> jData(cLen, 0);
      cefValue->GetBinary()->GetData(jData.data(), jData.size(), 0);
      jsonValue = nlohmann::json::binary(jData);
    } break;
    case CefValueType::VTYPE_DICTIONARY: {
      auto cDict = cefValue->GetDictionary();
      CefDictionaryValue::KeyList cKeys;
      if (!cDict->GetKeys(cKeys)) {
        logD("Failed to get cef dictionary keys");
      }
      jsonValue = nlohmann::json::object();
      for (auto& key : cKeys) {
        auto cVal = cDict->GetValue(key);
        nlohmann::json jVal;
        CefValueToJson(jVal, cVal.get());
        auto jKey = key.ToString();
        jsonValue[jKey] = jVal;
      }
    } break;
    case CefValueType::VTYPE_LIST: {
      auto cList = cefValue->GetList();
      auto cCount = cList->GetSize();
      jsonValue = nlohmann::json::array();
      for (int i = 0; i < cCount; i++) {
        auto cVal = cList->GetValue(i);
        nlohmann::json jVal;
        CefValueToJson(jVal, cVal.get());
        jsonValue.push_back(jVal);
      }
    } break;
    default: {
      logD("Unsupported CefValueType conversion: %d", type);
      return false;
    }
  }

  return true;
}

bool
ValueConvertor::CefValueToJsonString(std::string& jsonString, CefValue* cefValue)
{
  if (!cefValue) {
    logD("Invalid arguments");
    return false;
  }

  nlohmann::json jValue;
  if (CefValueToJson(jValue, cefValue)) {
    jsonString = nlohmann::to_string(jValue);
    return true;
  }

  return false;
}

static bool
JsonToCefValue(CefValue* cValue, const nlohmann::json& jsonValue)
{
  if (!cValue) {
    logD("Invalid arguments");
    return false;
  }

  auto type = jsonValue.type();
  switch (type) {
    case nlohmann::json::value_t::null: {
      cValue->SetNull();
    } break;
    case nlohmann::json::value_t::boolean: {
      cValue->SetBool(jsonValue.get<bool>());
    } break;
    case nlohmann::json::value_t::number_integer: {
      cValue->SetInt(jsonValue.get<int>());
    } break;
    case nlohmann::json::value_t::number_unsigned:
    case nlohmann::json::value_t::number_float: {
      cValue->SetDouble(jsonValue.get<double>());
    } break;
    case nlohmann::json::value_t::string: {
      CefString cStr = CefString(jsonValue.get<std::string>());
      cValue->SetString(cStr);
    } break;
    case nlohmann::json::value_t::binary: {
      auto jData = jsonValue.get<std::vector<uint8_t>>();
      auto cData = CefBinaryValue::Create(jData.data(), jData.size());
      cValue->SetBinary(cData);
    } break;
    case nlohmann::json::value_t::object: {
      auto cDict = CefDictionaryValue::Create();
      for (auto& it : jsonValue.items()) {
        auto jKey = it.key();
        auto cVal = CefValue::Create();
        JsonToCefValue(cVal.get(), it.value());
        cDict->SetValue(CefString(jKey), cVal);
      }
      cValue->SetDictionary(cDict);
    } break;
    case nlohmann::json::value_t::array: {
      auto jCount = jsonValue.size();
      auto cList = CefListValue::Create();
      for (int i = 0; i < jCount; i++) {
        auto jVal = jsonValue.at(i);
        auto cVal = CefValue::Create();
        JsonToCefValue(cVal.get(), jVal);
        cList->SetValue(i, cVal);
      }
      cValue->SetList(cList);
    } break;
    default: {
      logD("Unsupported Json conversion: %d", type);
      return false;
    }
  }

  return true;
}

bool
ValueConvertor::JsonStringToCefValue(CefValue* cefValue, const std::string& jsonString)
{
  if (jsonString.empty() || !cefValue) {
    logD("Invalid arguments");
    return false;
  }

  auto jsonValue = nlohmann::json::parse(jsonString);
  if (JsonToCefValue(cefValue, jsonValue)) {
    return true;
  }

  return false;
}
