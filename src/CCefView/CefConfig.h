#ifndef CCEFCONFIG_H
#define CCEFCONFIG_H

#pragma once
#include <memory>
#include <unordered_map>

#include <include/cef_app.h>

#include "CefTypes.h"

/// <summary>
///
/// </summary>
class CCefConfig
{
private:
  class Implementation;
  std::unique_ptr<Implementation> pImpl_;

  friend class CCefContext;

public:
  /// <summary>
  ///
  /// </summary>
  typedef std::unordered_map<std::string, std::string> ArgsMap;

public:
  /// <summary>
  /// Constructs a CEF config instance
  /// </summary>
  CCefConfig();

  /// <summary>
  /// Constructs a CEF config instance
  /// </summary>
  ~CCefConfig();

  /// <summary>
  /// Adds a switch to the command line args used to initialize the CEF
  /// </summary>
  /// <param name="smitch">The switch name</param>
  void addCommandLineSwitch(const std::string& smitch);

  /// <summary>
  /// Adds a switch with value to the command line args used to initialize the CEF
  /// </summary>
  /// <param name="smitch">The swtich name</param>
  /// <param name="v">The switch value</param>
  void addCommandLineSwitchWithValue(const std::string& smitch, const std::string& v);

  ///// <summary>
  ///// Sets the browser subprocess path
  ///// </summary>
  ///// <param name="path">The path to the sub process executable</param>
  // void setBrowserSubProcessPath(const std::string& path);

  ///// <summary>
  ///// Gets the browser subprocess path
  ///// </summary>
  // const std::string& browserSubProcessPath() const;

  ///// <summary>
  ///// Sets the resource directory path
  ///// </summary>
  ///// <param name="path">The resource directory path</param>
  // void setResourceDirectoryPath(const std::string& path);

  ///// <summary>
  ///// Gets the resource directory path
  ///// </summary>
  // const std::string& resourceDirectoryPath() const;

  ///// <summary>
  ///// Sets the locales directory path
  ///// </summary>
  ///// <param name="path">The locales directory path</param>
  // void setLocalesDirectoryPath(const std::string& path);

  ///// <summary>
  ///// Gets the locales directory path
  ///// </summary>
  // const std::string& localesDirectoryPath() const;

  /// <summary>
  /// Sets the log level
  /// </summary>
  /// <param name="lvl"></param>
  void setLogLevel(CefViewLogLevel lvl);

  /// <summary>
  /// Gets the log level
  /// </summary>
  /// <returns>The current log level</returns>
  CefViewLogLevel logLevel() const;

  /// <summary>
  /// Sets the locale
  /// </summary>
  /// <param name="locale">The locale to use. If empty the default locale of "en-US" will be used. This value is ignored
  /// on Linux where locale is determined using environment variable parsing with the precedence order: LANGUAGE,
  /// LC_ALL, LC_MESSAGES and LANG. Also configurable using the "lang" command-line switch.</param>
  void setLocale(const std::string& locale);

  /// <summary>
  /// Gets the locale
  /// </summary>
  const std::string& locale() const;

  /// <summary>
  /// Sets the user agent
  /// </summary>
  /// <param name="agent">The user agent</param>
  void setUserAgent(const std::string& agent);

  /// <summary>
  /// Gets the user agent
  /// </summary>
  const std::string& userAgent() const;

  /// <summary>
  /// Sets the cache directory path
  /// </summary>
  /// <param name="path">The cache path</param>
  void setCachePath(const std::string& path);

  /// <summary>
  /// Gets the cache directory path
  /// </summary>
  const std::string& cachePath() const;

  /// <summary>
  /// Sets the user data directory path
  /// </summary>
  /// <param name="path">The user data directory path</param>
  void setUserDataPath(const std::string& path);

  /// <summary>
  /// Gets the user data directory path
  /// </summary>
  const std::string& userDataPath() const;

  /// <summary>
  /// Sets the bridge object name
  /// </summary>
  /// <param name="name">The bridge object name</param>
  /// <remarks>
  /// The bridge object represents a Javascript object which will be inserted
  /// into all browser and frames. This object is designated for communicating
  /// between Javascript in web content and native context(C/C++) code.
  /// This object is set as an property of window object. That means it can be
  /// obtained by calling window.bridgeObject in the Javascript code
  /// </remarks>
  void setBridgeObjectName(const std::string& name);

  /// <summary>
  /// Gets the bridge object name
  /// </summary>
  const std::string& bridgeObjectName() const;

  /// <summary>
  /// Sets the background color of the web page
  /// </summary>
  /// <param name="color">The color to be set</param>
  /// <remarks>
  /// This only works if the web page has no background color set. The alpha component value
  /// will be adjusted to 0 or 255, it means if you pass a value with alpha value
  /// in the range of [1, 255], it will be accepted as 255. The default value is qRgba(255, 255,. 255, 255)
  /// </remarks>
  void setBackgroundColor(uint32_t color);

  /// <summary>
  /// Gets the background color
  /// </summary>
  uint32_t backgroundColor() const;

  /// <summary>
  /// Sets the acceptable language list
  /// </summary>
  /// <param name="languages"></param>
  void setAcceptLanguageList(const std::string& languages);

  /// <summary>
  /// Get the acceptable language list
  /// </summary>
  const std::string& acceptLanguageList() const;

  /// <summary>
  /// Sets whether to persist session cookie
  /// </summary>
  /// <param name="enabled">True if to persist session cookie</param>
  void setPersistSessionCookies(bool enabled);

  /// <summary>
  /// Gets whether to persist session cookie
  /// </summary>
  bool persistSessionCookies() const;

  /// <summary>
  /// Sets whether to persist user preferences
  /// </summary>
  /// <param name="enabled">True if to persist user preferences</param>
  void setPersistUserPreferences(bool enabled);

  /// <summary>
  /// Gets whether to persist user preferences
  /// </summary>
  bool persistUserPreferences() const;

  /// <summary>
  ///
  /// </summary>
  /// <param name="enable"></param>
  void setMultiThreadedMessageLoop(bool enable);

  /// <summary>
  ///
  /// </summary>
  bool multiThreadedMessageLoop() const;

  /// <summary>
  /// Sets the remote debugging port
  /// </summary>
  /// <param name="port">The port to use</param>
  /// <remarks>
  /// CEF supports the remote debugging with Dev Tools in Chrome/Edge.
  /// if this value is set then you can debug the web application by
  /// accessing http://127.0.0.1:port from Chrome/Edge
  /// </remarks>
  void setRemoteDebuggingPort(short port);

  /// <summary>
  /// Gets the remote debugging port
  /// </summary>
  short remoteDebuggingPort() const;

protected:
  /// <summary>
  ///
  /// </summary>
  /// <returns></returns>
  const CCefConfig::ArgsMap& getCommandLineArgs() const;

  /// <summary>
  ///
  /// </summary>
  /// <param name="settings"></param>
  /// <returns></returns>
  static void copyToCefSettings(const CCefConfig* config, CefSettings& settings);
};

#endif
