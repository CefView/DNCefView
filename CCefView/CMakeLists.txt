cmake_minimum_required(VERSION 3.20)
project(CCefView LANGUAGES CXX)


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

set(OUTPUT_BASE_DIR ${CMAKE_CURRENT_SOURCE_DIR}/output)

# Only generate Debug and Release configuration types.
set(CMAKE_CONFIGURATION_TYPES Debug Release)

if(NOT CMAKE_BUILD_TYPE)
  set(CMAKE_BUILD_TYPE "Debug")
endif()

# Use folders in the resulting project files.
set_property(GLOBAL PROPERTY USE_FOLDERS ON)
set_property(GLOBAL PROPERTY PREDEFINED_TARGETS_FOLDER "~CMakeTargets")

# ################################################################
# Third party projects 
add_subdirectory(thirdparty/json)
add_subdirectory(thirdparty/cefviewcore)

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

