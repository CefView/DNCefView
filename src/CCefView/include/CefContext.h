#ifndef CCEFCONTEXT_H
#define CCEFCONTEXT_H

#pragma once
// platform
#if defined(OS_WINDOWS)
#include <windows.h>
#endif

// stl
#include <memory>
#include <string>
#include <vector>

// cefviewcore
#include <CefViewBrowserApp.h>

// project
#include <CefConfig.h>

// details
#include <details/handlers/CCefAppDelegate.h>

/// <summary>
///
/// </summary>
class CCefContext
{
  friend class CCefBrowser;
  friend class CCefAppDelegate;
  friend class CCefClientDelegate;

private:
  static CCefContext* instance_;

  const CCefConfig* config_;
  CefRefPtr<CefViewBrowserApp> pApp_;
  CCefAppDelegate::RefPtr pAppDelegate_;

#if defined(OS_WINDOWS)
  std::string windowsJobName_;
  HANDLE windowsJobHandle_ = nullptr;
#endif

public:
  /// <summary>
  /// Constructs the CEF context
  /// </summary>
  /// <param name="app">The application</param>
  /// <param name="argc">The argument count</param>
  /// <param name="argv">The argument list pointer</param>
  /// <param name="config">The <see cref="QCefConfig"/> instance</param>
  CCefContext(const CCefConfig* config);

  /// <summary>
  /// Destructs the CEF context
  /// </summary>
  ~CCefContext();

  /// <summary>
  /// Adds a url mapping item with local web resource directory. This works for all <see ref="QCefView" /> instances
  /// created subsequently
  /// </summary>
  /// <param name="path">The path to the local resource directory</param>
  /// <param name="url">The url to be mapped to</param>
  /// <param name="priority">The priority</param>
  void addFolderResource(const std::string& path, const std::string& url, int priority = 0);

  /// <summary>
  /// Adds a url mapping item with local archive (.zip) file which contains the web resource. This works for all <see
  /// ref="QCefView" /> instances created subsequently
  /// </summary>
  /// <param name="path">The path to the local archive file</param>
  /// <param name="url">The url to be mapped to</param>
  /// <param name="password">The password of the archive</param>
  /// <param name="priority">The priority</param>
  void addArchiveResource(const std::string& path,
                          const std::string& url,
                          const std::string& password = "",
                          int priority = 0);

  /// <summary>
  /// Adds a cookie to the CEF context, this cookie is accessible from all browsers created with this context
  /// </summary>
  /// <param name="name">The cookie item name</param>
  /// <param name="value">The cookie item value</param>
  /// <param name="domain">The applicable domain name</param>
  /// <param name="url">The applicable url</param>
  /// <returns>True on success; otherwise false</returns>
  bool addCookie(const std::string& name, const std::string& value, const std::string& domain, const std::string& url);

  /// <summary>
  /// Deletes a specific cookie matching |url| and |name|.
  /// </summary>
  /// <returns>True on success; otherwise false</returns>
  bool deleteCookie(const std::string& url, const std::string& name);

  /// <summary>
  /// Deletes all cookies in the global cookie manager.
  /// </summary>
  /// <returns>True on success; otherwise false</returns>
  bool deleteAllCookies();

  /// <summary>
  /// Adds a cross-origin access whitelist entry.
  /// </summary>
  bool addCrossOriginWhitelistEntry(const std::string& sourceOrigin,
                                    const std::string& targetProtocol,
                                    const std::string& targetDomain,
                                    bool allowTargetSubdomains);

  /// <summary>
  /// Removes a cross-origin access whitelist entry.
  /// </summary>
  bool removeCrossOriginWhitelistEntry(const std::string& sourceOrigin,
                                       const std::string& targetProtocol,
                                       const std::string& targetDomain,
                                       bool allowTargetSubdomains);

  /// <summary>
  /// Clears all cross-origin whitelist entries.
  /// </summary>
  bool clearCrossOriginWhitelist();
  /// <summary>
  /// Visits all cookies from the global cookie manager and returns a JSON snapshot.
  /// </summary>
  /// <param name="timeoutMs">Maximum wait time in milliseconds.</param>
  /// <returns>JSON payload with "cookies", "started", and "timedOut".</returns>
  std::string visitAllCookiesJson(int timeoutMs = 3000);

  /// <summary>
  /// Visits URL-scoped cookies from the global cookie manager and returns a JSON snapshot.
  /// </summary>
  /// <param name="url">The target URL used for filtering.</param>
  /// <param name="includeHttpOnly">Whether HTTP-only cookies should be included.</param>
  /// <param name="timeoutMs">Maximum wait time in milliseconds.</param>
  /// <returns>JSON payload with "cookies", "started", and "timedOut".</returns>
  std::string visitUrlCookiesJson(const std::string& url, bool includeHttpOnly, int timeoutMs = 3000);

  /// <summary>
  ///
  /// </summary>
  void doCefMessageLoopWork();

  /// <summary>
  ///
  /// </summary>
  /// <returns></returns>
  bool isSafeToShutdown();

protected:
  /// <summary>
  ///
  /// </summary>
  /// <returns></returns>
  static CCefContext* instance();

  /// <summary>
  /// Gets the QCefConfig
  /// </summary>
  /// <returns>The QCefConfig instance</returns>
  const CCefConfig* cefConfig() const;

  /// <summary>
  /// Initialize the CEF context
  /// </summary>
  /// <param name="config">The <see cref="QCefConfig"/> instance</param>
  /// <returns>True on success; otherwise false</returns>
  bool init(const CCefConfig* config);

  /// <summary>
  /// Uninitialize the CEF context
  /// </summary>
  void uninit();

  /// <summary>
  ///
  /// </summary>
  /// <param name="delayMs"></param>
  void scheduleCefLoopWork(int64_t delayMs);
};

#endif
