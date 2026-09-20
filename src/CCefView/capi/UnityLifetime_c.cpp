#include "CefContext_c.h"
#include "CefContext.h"
#include <mutex>

// The editor keeps CEF initialized across managed-domain reloads. The configuration
// is copied by CCefContext; no managed pointer or callback is retained by this lease.
static CCefContext* editorContext = nullptr;
static std::mutex contextMutex;
// ABI 3 adds the Dialog/Download/Find/BeforeUnload/ContextMenu/Permission/
// RenderProcessTerminated callback fields and the editor-command, find,
// file-dialog, download, context-menu and permission answer exports.
// ABI 4 adds the leading void* host parameter to every pfn callback (static
// thunks for IL2CPP, which cannot marshal instance-method delegates) and the
// two-phase CCefBrowser_new0/CCefBrowser_start construction.
extern "C" CCEFVIEW_EXPORT int CCefView_GetUnityAbiVersion() { return 4; }
extern "C" CCEFVIEW_EXPORT CCefContext* CCefContext_AcquireEditor(const CCefConfig* config) {
  std::lock_guard<std::mutex> lock(contextMutex);
  if (!editorContext) editorContext = new CCefContext(config);
  if (!editorContext->isInitialized()) { delete editorContext; editorContext = nullptr; }
  return editorContext;
}
