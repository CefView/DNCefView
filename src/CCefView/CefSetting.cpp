#include "CefSetting.h"

#include <optional>

class CCefSetting::Implementation
{
public:
  std::string standardFontFamily_;
  std::string fixedFontFamily_;
  std::string serifFontFamily_;
  std::string sansSerifFontFamily_;
  std::string cursiveFontFamily_;
  std::string fantasyFontFamily_;
  std::string defaultEncoding_;
  std::string acceptLanguageList_;

  std::optional<uint32_t> backgroundColor_;

  std::optional<int> windowlessFrameRate_;
  std::optional<int> defaultFontSize_;
  std::optional<int> defaultFixedFontSize_;
  std::optional<int> minimumFontSize_;
  std::optional<int> minimumLogicalFontSize_;

  CefViewPluingState remoteFonts_ = STATE_DEFAULT;
  CefViewPluingState javascript_ = STATE_DEFAULT;
  CefViewPluingState javascriptCloseWindows_ = STATE_DEFAULT;
  CefViewPluingState javascriptAccessClipboard_ = STATE_DEFAULT;
  CefViewPluingState javascriptDomPaste_ = STATE_DEFAULT;
  CefViewPluingState plugins_ = STATE_DEFAULT;
  CefViewPluingState imageLoading_ = STATE_DEFAULT;
  CefViewPluingState imageShrinkStandaloneToFit_ = STATE_DEFAULT;
  CefViewPluingState textAreaResize_ = STATE_DEFAULT;
  CefViewPluingState tabToLinks_ = STATE_DEFAULT;
  CefViewPluingState localStorage_ = STATE_DEFAULT;
  CefViewPluingState databases_ = STATE_DEFAULT;
  CefViewPluingState webGL_ = STATE_DEFAULT;
};

CCefSetting::CCefSetting()
  : pImpl_(std::make_unique<Implementation>())
{
  pImpl_->backgroundColor_ = 0xFFFFFFFF;
}

CCefSetting::~CCefSetting() {}

void
CCefSetting::setStandardFontFamily(const std::string& value)
{
  pImpl_->standardFontFamily_ = value;
}

const std::string&
CCefSetting::standardFontFamily() const
{
  return pImpl_->standardFontFamily_;
}

void
CCefSetting::setFixedFontFamily(const std::string& value)
{
  pImpl_->fixedFontFamily_ = value;
}

const std::string&
CCefSetting::fixedFontFamily() const
{
  return pImpl_->fixedFontFamily_;
}

void
CCefSetting::setSerifFontFamily(const std::string& value)
{
  pImpl_->serifFontFamily_ = value;
}

const std::string&
CCefSetting::serifFontFamily() const
{
  return pImpl_->serifFontFamily_;
}

void
CCefSetting::setSansSerifFontFamily(const std::string& value)
{
  pImpl_->sansSerifFontFamily_ = value;
}

const std::string&
CCefSetting::sansSerifFontFamily() const
{
  return pImpl_->sansSerifFontFamily_;
}

void
CCefSetting::setCursiveFontFamily(const std::string& value)
{
  pImpl_->cursiveFontFamily_ = value;
}

const std::string&
CCefSetting::cursiveFontFamily() const
{
  return pImpl_->cursiveFontFamily_;
}

void
CCefSetting::setFantasyFontFamily(const std::string& value)
{
  pImpl_->fantasyFontFamily_ = value;
}

const std::string&
CCefSetting::fantasyFontFamily() const
{
  return pImpl_->fantasyFontFamily_;
}

void
CCefSetting::setDefaultEncoding(const std::string& value)
{
  pImpl_->defaultEncoding_ = value;
}

const std::string&
CCefSetting::defaultEncoding() const
{
  return pImpl_->defaultEncoding_;
}

void
CCefSetting::setAcceptLanguageList(const std::string& value)
{
  pImpl_->acceptLanguageList_ = value;
}

const std::string&
CCefSetting::acceptLanguageList() const
{
  return pImpl_->acceptLanguageList_;
}

void
CCefSetting::setWindowlessFrameRate(const int value)
{
  pImpl_->windowlessFrameRate_ = value;
}

