#include "CefSetting.h"

CCefSetting::CCefSetting()
{
  backgroundColor_ = 0xFFFFFFFF;
}

CCefSetting::~CCefSetting() {}

void
CCefSetting::setStandardFontFamily(const std::string& value)
{
  standardFontFamily_ = value;
}

const std::string&
CCefSetting::standardFontFamily() const
{
  return standardFontFamily_;
}

void
CCefSetting::setFixedFontFamily(const std::string& value)
{
  fixedFontFamily_ = value;
}

const std::string&
CCefSetting::fixedFontFamily() const
{
  return fixedFontFamily_;
}

void
CCefSetting::setSerifFontFamily(const std::string& value)
{
  serifFontFamily_ = value;
}

const std::string&
CCefSetting::serifFontFamily() const
{
  return serifFontFamily_;
}

void
CCefSetting::setSansSerifFontFamily(const std::string& value)
{
  sansSerifFontFamily_ = value;
}

const std::string&
CCefSetting::sansSerifFontFamily() const
{
  return sansSerifFontFamily_;
}

void
CCefSetting::setCursiveFontFamily(const std::string& value)
{
  cursiveFontFamily_ = value;
}

const std::string&
CCefSetting::cursiveFontFamily() const
{
  return cursiveFontFamily_;
}

void
CCefSetting::setFantasyFontFamily(const std::string& value)
{
  fantasyFontFamily_ = value;
}

const std::string&
CCefSetting::fantasyFontFamily() const
{
  return fantasyFontFamily_;
}

void
CCefSetting::setDefaultEncoding(const std::string& value)
{
  defaultEncoding_ = value;
}

const std::string&
CCefSetting::defaultEncoding() const
{
  return defaultEncoding_;
}

void
CCefSetting::setAcceptLanguageList(const std::string& value)
{
  acceptLanguageList_ = value;
}

const std::string&
CCefSetting::acceptLanguageList() const
{
  return acceptLanguageList_;
}

void
CCefSetting::setWindowlessFrameRate(const int value)
{
  windowlessFrameRate_ = value;
}

int
CCefSetting::windowlessFrameRate() const
{
  return windowlessFrameRate_.value_or(0);
}

void
CCefSetting::setDefaultFontSize(const int value)
{
  defaultFontSize_ = value;
}

int
CCefSetting::defaultFontSize() const
{
  return defaultFontSize_.value_or(0);
}

void
CCefSetting::setDefaultFixedFontSize(const int value)
{
  defaultFixedFontSize_ = value;
}

int
CCefSetting::defaultFixedFontSize() const
{
  return defaultFixedFontSize_.value_or(0);
}

void
CCefSetting::setMinimumFontSize(const int value)
{
  minimumFontSize_ = value;
}

int
CCefSetting::minimumFontSize() const
{
  return minimumFontSize_.value_or(0);
}

void
CCefSetting::setMinimumLogicalFontSize(const int value)
{
  minimumLogicalFontSize_ = value;
}

int
CCefSetting::minimumLogicalFontSize() const
{
  return minimumLogicalFontSize_.value_or(0);
}

void
CCefSetting::setRemoteFonts(CefViewPluingState value)
{
  remoteFonts_ = value;
}

CefViewPluingState
CCefSetting::remoteFonts() const
{
  return remoteFonts_;
}

void
CCefSetting::setJavascript(CefViewPluingState value)
{
  javascript_ = value;
}

CefViewPluingState
CCefSetting::javascript() const
{
  return javascript_;
}

void
CCefSetting::setJavascriptCloseWindows(CefViewPluingState value)
{
  javascriptCloseWindows_ = value;
}

CefViewPluingState
CCefSetting::javascriptCloseWindows() const
{
  return javascriptCloseWindows_;
}

void
CCefSetting::setJavascriptAccessClipboard(CefViewPluingState value)
{
  javascriptAccessClipboard_ = value;
}

CefViewPluingState
CCefSetting::javascriptAccessClipboard() const
{
  return javascriptAccessClipboard_;
}

void
CCefSetting::setJavascriptDomPaste(CefViewPluingState value)
{
  javascriptDomPaste_ = value;
}

CefViewPluingState
CCefSetting::javascriptDomPaste() const
{
  return javascriptDomPaste_;
}

void
CCefSetting::setPlugins(CefViewPluingState value)
{
  plugins_ = value;
}

CefViewPluingState
CCefSetting::plugins() const
{
  return plugins_;
}

void
CCefSetting::setImageLoading(CefViewPluingState value)
{
  imageLoading_ = value;
}

CefViewPluingState
CCefSetting::imageLoading() const
{
  return imageLoading_;
}

