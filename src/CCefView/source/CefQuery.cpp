#include "CefQuery.h"

// details
#include "details/handlers/CCefAppDelegate.h"

CCefQuery::CCefQuery() {}

CCefQuery::CCefQuery(const std::string& req, const int64_t query)
  : CCefQuery()
{
  id_ = query;
  request_ = req;
}

CCefQuery::~CCefQuery() {}

const std::string&
CCefQuery::getRequest() const
{
  return request_;
}

const int64_t
CCefQuery::getId() const
{
  return id_;
}

const std::string&
CCefQuery::getResponse() const
{
  return response_;
}

const bool
CCefQuery::getResult() const
{
  return restult_;
}

const int
CCefQuery::getError() const
{
  return error_;
}

void
CCefQuery::setResponseResult(bool success, const std::string& response, int error /*= 0*/)
{
  restult_ = success;
  response_ = response;
  error_ = error;
}
