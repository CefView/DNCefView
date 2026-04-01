cmake_minimum_required(VERSION 3.20)
project(CCefView LANGUAGES CXX)

# C standard
set(CMAKE_C_STANDARD_REQUIRED ON)
set(CMAKE_C_STANDARD 11)

# C++ standard
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_STANDARD 20)

# Only generate Debug and Release configuration types.
set(CMAKE_CONFIGURATION_TYPES Debug Release)

if(NOT CMAKE_BUILD_TYPE)
  set(CMAKE_BUILD_TYPE "Debug")
endif()

# Use folders in the resulting project files.
set_property(GLOBAL PROPERTY USE_FOLDERS ON)
set_property(GLOBAL PROPERTY PREDEFINED_TARGETS_FOLDER "~CMakeTargets")

add_definitions(-D_SILENCE_CXX17_RESULT_OF_DEPRECATION_WARNING=1)

if(CMAKE_SYSTEM_NAME STREQUAL "Windows")
  message(STATUS "Target system is Windows")
  set(OS_WINDOWS TRUE)
  set(DOTNET_RID_OS "win")
elseif(CMAKE_SYSTEM_NAME STREQUAL "Darwin")
  message(STATUS "Target system is Apple (macOS/iOS)")
  set(OS_MACOS TRUE)
  set(DOTNET_RID_OS "osx")

  add_compile_options(
    "-g"
    "-fdeclspec"
    "-Wno-overriding-option"
    "-Wno-overriding-t-option"
    "-Wno-unknown-warning-option"
    "$<$<CONFIG:DEBUG>:-O0>"
    "$<$<CONFIG:RELEASE>:-O3>"
  )
elseif(CMAKE_SYSTEM_NAME STREQUAL "Linux")
  message(STATUS "Target system is Linux")
  set(OS_LINUX TRUE)
  set(DOTNET_RID_OS "linux")
endif()

if(OS_WINDOWS)
  # #############################################################################
  # Windows specific settings
  set(OS_WINDOWS 1)
  set(STATIC_CRT ON CACHE BOOL "Use MultiThreaded linkage for MSVC" FORCE)

  add_definitions(
    -DOS_WINDOWS=1
    -D_WIN32_WINNT=0x602
  )

  if(USE_WIN_DCOMPOSITION)
    add_definitions(
      -DENABLE_WINDOWS_DIRECT_COMPOSITION=1
    )
  endif()

  add_compile_options(
    /W3
    /WX
    /wd4099
    /MT$<$<CONFIG:Debug>:d>
  )
  add_link_options(/DEBUG)

elseif(OS_MACOS)
  # #############################################################################
  # macOS specific settings
  set(OS_MACOS 1)
  set(OS_POSIX 1)
  add_definitions(-DOS_MACOS=1 -DOS_POSIX=1)
  add_compile_options(
    "-g"
    "-fdeclspec"
    "-Wno-overriding-option"
    "-Wno-overriding-t-option"
    "-Wno-unknown-warning-option"
    "$<$<CONFIG:DEBUG>:-O0>"
    "$<$<CONFIG:RELEASE>:-O3>"
  )

  # Target architecture.
  if(PROJECT_ARCH STREQUAL "x86_64")
    set(CMAKE_OSX_ARCHITECTURES "x86_64")
  elseif(PROJECT_ARCH STREQUAL "arm64")
    set(CMAKE_OSX_ARCHITECTURES "arm64")
  else()
    set(CMAKE_OSX_ARCHITECTURES "i386")
  endif()
elseif(OS_LINUX)
  # #############################################################################
  # Linux specific settings
  set(OS_LINUX 1)
  set(OS_POSIX 1)
  add_definitions(-DOS_LINUX=1 -DOS_POSIX=1)
  add_compile_options(
    "-g"
    "$<$<CONFIG:DEBUG>:-O0>"
    "$<$<CONFIG:RELEASE>:-O3>"
  )

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
else()
  message(FATAL_ERROR "Unsupported OS: ${CMAKE_SYSTEM_NAME}")
endif()

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

if (PROJECT_ARCH STREQUAL "x86_64")
  set(DOTNET_RID_ARCH "x64")
else()
  set(DOTNET_RID_ARCH "${PROJECT_ARCH}")
endif()
set(DOTNET_RID "${DOTNET_RID_OS}-${DOTNET_RID_ARCH}")

set(OUTPUT_BASE_DIR ${CMAKE_CURRENT_SOURCE_DIR}/output)

set(CCefView_LIB_TYPE SHARED)

# ################################################################
# Third party projects 
add_subdirectory(thirdparty/json)
add_subdirectory(thirdparty/cefviewcore)

if(OS_MACOS)
  # detect minimum deployment target by CEF
  # please refer to: https://bitbucket.org/chromiumembedded/cef/wiki/BranchesAndBuilding.md
  if(${CEF_VERSION_MAJOR} VERSION_GREATER_EQUAL 138)
    set(CMAKE_OSX_DEPLOYMENT_TARGET 12.0)
  elseif(${CEF_VERSION_MAJOR} VERSION_GREATER_EQUAL 117)
    set(CMAKE_OSX_DEPLOYMENT_TARGET 10.15)
  elseif(${CEF_VERSION_MAJOR} VERSION_GREATER_EQUAL 104)
    set(CMAKE_OSX_DEPLOYMENT_TARGET 10.13)
  else()
    set(CMAKE_OSX_DEPLOYMENT_TARGET 10.11)
  endif()
endif()

# ################################################################
# Native projects
add_subdirectory(src/CCefView)

# ################################################################
# Generate C and C# binding source files
if((DEFINED UPDATE_BINDING_SOURCE) AND UPDATE_BINDING_SOURCE)
    message(STATUS "Generating C and C# binding source files")
    execute_process(
        COMMAND python scripts/gen.py --cef-include-path "${CefViewCore_CEF_INCLUDE_DIR}/.." --c-gen-out-dir "src/CCefView/capi" --cs-gen-out-dir "${OUTPUT_BASE_DIR}/compile"
        WORKING_DIRECTORY ${CMAKE_SOURCE_DIR}
        RESULT_VARIABLE res
        OUTPUT_VARIABLE out
        OUTPUT_FILE ${CMAKE_BINARY_DIR}/code-gen.log
        ERROR_FILE ${CMAKE_BINARY_DIR}/code-gen.log
    )
else()
    message(STATUS "Skipping generating C and C# binding source files")
endif()

