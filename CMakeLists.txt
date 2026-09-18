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
set(CEF_SDK_VERSION "126.2.18+g3647d39+chromium-126.0.6478.183")
set(CEFVIEW_CORE_VERSION "9415c2c5d893c7e3ddc10fc917671e81f1bfe8ce")
add_subdirectory(src/CCefView)

# ################################################################
# .NET projects settings
set(CMAKE_FOLDER DNCefView)
set(CMAKE_DOTNET_SDK "Microsoft.NET.Sdk")
set(CMAKE_DOTNET_TARGET_FRAMEWORK "net8.0")
set(CMAKE_VS_GLOBALS
  "Authors=CefView"
  "Company=CefView"
  "Version=1.0.8"
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
