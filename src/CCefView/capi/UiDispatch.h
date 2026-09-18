#pragma once
#include <include/cef_task.h>
#include <include/wrapper/cef_closure_task.h>
#include <future>
#include <memory>
#include <stdexcept>

// All browser member access is serialized with its CEF callbacks. Captured
// strings/arrays remain valid because the caller waits for the operation.
template<class F> auto OnCefUi(F function) -> decltype(function()) {
  if (CefCurrentlyOn(TID_UI)) { try { return function(); } catch (...) { return decltype(function())(); } }
  auto task = std::make_shared<std::packaged_task<decltype(function())()>>(std::move(function));
  auto result = task->get_future();
  if (!CefPostTask(TID_UI, base::BindOnce([](decltype(task) t) { (*t)(); }, task)))
    return decltype(function())();
  try { return result.get(); } catch (...) { return decltype(function())(); }
}
