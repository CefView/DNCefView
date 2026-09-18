#include "CefContext_c.h"
#include "CefContext.h"
#include <mutex>

// The editor keeps CEF initialized across managed-domain reloads. The configuration
// is copied by CCefContext; no managed pointer or callback is retained by this lease.
static CCefContext* editorContext = nullptr;
static std::mutex contextMutex;
extern "C" CCEFVIEW_EXPORT int CCefView_GetUnityAbiVersion() { return 2; }
extern "C" CCEFVIEW_EXPORT CCefContext* CCefContext_AcquireEditor(const CCefConfig* config) {
  std::lock_guard<std::mutex> lock(contextMutex);
  if (!editorContext) editorContext = new CCefContext(config);
  if (!editorContext->isInitialized()) { delete editorContext; editorContext = nullptr; }
  return editorContext;
}
