#ifndef CCEFVIEW_H
#define CCEFVIEW_H

#pragma once
#include <cstdint>
#include <memory>
#include <string>

#include <include/cef_app.h>

#include "CefBrowserCallback.h"
#include "CefContext.h"
#include "CefQuery.h"
#include "CefSetting.h"
#include "CefTypes.h"

/// <summary>
///
/// </summary>
class CCefBrowser
{
private:
  class Implementation;
  std::unique_ptr<Implementation> pImpl_;

  CefBrowserCallback callbackTable_;

  friend class CCefClientDelegate;

public:
  /// <summary>
  /// The main frame identity
  /// </summary>
  static const int64_t MainFrameID = 0;

  /// <summary>
  ///
  /// </summary>
  static const int64_t AllFrameID = -1;

public:
  /// <summary>
  /// Constructs a CCefView instance
  /// </summary>
  /// <param name="callback">The delegate table</param>
  /// <param name="url">The target url</param>
  /// <param name="setting">The <see cref="QCefSetting"/> instance</param>
  /// <param name="parent">The parent</param>
  CCefBrowser(CefBrowserCallback callback, const std::string& url, const CCefSetting* setting);

  /// <summary>
  /// Destructs the CCefView instance
  /// </summary>
  ~CCefBrowser();

  /// <summary>
  /// Adds a url mapping item with local web resource directory
  /// </summary>
  /// <param name="path">The path to the local resource directory</param>
  /// <param name="url">The url to be mapped to</param>
  /// <param name="priority">The priority</param>
  void addLocalFolderResource(const std::string& path, const std::string& url, int priority = 0);

  /// <summary>
  /// Adds a url mapping item with local archive (.zip) file which contains the web resource
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
  /// Gets the browser id
  /// </summary>
  /// <returns>The browser id</returns>
  int browserId();

  /// <summary>
  /// Navigates to the content.
  /// </summary>
  /// <param name="content">The content</param>
  void navigateToString(const std::string& content);

  /// <summary>
  /// Navigates to the URL
  /// </summary>
  /// <param name="url">The url</param>
  void navigateToUrl(const std::string& url);

  /// <summary>
  /// Checks whether the browser can go back
  /// </summary>
  /// <returns>True if can; otherwise false</returns>
  bool canGoBack();

  /// <summary>
  /// Checks whether the browser can go forward
  /// </summary>
  /// <returns>True if can; otherwise false</returns>
  bool canGoForward();

  /// <summary>
  /// Requires the browser to go back
  /// </summary>
  void goBack();

  /// <summary>
  /// Requires the browser to go forward
  /// </summary>
  void goForward();

  /// <summary>
  /// Checks whether the browser is loading
  /// </summary>
  /// <returns>True if it is loading; otherwise false</returns>
  bool isLoading();

  /// <summary>
  /// Requires the browser to reload
  /// </summary>
  void reload();

  /// <summary>
  /// Requires the browser to stop load
  /// </summary>
  void stopLoad();

  /// <summary>
  /// Triggers the event for main frame
  /// </summary>
  /// <param name="event">The <see cref="CCefEvent"/> instance</param>
  /// <returns>True on successful; otherwise false</returns>
  bool triggerEventOnMainFrame(const std::string& evtName, const std::string& evtArgs);

  /// <summary>
  /// Triggers the event for specified frame
  /// </summary>
  /// <param name="event">The <see cref="CCefEvent"/> instance</param>
  /// <param name="frameId">The frame id</param>
  /// <returns>True on successful; otherwise false</returns>
  bool triggerEventOnFrame(const std::string& evtName, const std::string& evtArgs, int64_t frameId);

  /// <summary>
  /// Broad cast the event for all frames
  /// </summary>
  /// <param name="event">The <see cref="CCefEvent"/> instance</param>
  /// <returns>True on successful; otherwise false</returns>
  bool broadcastEvent(const std::string& evtName, const std::string& evtArgs);

  /// <summary>
  ///
  /// </summary>
  /// <param name="name"></param>
  /// <param name="args"></param>
  /// <param name="frameId"></param>
  /// <returns></returns>
  bool triggerEvent(const std::string& name, const std::string& args, int64_t frameId = CCefBrowser::MainFrameID);

  /// <summary>
  /// Response the <see cref="QCefQuery"/> request
  /// </summary>
  /// <param name="query">The query instance</param>
  /// <returns>True on successful; otherwise false</returns>
  bool responseQCefQuery(const CCefQuery* query);

