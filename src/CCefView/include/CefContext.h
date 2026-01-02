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

// cefviewcore
#include <CefViewBrowserApp.h>

// project
#include <CefConfig.h>

// details
#include <details/handlers/CCefAppDelegate.h>

/// <summary>
///
/// </summary>
struct CCefContext
{
  friend struct CCefBrowser;
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