int
CCefSetting::windowlessFrameRate() const
{
  return pImpl_->windowlessFrameRate_.value_or(0);
}

void
CCefSetting::setDefaultFontSize(const int value)
{
  pImpl_->defaultFontSize_ = value;
}

int
CCefSetting::defaultFontSize() const
{
  return pImpl_->defaultFontSize_.value_or(0);
}

void
CCefSetting::setDefaultFixedFontSize(const int value)
{
  pImpl_->defaultFixedFontSize_ = value;
}

int
CCefSetting::defaultFixedFontSize() const
{
  return pImpl_->defaultFixedFontSize_.value_or(0);
}

void
CCefSetting::setMinimumFontSize(const int value)
{
  pImpl_->minimumFontSize_ = value;
}

int
CCefSetting::minimumFontSize() const
{
  return pImpl_->minimumFontSize_.value_or(0);
}

void
CCefSetting::setMinimumLogicalFontSize(const int value)
{
  pImpl_->minimumLogicalFontSize_ = value;
}

int
CCefSetting::minimumLogicalFontSize() const
{
  return pImpl_->minimumLogicalFontSize_.value_or(0);
}

void
CCefSetting::setRemoteFonts(CefViewPluingState value)
{
  pImpl_->remoteFonts_ = value;
}

CefViewPluingState
CCefSetting::remoteFonts() const
{
  return pImpl_->remoteFonts_;
}

void
CCefSetting::setJavascript(CefViewPluingState value)
{
  pImpl_->javascript_ = value;
}

CefViewPluingState
CCefSetting::javascript() const
{
  return pImpl_->javascript_;
}

void
CCefSetting::setJavascriptCloseWindows(CefViewPluingState value)
{
  pImpl_->javascriptCloseWindows_ = value;
}

CefViewPluingState
CCefSetting::javascriptCloseWindows() const
{
  return pImpl_->javascriptCloseWindows_;
}

void
CCefSetting::setJavascriptAccessClipboard(CefViewPluingState value)
{
  pImpl_->javascriptAccessClipboard_ = value;
}

CefViewPluingState
CCefSetting::javascriptAccessClipboard() const
{
  return pImpl_->javascriptAccessClipboard_;
}

void
CCefSetting::setJavascriptDomPaste(CefViewPluingState value)
{
  pImpl_->javascriptDomPaste_ = value;
}

CefViewPluingState
CCefSetting::javascriptDomPaste() const
{
  return pImpl_->javascriptDomPaste_;
}

void
CCefSetting::setPlugins(CefViewPluingState value)
{
  pImpl_->plugins_ = value;
}

CefViewPluingState
CCefSetting::plugins() const
{
  return pImpl_->plugins_;
}

void
CCefSetting::setImageLoading(CefViewPluingState value)
{
  pImpl_->imageLoading_ = value;
}

CefViewPluingState
CCefSetting::imageLoading() const
{
  return pImpl_->imageLoading_;
}

void
CCefSetting::setImageShrinkStandaloneToFit(CefViewPluingState value)
{
  pImpl_->imageShrinkStandaloneToFit_ = value;
}

CefViewPluingState
CCefSetting::imageShrinkStandaloneToFit() const
{
  return pImpl_->imageShrinkStandaloneToFit_;
}

void
CCefSetting::setTextAreaResize(CefViewPluingState value)
{
  pImpl_->textAreaResize_ = value;
}

CefViewPluingState
CCefSetting::textAreaResize() const
{
  return pImpl_->textAreaResize_;
}

void
CCefSetting::setTabToLinks(CefViewPluingState value)
{
  pImpl_->tabToLinks_ = value;
}

CefViewPluingState
CCefSetting::tabToLinks() const
{
  return pImpl_->tabToLinks_;
}

void
CCefSetting::setLocalStorage(CefViewPluingState value)
{
  pImpl_->localStorage_ = value;
}

CefViewPluingState
CCefSetting::localStorage() const
{
  return pImpl_->localStorage_;
}

void
CCefSetting::setDatabases(CefViewPluingState value)
{
  pImpl_->databases_ = value;
}

CefViewPluingState
CCefSetting::databases() const
{
  return pImpl_->databases_;
}

