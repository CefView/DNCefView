#ifndef CCEFQUERY_H
#define CCEFQUERY_H

#pragma once
#include <memory>
#include <string>

/// <summary>
///
/// </summary>
class CCefQuery
{
private:
  class Implementaion;
  std::unique_ptr<Implementaion> pImpl_;

public:
  /// <summary>
  /// Constructs a query instance
  /// </summary>
  CCefQuery();

  /// <summary>
  /// Constructs a query instance with request context and query id
  /// </summary>
  /// <param name="req">The request context</param>
  /// <param name="query">The query id</param>
  CCefQuery(const std::string& req, const int64_t query);

  /// <summary>
  ///
  /// </summary>
  ~CCefQuery();

  /// <summary>
  /// Gets the query content
  /// </summary>
  /// <returns>The content string</returns>
  const std::string& getRequest() const;

  /// <summary>
  /// Gets the query id
  /// </summary>
  /// <returns>The query id</returns>
  const int64_t getId() const;

  /// <summary>
  /// Gets the response content string
  /// </summary>
  /// <returns>The response content string</returns>
  const std::string& getResponse() const;

  /// <summary>
  /// Gets the response result
  /// </summary>
  /// <returns>The respone result</returns>
  const bool getResult() const;

  /// <summary>
  /// Gets the response error
  /// </summary>
  /// <returns>The response error</returns>
  const int getError() const;

  /// <summary>
  /// Sets the response
  /// </summary>
  /// <param name="success">True if the query is successful; otherwise false</param>
  /// <param name="response">The response content string</param>
  /// <param name="error">The response error</param>
  void setResponseResult(bool success, const std::string& response, int error = 0);
};

#endif