  /// <summary>
  /// Executes javascript code in specified frame, this method does not report the result of the javascript.
  /// To get the result of the javascript execution use <see cref="executeJavascriptWithResult"/>
  /// </summary>
  /// <param name="frameId">The frame id</param>
  /// <param name="code">The javascript code</param>
  /// <param name="url">
  /// The URL where the script in question can be found, if any. The renderer may request this URL to show the developer
  /// the source of the error
  /// </param>
  /// <returns>True on successful; otherwise false</returns>
  bool executeJavascript(int64_t frameId, const std::string& code, const std::string& url);

  /// <summary>
  /// Executes javascript code in specified frame and the result will be reported through <see
  /// cref="reportJavascriptResult"/> signal
  /// </summary>
  /// <param name="frameId">The frame id</param>
  /// <param name="code">The javascript code</param>
  /// <param name="url">
  /// The URL where the script in question can be found, if any. The renderer may request this URL to show the developer
  /// the source of the error
  /// </param>
  /// <param name="context">The context used to identify the one execution</param>
  /// <returns>True on successful; otherwise false</returns>
  bool executeJavascriptWithResult(int64_t frameId, const std::string& code, const std::string& url, int64_t context);

  /// <summary>
  /// Sets the preference for this browser
  /// </summary>
  /// <param name="name">The preference name</param>
  /// <param name="value">
  /// The preference value, if this value is QVariant::UnknownType or QVariant::Invalid, the
  /// preference will be restored to default value
  /// </param>
  /// <param name="error">The error message populated on failure</param>
  // <returns>True on successful; otherwise false</returns>
  bool setPreference(const std::string& name, const std::string& value /*, std::string& error*/);

  /// <summary>
  /// Sets whether to disable the context menu for popup browser
  /// </summary>
  /// <param name="disable">True to disable; otherwise false</param>
  void setDisablePopupContextMenu(bool disable);

  /// <summary>
  /// Gets whether to disable the context menu for popup browser
  /// </summary>
  /// <returns>True to disable; otherwise false</returns>
  bool isPopupContextMenuDisabled();

  /// <summary>
  ///
  /// </summary>
  /// <param name="text"></param>
  /// <param name="underlines"></param>
  /// <param name="replacement_range"></param>
  /// <param name="selection_range"></param>
  virtual void ImeSetComposition(const std::string& text,
                                 CefViewCompositionUnderline* underlines,
                                 int count,
                                 CefViewRange replacement_range,
                                 CefViewRange selection_range);

  /// <summary>
  ///
  /// </summary>
  /// <param name="text"></param>
  /// <param name="replacement_range"></param>
  /// <param name="relative_cursor_pos"></param>
  virtual void ImeCommitText(const std::string& text, CefViewRange replacement_range, int relative_cursor_pos);

  /// <summary>
  ///
  /// </summary>
  /// <param name="keep_selection"></param>
  virtual void ImeFinishComposingText(bool keep_selection);

#pragma region Control CEF
  void setFocus(bool focused);
  void wasResized();
  void wasHidden(bool hidden);
  void sendMouseMoveEvent(int x, int y, uint32_t modifiers, bool leave);
  void sendMouseClickEvent(int x, int y, uint32_t modifiers, CefViewMouseButtonType type, bool mouseUp, int clickCount);
  void sendWheelEvent(int x, int y, uint32_t modifiers, int deltaX, int deltaY);
  void sendKeyEvent(CefViewKeyEventType type,
                    uint32_t modifiers,
                    int windowsKeyCode,
                    int nativeKeyCode,
                    bool isSysKey,
                    uint16_t character,
                    uint16_t umodifiedCharacter,
                    bool isFocusOnEditableField);
  void notifyMoveOrResizeStarted();
  void notifyScreenChanged();

#pragma endregion

#pragma region CEF Callbacks
protected:
  void cefQueryRequest(int browserId, int64_t frameId, const CCefQuery* query);
  void invokeMethod(int browserId, int64_t frameId, const std::string& method, const std::string& arguments);
  void reportJavascriptResult(int browserId, int64_t frameId, int64_t context, const std::string& result);
  void inputStateChanged(int browserId, int64_t frameId, bool editable);
#pragma endregion

private:
  /// <summary>
  ///
  /// </summary>
  /// <param name="frameId"></param>
  /// <param name="name"></param>
  /// <param name="args"></param>
  /// <returns></returns>
  bool sendEventNotifyMessage(int64_t frameId, const std::string& name, const std::string& args);
};

#endif
