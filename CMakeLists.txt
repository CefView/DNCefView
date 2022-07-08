#
# The main config file for DNCef
#
cmake_minimum_required(VERSION 3.9.1)
project(DNCef)

option(USE_SANDBOX "Enable CEF Sandbox" OFF)
option(USE_OSR "Enable CEF Offscreen rendering" ON)
option(STATIC_CRT "Use MultiThreaded linkage for MSVC" OFF)

# Only generate Debug and Release configuration types.
set(CMAKE_CONFIGURATION_TYPES Debug Release)

if(NOT CMAKE_BUILD_TYPE)
  set(CMAKE_BUILD_TYPE "Debug")
endif()

# Use folders in the resulting project files.
set_property(GLOBAL PROPERTY USE_FOLDERS ON)
set_property(GLOBAL PROPERTY PREDEFINED_TARGETS_FOLDER "~CMakeTargets")

# C standard
set(CMAKE_C_STANDARD_REQUIRED ON)
set(CMAKE_C_STANDARD 11)

# C++ standard
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_STANDARD 17)

add_definitions(-D_SILENCE_CXX17_RESULT_OF_DEPRECATION_WARNING=1)

# Determine the project architecture.
if(NOT DEFINED PROJECT_ARCH)
  if(OS_WINDOWS AND "${CMAKE_GENERATOR_PLATFORM}" STREQUAL "arm64")
    set(PROJECT_ARCH "arm64")
  elseif(CMAKE_SIZEOF_VOID_P MATCHES 8)
    set(PROJECT_ARCH "x86_64")
  else()
    set(PROJECT_ARCH "x86")
  endif()
endif()

if("${CMAKE_SYSTEM_NAME}" STREQUAL "Darwin")
  set(OS_MACOS 1)
  set(OS_POSIX 1)
  set(DOTNET_RID_OS "osx")
  add_definitions(-DOS_MACOS=1 -DOS_POSIX=1)
  add_compile_options(-fdeclspec)

  # Target architecture.
  if(PROJECT_ARCH STREQUAL "x86_64")
    set(CMAKE_OSX_ARCHITECTURES "x86_64")
  elseif(PROJECT_ARCH STREQUAL "arm64")
    set(CMAKE_OSX_ARCHITECTURES "arm64")
  else()
    set(CMAKE_OSX_ARCHITECTURES "i386")
  endif()
elseif("${CMAKE_SYSTEM_NAME}" STREQUAL "Linux")
  set(OS_LINUX 1)
  set(OS_POSIX 1)
  set(DOTNET_RID_OS "linux")
  add_definitions(-DOS_LINUX=1 -DOS_POSIX=1)

  # Target architecture.
  if(PROJECT_ARCH STREQUAL "x86_64")
    # x86 64-bit architecture.
    add_compile_options(-m64 -march=x86-64)
    add_link_options(-m64)
  elseif(PROJECT_ARCH STREQUAL "x86")
    # x86 32-bit architecture.
    add_compile_options(-m32)
    add_link_options(-m32)
  endif()
elseif("${CMAKE_SYSTEM_NAME}" STREQUAL "Windows")
  set(OS_WINDOWS 1)
  set(DOTNET_RID_OS "win")
  add_definitions(-DOS_WINDOWS=1)
endif()

# Detect Arch
if(OS_WINDOWS AND "${CMAKE_GENERATOR_PLATFORM}" STREQUAL "arm64")
  set(DOTNET_RID_ARCH "arm64")
elseif(CMAKE_SIZEOF_VOID_P MATCHES 8)
  set(DOTNET_RID_ARCH "x64")
else()
  set(DOTNET_RID_ARCH "x86")
endif()

set(CCefView_LIB_TYPE SHARED)

set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}/output/$<CONFIG>/bin)
set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}/output/$<CONFIG>/bin)
set(CMAKE_ARCHIVE_OUTPUT_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}/output/$<CONFIG>/lib)

# Config the CEF
# ##############################################################
if(OS_WINDOWS)
  add_link_options(/DEBUG)

  # Disable the sandbox on Windows, because the sandbox.lib is MT which
  # is conflict with Qt
  set(USE_SANDBOX OFF CACHE BOOL "Disable sandbox on Windows" FORCE)
  add_compile_options("/M$<IF:$<BOOL:${STATIC_CRT}>,T,D>$<$<CONFIG:Debug>:d>")
else()
  add_compile_options(
    "-g"
    "$<$<CONFIG:DEBUG>:-O0>"
    "$<$<CONFIG:RELEASE>:-O3>"
  )
endif()

if(OS_MACOS)
  set(CMAKE_OSX_DEPLOYMENT_TARGET 10.11)
endif()

add_subdirectory(thirdparty/json)
add_subdirectory(thirdparty/cefviewcore)

# Config CCefView target
# ##############################################################
add_subdirectory(src/CCefView)
add_subdirectory(src/DNCef)

# ##############################################################
add_subdirectory(src/DNCef.WPF/DNCef.WPF)
