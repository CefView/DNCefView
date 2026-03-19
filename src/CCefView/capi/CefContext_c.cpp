// Auto-generated file. Do not modify.
// clang-format off
#include "CefContext_c.h"
#include "CefContext.h"
#include <string>

void CCefContext_Delete(ccefcontext_class * thiz) {
  return delete thiz;
}

ccefcontext_class * CCefContext_new0(const ccefconfig_class * config) {
  return new CCefContext(config);
}

void CCefContext_addFolderResource(ccefcontext_class * thiz, const char * path, const char * url, int priority) {
  thiz->addFolderResource(path, url, priority);
}

void CCefContext_addArchiveResource(ccefcontext_class * thiz, const char * path, const char * url, const char * password, int priority) {
  thiz->addArchiveResource(path, url, password, priority);
}

bool CCefContext_addCookie(ccefcontext_class * thiz, const char * name, const char * value, const char * domain, const char * url) {
  return thiz->addCookie(name, value, domain, url);
}

bool CCefContext_deleteCookie(ccefcontext_class * thiz, const char * url, const char * name) {
  return thiz->deleteCookie(url, name);
}

bool CCefContext_deleteAllCookies(ccefcontext_class * thiz) {
  return thiz->deleteAllCookies();
}

bool CCefContext_addCrossOriginWhitelistEntry(ccefcontext_class * thiz, const char * sourceOrigin, const char * targetProtocol, const char * targetDomain, bool allowTargetSubdomains) {
  return thiz->addCrossOriginWhitelistEntry(sourceOrigin, targetProtocol, targetDomain, allowTargetSubdomains);
}

bool CCefContext_removeCrossOriginWhitelistEntry(ccefcontext_class * thiz, const char * sourceOrigin, const char * targetProtocol, const char * targetDomain, bool allowTargetSubdomains) {
  return thiz->removeCrossOriginWhitelistEntry(sourceOrigin, targetProtocol, targetDomain, allowTargetSubdomains);
}

bool CCefContext_clearCrossOriginWhitelist(ccefcontext_class * thiz) {
  return thiz->clearCrossOriginWhitelist();
}

const char * CCefContext_visitAllCookiesJson(ccefcontext_class * thiz, int timeoutMs) {
  thread_local std::string cookiesJson;
  cookiesJson = thiz->visitAllCookiesJson(timeoutMs);
  return cookiesJson.c_str();
}

const char * CCefContext_visitUrlCookiesJson(ccefcontext_class * thiz, const char * url, bool includeHttpOnly, int timeoutMs) {
  thread_local std::string cookiesJson;
  cookiesJson = thiz->visitUrlCookiesJson(url ? url : "", includeHttpOnly, timeoutMs);
  return cookiesJson.c_str();
}

void CCefContext_doCefMessageLoopWork(ccefcontext_class * thiz) {
  thiz->doCefMessageLoopWork();
}

bool CCefContext_isSafeToShutdown(ccefcontext_class * thiz) {
  return thiz->isSafeToShutdown();
}