void
CCefSetting::setImageShrinkStandaloneToFit(CefViewPluingState value)
{
  imageShrinkStandaloneToFit_ = value;
}

CefViewPluingState
CCefSetting::imageShrinkStandaloneToFit() const
{
  return imageShrinkStandaloneToFit_;
}

void
CCefSetting::setTextAreaResize(CefViewPluingState value)
{
  textAreaResize_ = value;
}

CefViewPluingState
CCefSetting::textAreaResize() const
{
  return textAreaResize_;
}

void
CCefSetting::setTabToLinks(CefViewPluingState value)
{
  tabToLinks_ = value;
}

CefViewPluingState
CCefSetting::tabToLinks() const
{
  return tabToLinks_;
}

void
CCefSetting::setLocalStorage(CefViewPluingState value)
{
  localStorage_ = value;
}

CefViewPluingState
CCefSetting::localStorage() const
{
  return localStorage_;
}

void
CCefSetting::setDatabases(CefViewPluingState value)
{
  databases_ = value;
}

CefViewPluingState
CCefSetting::databases() const
{
  return databases_;
}

void
CCefSetting::setWebGL(CefViewPluingState value)
{
  webGL_ = value;
}

CefViewPluingState
CCefSetting::webGL() const
{
  return webGL_;
}

void
CCefSetting::setBackgroundColor(const uint32_t& value)
{
  backgroundColor_ = value;
}

uint32_t
CCefSetting::backgroundColor() const
{
  return backgroundColor_.value_or(-1);
}

void
CCefSetting::copyToCefBrowserSettings(const CCefSetting* qs, CefBrowserSettings& cs)
{
  if (!qs) {
    CCefSetting defaultSettings;
    cs.background_color = qs->backgroundColor_.value_or(0xFFFFFFFF);
  }

  if (!qs->standardFontFamily_.empty())
    CefString(&cs.standard_font_family) = qs->standardFontFamily_;

  if (!qs->fixedFontFamily_.empty())
    CefString(&cs.fixed_font_family) = qs->fixedFontFamily_;

  if (!qs->serifFontFamily_.empty())
    CefString(&cs.serif_font_family) = qs->serifFontFamily_;

  if (!qs->sansSerifFontFamily_.empty())
    CefString(&cs.sans_serif_font_family) = qs->sansSerifFontFamily_;

  if (!qs->cursiveFontFamily_.empty())
    CefString(&cs.cursive_font_family) = qs->cursiveFontFamily_;

  if (!qs->fantasyFontFamily_.empty())
    CefString(&cs.fantasy_font_family) = qs->fantasyFontFamily_;

  if (!qs->defaultEncoding_.empty())
    CefString(&cs.default_encoding) = qs->defaultEncoding_;

  if (qs->windowlessFrameRate_)
    cs.windowless_frame_rate = qs->windowlessFrameRate_.value_or(0);

  if (qs->defaultFontSize_)
    cs.default_font_size = qs->defaultFontSize_.value_or(0);

  if (qs->defaultFixedFontSize_)
    cs.default_fixed_font_size = qs->defaultFixedFontSize_.value_or(0);

  if (qs->minimumFontSize_)
    cs.minimum_font_size = qs->minimumFontSize_.value_or(0);

  if (qs->minimumLogicalFontSize_)
    cs.minimum_logical_font_size = qs->minimumLogicalFontSize_.value_or(0);

  cs.remote_fonts = (cef_state_t)(qs->remoteFonts_);

  cs.javascript = (cef_state_t)(qs->javascript_);

  cs.javascript_close_windows = (cef_state_t)(qs->javascriptCloseWindows_);

  cs.javascript_access_clipboard = (cef_state_t)(qs->javascriptAccessClipboard_);

  cs.javascript_dom_paste = (cef_state_t)(qs->javascriptDomPaste_);

#if defined(CEF_VERSION_MAJOR) && CEF_VERSION_MAJOR < 100
  //
  cs.plugins = (cef_state_t)(qs->plugins_);
#endif

  cs.image_loading = (cef_state_t)(qs->imageLoading_);

  cs.image_shrink_standalone_to_fit = (cef_state_t)(qs->imageShrinkStandaloneToFit_);

  cs.text_area_resize = (cef_state_t)(qs->textAreaResize_);

  cs.tab_to_links = (cef_state_t)(qs->tabToLinks_);

  cs.local_storage = (cef_state_t)(qs->localStorage_);

  cs.databases = (cef_state_t)(qs->databases_);

  cs.webgl = (cef_state_t)(qs->webGL_);

  cs.background_color = qs->backgroundColor_.value_or(0xFFFFFFFF);
}
