#include "CCefClientDelegate.h"

#include <CefBrowser.h>

bool
CCefClientDelegate::onPreKeyEvent(CefRefPtr<CefBrowser>& browser,
                                  const CefKeyEvent& event,
                                  CefEventHandle os_event,
                                  bool* is_keyboard_shortcut)
{
#if defined(OS_MACOS)
 if (event.modifiers & EVENTFLAG_COMMAND_DOWN && event.type == KEYEVENT_RAWKEYDOWN) {
   switch (event.native_key_code) {
     case 0: // A
     case 6: // Z
     case 7: // X
     case 8: // C
     case 9: // V
     case 16: // Y
       *is_keyboard_shortcut = true;
       break;
   }
 }
#endif
  
  return false;
}

bool
CCefClientDelegate::onKeyEvent(CefRefPtr<CefBrowser>& browser, const CefKeyEvent& event, CefEventHandle os_event)
{
#if defined(OS_MACOS)
 if (event.modifiers & EVENTFLAG_COMMAND_DOWN && event.type == KEYEVENT_RAWKEYDOWN) {
   switch (event.native_key_code) {
     case 0: // A
       browser->GetFocusedFrame()->SelectAll();
       break;
     case 6: // Z
       browser->GetFocusedFrame()->Undo();
       break;
     case 7: // X
       browser->GetFocusedFrame()->Cut();
       break;
     case 8: // C
       browser->GetFocusedFrame()->Copy();
       break;
     case 9: // V
       browser->GetFocusedFrame()->Paste();
       break;
     case 16: // Y
       browser->GetFocusedFrame()->Redo();
       break;
   }
 }
#endif
  
  return false;
}
