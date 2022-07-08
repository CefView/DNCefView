#ifndef CCEFSETTING_H
#define CCEFSETTING_H

#pragma once
#include <memory>
#include <string>

#include <include/cef_app.h>

#include "CefTypes.h"

/// <summary>
///
/// </summary>
class CCefSetting
{
private:
  class Implementation;
  std::unique_ptr<Implementation> pImpl_;

  friend class CCefBrowser;

public:
  /// <summary
  /// Constructs the CCefSetting instance
  /// </summary
  CCefSetting();

  /// <summary
  ///
  /// </summary
  ~CCefSetting();

  /// <summary
  /// Sets the standard font family
  /// </summary
  /// <param name="value"The font family</param
  void setStandardFontFamily(const std::string& value);

  /// <summary
  /// Gets the standard font family
  /// </summary
  /// <returnsThe font family</returns
  const std::string& standardFontFamily() const;

  /// <summary
  /// Sets the fixed font family
  /// </summary
  /// <param name="value"The font family</param
  void setFixedFontFamily(const std::string& value);

  /// <summary
  /// Gets the fixed font family
  /// </summary
  /// <returnsThe font family</returns
  const std::string& fixedFontFamily() const;

  /// <summary
  /// Sets the serif font family
  /// </summary
  /// <param name="value"The font family</param
  void setSerifFontFamily(const std::string& value);

  /// <summary
  /// Gets the fixed font family
  /// </summary
  /// <returnsThe font family</returns
  const std::string& serifFontFamily() const;

  /// <summary
  /// Sets the sans serif font family
  /// </summary
  /// <param name="value"The font family</param
  void setSansSerifFontFamily(const std::string& value);

  /// <summary
  /// Gets the sans serif font family
  /// </summary
  /// <returnsThe font family</returns
  const std::string& sansSerifFontFamily() const;

  /// <summary
  /// Sets the cursive font family
  /// </summary
  /// <param name="value"The font family</param
  void setCursiveFontFamily(const std::string& value);

  /// <summary
  /// Gets the cursive font family
  /// </summary
  /// <returnsThe font family</returns
  const std::string& cursiveFontFamily() const;

  /// <summary
  /// Sets the fantasy font family
  /// </summary
  /// <param name="value"The font family</param
  void setFantasyFontFamily(const std::string& value);

  /// <summary
  /// Gets the fantasy font family
  /// </summary
  /// <returnsThe font family</returns
  const std::string& fantasyFontFamily() const;

  /// <summary
  /// Sets the default encoding
  /// </summary
  /// <param name="value"The encoding name</param
  void setDefaultEncoding(const std::string& value);

  /// <summary
  /// Gets the default encoding
  /// </summary
  /// <returnsThe encoding name</returns
  const std::string& defaultEncoding() const;

  /// <summary
  /// Sets the acceptable language list
  /// </summary
  /// <param name="value"The acceptable languate list</param
  void setAcceptLanguageList(const std::string& value);

  /// <summary
  /// Gets the acceptable language list
  /// </summary
  /// <returnsThe acceptable languate list</returns
  const std::string& acceptLanguageList() const;

  /// <summary
  /// Sets the frame rate in window less mode
  /// </summary
  /// <param name="value"The frame rate</param
  void setWindowlessFrameRate(const int value);

  /// <summary
  /// Gets the frame rate in window less mode
  /// </summary
  /// <returnsThe frame rate</returns
  int windowlessFrameRate() const;

  /// <summary
  /// Sets the default font size
  /// </summary
  /// <param name="value"The font size</param
  void setDefaultFontSize(const int value);

  /// <summary
  /// Gets the default font size
  /// </summary
  /// <returnsThe font size</returns
  int defaultFontSize() const;

  /// <summary
  /// Sets the default fixed font size
  /// </summary
  /// <param name="value"The font size</param
  void setDefaultFixedFontSize(const int value);

  /// <summary
  /// Gets the default fixed font size
  /// </summary
  /// <returnsThe font size</returns
  int defaultFixedFontSize() const;

  /// <summary
  /// Sets the minimum font size
  /// </summary
  /// <param name="value"The font size</param
  void setMinimumFontSize(const int value);

  /// <summary
  /// Gets the minimum font size
  /// </summary
  /// <returnsThe font size</returns
  int minimumFontSize() const;

  /// <summary
  /// Sets the minimum logical font size
  /// </summary
  /// <param name="value"The font size</param
  void setMinimumLogicalFontSize(const int value);

  /// <summary
  /// Gets the minimum logical font size
  /// </summary
  /// <returnsThe font size</returns
  int minimumLogicalFontSize() const;

  /// <summary
  /// Sets to enable or disable remote fonts
  /// </summary
  /// <param name="value"True to enalbe; false to disable</param
  void setRemoteFonts(CefViewPluingState value);

  /// <summary
  /// Gets whether to enable or disable the remote fonts
  /// </summary
  /// <returnsTrue to enalbe; false to disable</returns
  CefViewPluingState remoteFonts() const;

  /// <summary
  /// Sets to enable or disable Javascript
  /// </summary
  /// <param name="value"True to enalbe; false to disable</param
  void setJavascript(CefViewPluingState value);

