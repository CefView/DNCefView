# Repository Guidelines

## Structure
Native CEF integration is in `src/CCefView`, its exported C interface in `capi`, and managed bindings in `src/DNCefView`. `AutoGen` contains a checked-in binding snapshot with interop/lifetime hardening; regeneration must preserve those contracts. The sibling Unity project consumes an exact source copy through the workspace deploy script.

## Build
Use VS2022 x64, CMake and the .NET 8 target SDK. Configure `.build/windows.x86_64` with `-DPROJECT_ARCH=x86_64`, then `cmake --build .build/windows.x86_64 --config Debug --parallel 8`. The sibling `../UCefView/Tools/build.ps1 -Target Bindings` wraps these commands; `../UCefView/Tools/validate.ps1` runs the complete integration suite. CEF and CefViewCore are pinned. Change dependency versions deliberately, never through a floating branch.

## Interop and Lifetime
Native bool is one byte and callback strings are UTF-8. Match callback field order and signatures exactly. Browser operations run on CEF's UI thread. Keep managed callback delegates rooted until close completes and the native deletion barrier returns. Do not delete a browser while callbacks are in flight. CEF shutdown belongs to its initialization thread, not a finalizer. The native editor lease owns a copied configuration across Unity managed-domain reloads.

## Validation
Build both native and managed code, deploy the complete matched runtime, and run Unity's Regression scene plus a standalone Player. Include malformed JSON, Unicode callbacks, focus, load errors, per-browser sizes and repeated close/recreate. Review generated diffs; do not infer ABI correctness from successful compilation. Tests that only compare copied source do not replace runtime checks.

## Style and Review
Follow `.clang-format` and `.editorconfig`; use four-space C# indentation. Preserve ownership comments and document synchronous callback constraints. Use imperative `fix:`, `feat:` or `chore:` commits. PRs explain observable behavior, ABI/dependency changes, executed validation and remaining limits. Do not include generated builds or machine-specific paths.
