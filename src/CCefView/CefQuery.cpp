#include "CefQuery.h"

class CCefQuery::Implementaion
{
public:
  int64_t id_ = -1;
  int error_ = 0;
  bool restult_ = false;

  std::string request_;
  std::string response_;
};

CCefQuery::CCefQuery()
  : pImpl_(std::make_unique<Implementaion>())
{}

CCefQuery::CCefQuery(const std::string& req, const int64_t query)
  : CCefQuery()
{
  pImpl_->id_ = query;
  pImpl_->request_ = req;
}

CCefQuery::~CCefQuery()
{
  pImpl_.reset();
}

const std::string&
CCefQuery::getRequest() const
{
  return pImpl_->request_;
}

const int64_t
CCefQuery::getId() const
{
  return pImpl_->id_;
}

const std::string&
CCefQuery::getResponse() const
{
  return pImpl_->response_;
}

const bool
CCefQuery::getResult() const
{
  return pImpl_->restult_;
}

const int
CCefQuery::getError() const
{
  return pImpl_->error_;
}

void
CCefQuery::setResponseResult(bool success, const std::string& response, int error /*= 0*/)
{
  pImpl_->restult_ = success;
  pImpl_->response_ = response;
  pImpl_->error_ = error;
}