  /// <summary
  /// Gets whether to enable or disable Javascript
  /// </summary
  /// <returnsTrue to enalbe; false to disable</returns
  CefViewPluingState javascript() const;

  /// <summary
  /// Sets to enable or disable the permission of closing window from Javascript
  /// </summary
  /// <param name="value"True to enalbe; false to disable</param
  void setJavascriptCloseWindows(CefViewPluingState value);

  /// <summary
  /// Gets whether to enable or disable the permission of closing window from Javascript
  /// </summary
  /// <returnsTrue to enalbe; false to disable</returns
  CefViewPluingState javascriptCloseWindows() const;

  /// <summary
  /// Sets to enable or disable the permission of accessing clipboard from Javascript
  /// </summary
  /// <param name="value"True to enalbe; false to disable</param
  void setJavascriptAccessClipboard(CefViewPluingState value);

  /// <summary
  /// Gets whether to enable or disable the permission of accessing clipboard from Javascript
  /// </summary
  /// <returnsTrue to enalbe; false to disable</returns
  CefViewPluingState javascriptAccessClipboard() const;

  /// <summary
  /// Sets to enable or disable the permission of pasting DOM in Javascript
  /// </summary
  /// <param name="value"True to enalbe; false to disable</param
  void setJavascriptDomPaste(CefViewPluingState value);

  /// <summary
  /// Gets whether to enable or disable the permission of pasting DOM in Javascript
  /// </summary
  /// <returnsTrue to enalbe; false to disable</returns
  CefViewPluingState javascriptDomPaste() const;

  /// <summary
  /// Sets to enable or disable plugins
  /// </summary
  /// <param name="value"True to enalbe; false to disable</param
  void setPlugins(CefViewPluingState value);

  /// <summary
  /// Gets whether to enable or disable plugins
  /// </summary
  /// <returnsTrue to enalbe; false to disable</returns
  CefViewPluingState plugins() const;

  /// <summary
  /// Sets to enable or disable the permission of loading images
  /// </summary
  /// <param name="value"True to enalbe; false to disable</param
  void setImageLoading(CefViewPluingState value);

  /// <summary
  /// Gets whether to enable or disable the permission of loading images
  /// </summary
  /// <returnsTrue to enalbe; false to disable</returns
  CefViewPluingState imageLoading() const;

  /// <summary
  /// Sets to enable or disable the shrinking image standalone to fit
  /// </summary
  /// <param name="value"True to enalbe; false to disable</param
  void setImageShrinkStandaloneToFit(CefViewPluingState value);

  /// <summary
  /// Gets whether to enable or disable the shrinking image standalone to fit
  /// </summary
  /// <returnsTrue to enalbe; false to disable</returns
  CefViewPluingState imageShrinkStandaloneToFit() const;

  /// <summary
  /// Sets to enable or disable the resizing of text area
  /// </summary
  /// <param name="value"True to enalbe; false to disable</param
  void setTextAreaResize(CefViewPluingState value);

  /// <summary
  /// Gets whether to enable or disable the resizing of text area
  /// </summary
  /// <returnsTrue to enalbe; false to disable</returns
  CefViewPluingState textAreaResize() const;

  /// <summary
  /// Sets to enable or disable tab to links
  /// </summary
  /// <param name="value"True to enalbe; false to disable</param
  void setTabToLinks(CefViewPluingState value);

  /// <summary
  /// Gets whether to enable or disable  tab to links
  /// </summary
  /// <returnsTrue to enalbe; false to disable</returns
  CefViewPluingState tabToLinks() const;

  /// <summary
  /// Sets to enable or disable local storage
  /// </summary
  /// <param name="value"True to enalbe; false to disable</param
  void setLocalStorage(CefViewPluingState value);

  /// <summary
  /// Gets whether to enable or disable local storage
  /// </summary
  /// <returnsTrue to enalbe; false to disable</returns
  CefViewPluingState localStorage() const;

  /// <summary
  /// Sets to enable or disable database
  /// </summary
  /// <param name="value"True to enalbe; false to disable</param
  void setDatabases(CefViewPluingState value);

  /// <summary
  /// Gets whether to enable or disable database
  /// </summary
  /// <returnsTrue to enalbe; false to disable</returns
  CefViewPluingState databases() const;

  /// <summary
  /// Sets to enable or disable webGL
  /// </summary
  /// <param name="value"True to enalbe; false to disable</param
  void setWebGL(CefViewPluingState value);

  /// <summary
  /// Gets whether to enable or disable webGL
  /// </summary
  /// <returnsTrue to enalbe; false to disable</returns
  CefViewPluingState webGL() const;

  /// <summary
  /// Sets the background color
  /// </summary
  /// <param name="value"The color</param
  /// <remarks
  /// This only works if the web page has no background color set. The alpha component value
  /// will be adjusted to 0 or 255, it means if you pass a value with alpha value
  /// in the range of [1, 255], it will be accepted as 255. The default value is inherited from
  /// <see cref="QCefConfig::backgroundColor()"/
  /// </remarks
  void setBackgroundColor(const uint32_t& value);

  /// <summary
  /// Gets the background color
  /// </summary
  /// <returnsThe color</returns
  uint32_t backgroundColor() const;

protected:
  static void copyToCefBrowserSettings(const CCefSetting* setting, CefBrowserSettings& cs);
};

#endif
