cmake_minimum_required(VERSION 3.20)
project(DNCefView LANGUAGES CXX CSharp)

if(CMAKE_SYSTEM_NAME STREQUAL "Windows")
  message(STATUS "Target system is Windows")
  set(OS_WINDOWS TRUE)
  set(DOTNET_RID_OS "win")
elseif(CMAKE_SYSTEM_NAME STREQUAL "Darwin")
  message(STATUS "Target system is Apple (macOS/iOS)")
  set(OS_MACOS TRUE)
  set(DOTNET_RID_OS "osx")
elseif(CMAKE_SYSTEM_NAME STREQUAL "Linux")
  set(OS_LINUX TRUE)
  message(STATUS "Target system is Linux")
  set(DOTNET_RID_OS "linux")
endif()

if(OS_WINDOWS AND "${CMAKE_GENERATOR_PLATFORM}" STREQUAL "arm64")
  set(DOTNET_RID_ARCH "arm64")
elseif(CMAKE_SIZEOF_VOID_P MATCHES 8)
  set(DOTNET_RID_ARCH "x64")
else()
  set(DOTNET_RID_ARCH "x86")
endif()

set(DOTNET_RID "${DOTNET_RID_OS}-${DOTNET_RID_ARCH}")

# Only generate Debug and Release configuration types.
set(CMAKE_CONFIGURATION_TYPES Debug Release)

if(NOT CMAKE_BUILD_TYPE)
  set(CMAKE_BUILD_TYPE "Debug")
endif()

# Use folders in the resulting project files.
set_property(GLOBAL PROPERTY USE_FOLDERS ON)
set_property(GLOBAL PROPERTY PREDEFINED_TARGETS_FOLDER "~CMakeTargets")

# ################################################################
# Native projects
set(CMAKE_FOLDER CCefView)
# 142.0.15/142.0.17 were attempted and rolled back on this machine: the OSR
# renderer stays alive, answers GetViewRect and completes loads, but no
# OnPaint/OnAcceleratedPaint ever arrives (see review-evidence native-diag
# traces, 2026-09-19). 127.3.5 keeps the 125-127 chrome_runtime workaround
# (chromiumembedded/cef#3685) and passed the full regression on 126/127 guards.
set(CEF_SDK_VERSION "127.3.5+g114ea2a+chromium-127.0.6533.120")
# Pinned to the feat/unity-callbacks tip (onBeforeBrowse, onRenderProcessTerminated,
# onFindResult, onFileDialogEx) until the upstream MRs merge; repin to the upstream
# merge commit afterwards. Upstream parent: 9415c2c5d893c7e3ddc10fc917671e81f1bfe8ce.
set(CEFVIEW_CORE_VERSION "99b83e74c6367b78c0d67a32c9a0b0dcb2050688")
add_subdirectory(src/CCefView)

# ################################################################
# .NET projects settings
set(CMAKE_FOLDER DNCefView)
set(CMAKE_DOTNET_SDK "Microsoft.NET.Sdk")
set(CMAKE_DOTNET_TARGET_FRAMEWORK "net8.0")
set(CMAKE_VS_GLOBALS
  "Authors=CefView"
  "Company=CefView"
  "Version=1.1.0"
  "Copyright=Copyright © 2026 CefView"
  "Description=.NET Implementation of CefView"
  "PackageTags=CefView CEF"
  "PackageLicenseExpression=MIT"
  "PackageRequireLicenseAcceptance false"
  "PackageProjectUrl=https://github.com/CefView/DNCefView"
  "RepositoryUrl=https://github.com/CefView/DNCefView"
  "IncludeSymbols=true"
  "SymbolPackageFormat=snupkg"
  "PublishRepositoryUrl=true"
  "EmbedUntrackedSources=false"
  "GeneratePackageOnBuild=true"
  "AllowUnsafeBlocks=true"
  "Nullable=enable"
)

# ###################################################################
# DNCefView projects
add_subdirectory(src/DNCefView)
