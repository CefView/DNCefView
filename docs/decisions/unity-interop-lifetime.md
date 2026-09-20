# Decision: owned snapshots and explicit browser lifetime

## Context
The Unity integration previously mixed incompatible binary/binding revisions, retained CEF frame resources after their callback, and used reflection to reattach CEF across managed-domain reloads. A crash dump showed Unity accessing a texture after its release.

## Decision
Unity ABI 2 uses a native editor lease with an owned configuration. Browsers retain their managed callbacks until OnBeforeClose and a native UI-thread deletion barrier. Public browser calls hold short managed leases; deletion blocks new calls, drains existing calls, then executes on CEF's UI thread. No managed monitor remains held while a native UI-thread operation runs.

The GPU plugin copies borrowed frames to owned snapshots before returning from CEF. Unity's native render event copies snapshots into Unity-owned targets. Each view and popup has independent state. Synchronous GPU completion is intentional; performance changes require equivalent lifetime guarantees and measurements.

## Generated code policy
`scripts/gen.py <cef-include-path>` now writes candidate files to `.generated-bindings/`, outside maintained source. Compare candidates with `src/CCefView/capi` and `src/DNCefView/AutoGen`; merge API changes while preserving one-byte booleans, UTF-8 strings, callback layout, UI dispatch, call leases, bounds checks, and shutdown sequencing. Do not overwrite hardened snapshots with raw generator output.

## Consequences and validation
This changes the Unity-specific ABI; deploy the complete native/managed/resource set. The CEF baseline remains pinned to 126.2.18 and the upstream API snapshot is v1.0.13 plus local hardening. The NuGet package's existing version metadata is not a release of these uncommitted changes.

Validate with native SurfaceOwnership tests and the sibling Unity Regression scene, including concurrent deletion, early disposal, malformed JSON, Unicode, dialog continuation and repeated managed reloads. Player shutdown must occur on the initialization thread after all clients drain. IL2CPP, non-D3D11 acceleration, device-reset recovery and general CEF feature parity are not established by these checks.

## 2026-09-21: generator double-hash verification (GEN-01 machine contract)

`gen.py` runs against CEF 127.3.5 + MSVC/WinSDK includes (see gen-run.log
invocation). Two consecutive runs produce identical tree hashes
(`1ee8165a…`, 28 files) — reproducibility holds.

Known generator defect surfaced by candidate-vs-hardened audit, NOT fixed
(hardened snapshots are unaffected — the GEN-01 contract is why):
- `csharp.py:158` class-name rule strips ONE leading `C` from any C++ class:
  `CCefBrowser` -> `CefBrowser` (intended), but CEF-native `CefDragData` ->
  `efDragData` (wrong; it also lands in the CefBrowser candidate file). The
  rule assumes capi only exposes `CCef*` wrappers; `CefDragData` is a
  forward-declared CEF type that leaks through the CLASS_DECL path.
- Trailing-space `// Source:` comments and LF-vs-CRLF differ from the
  hardened files (cosmetic; normalized diff separates them from real drift).
- The hardened lifetime hardening (ObjectDisposedException getters,
  Interlocked dispose, `[MarshalAs(I1)] bool`) is post-generation work by
  design; candidates never carry it.

When merging future upstream API changes: run gen.py twice, hash-compare,
then diff candidates against snapshots EXCLUDING the known-drift classes
above; merge by hand into the hardened files. Never copy candidates over.
