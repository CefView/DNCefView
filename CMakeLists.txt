cmake_minimum_required(VERSION 3.20)
project(DNCefView)

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
add_subdirectory(src/CCefView)

# ################################################################
# .NET projects
set(CMAKE_FOLDER DNCefView)
set(CMAKE_DOTNET_SDK "Microsoft.NET.Sdk")
set(CMAKE_DOTNET_TARGET_FRAMEWORK "net8.0")
set(CMAKE_VS_GLOBALS
  "Authors=tishion"
  "Copyright=Copyright © 2025 tishion"
  "Description=An encapsulation of CefView for .NET"
  "PackageProjectUrl=https://github.com/CefView/DNCefView"
  "RepositoryUrl=https://github.com/CefView/DNCefView"
)
add_subdirectory(src/DNCefView)
add_subdirectory(src/DNCefView.WPF)

set(CMAKE_FOLDER Examples)
add_subdirectory(demo/DNCefView.WPF.Demo)