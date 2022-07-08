#include "CefQuery_c.h"
#include "CefQuery.h"

void CCefQuery_Delete(ccefquery_class * thiz) {
  return delete thiz;
}

ccefquery_class * CCefQuery_new0() {
  return new CCefQuery();
}

ccefquery_class * CCefQuery_new1(const char * req, const int64_t query) {
  return new CCefQuery(req, query);
}

const char * CCefQuery_getRequest(ccefquery_class * thiz) {
  return thiz->getRequest().c_str();
}

const int64_t CCefQuery_getId(ccefquery_class * thiz) {
  return thiz->getId();
}

const char * CCefQuery_getResponse(ccefquery_class * thiz) {
  return thiz->getResponse().c_str();
}

const bool CCefQuery_getResult(ccefquery_class * thiz) {
  return thiz->getResult();
}

const int CCefQuery_getError(ccefquery_class * thiz) {
  return thiz->getError();
}

void CCefQuery_setResponseResult(ccefquery_class * thiz, bool success, const char * response, int error) {
  thiz->setResponseResult(success, response, error);
}