void
CCefSetting::setWebGL(CefViewPluingState value)
{
  pImpl_->webGL_ = value;
}

CefViewPluingState
CCefSetting::webGL() const
{
  return pImpl_->webGL_;
}

void
CCefSetting::setBackgroundColor(const uint32_t& value)
{
  pImpl_->backgroundColor_ = value;
}

uint32_t
CCefSetting::backgroundColor() const
{
  return pImpl_->backgroundColor_.value_or(-1);
}

void
CCefSetting::copyToCefBrowserSettings(const CCefSetting* qs, CefBrowserSettings& cs)
{
  if (!qs) {
    CCefSetting defaultSettings;
    cs.background_color = qs->pImpl_->backgroundColor_.value_or(0xFFFFFFFF);
  }

  if (!qs->pImpl_->standardFontFamily_.empty())
    CefString(&cs.standard_font_family) = qs->pImpl_->standardFontFamily_;

  if (!qs->pImpl_->fixedFontFamily_.empty())
    CefString(&cs.fixed_font_family) = qs->pImpl_->fixedFontFamily_;

  if (!qs->pImpl_->serifFontFamily_.empty())
    CefString(&cs.serif_font_family) = qs->pImpl_->serifFontFamily_;

  if (!qs->pImpl_->sansSerifFontFamily_.empty())
    CefString(&cs.sans_serif_font_family) = qs->pImpl_->sansSerifFontFamily_;

  if (!qs->pImpl_->cursiveFontFamily_.empty())
    CefString(&cs.cursive_font_family) = qs->pImpl_->cursiveFontFamily_;

  if (!qs->pImpl_->fantasyFontFamily_.empty())
    CefString(&cs.fantasy_font_family) = qs->pImpl_->fantasyFontFamily_;

  if (!qs->pImpl_->defaultEncoding_.empty())
    CefString(&cs.default_encoding) = qs->pImpl_->defaultEncoding_;

  if (!qs->pImpl_->acceptLanguageList_.empty())
    CefString(&cs.accept_language_list) = qs->pImpl_->acceptLanguageList_;

  if (qs->pImpl_->windowlessFrameRate_)
    cs.windowless_frame_rate = qs->pImpl_->windowlessFrameRate_.value_or(0);

  if (qs->pImpl_->defaultFontSize_)
    cs.default_font_size = qs->pImpl_->defaultFontSize_.value_or(0);

  if (qs->pImpl_->defaultFixedFontSize_)
    cs.default_fixed_font_size = qs->pImpl_->defaultFixedFontSize_.value_or(0);

  if (qs->pImpl_->minimumFontSize_)
    cs.minimum_font_size = qs->pImpl_->minimumFontSize_.value_or(0);

  if (qs->pImpl_->minimumLogicalFontSize_)
    cs.minimum_logical_font_size = qs->pImpl_->minimumLogicalFontSize_.value_or(0);

  cs.remote_fonts = (cef_state_t)(qs->pImpl_->remoteFonts_);

  cs.javascript = (cef_state_t)(qs->pImpl_->javascript_);

  cs.javascript_close_windows = (cef_state_t)(qs->pImpl_->javascriptCloseWindows_);

  cs.javascript_access_clipboard = (cef_state_t)(qs->pImpl_->javascriptAccessClipboard_);

  cs.javascript_dom_paste = (cef_state_t)(qs->pImpl_->javascriptDomPaste_);

  cs.plugins = (cef_state_t)(qs->pImpl_->plugins_);

  cs.image_loading = (cef_state_t)(qs->pImpl_->imageLoading_);

  cs.image_shrink_standalone_to_fit = (cef_state_t)(qs->pImpl_->imageShrinkStandaloneToFit_);

  cs.text_area_resize = (cef_state_t)(qs->pImpl_->textAreaResize_);

  cs.tab_to_links = (cef_state_t)(qs->pImpl_->tabToLinks_);

  cs.local_storage = (cef_state_t)(qs->pImpl_->localStorage_);

  cs.databases = (cef_state_t)(qs->pImpl_->databases_);

  cs.webgl = (cef_state_t)(qs->pImpl_->webGL_);

  cs.background_color = qs->pImpl_->backgroundColor_.value_or(0xFFFFFFFF);
}
