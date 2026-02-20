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

set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/$<CONFIG>/runtimes/${DOTNET_RID}/native/)
set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/$<CONFIG>/)
set(CMAKE_ARCHIVE_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/$<CONFIG>/)

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

  # detect minimum deployment target by CEF
  # plese refer to: https://bitbucket.org/chromiumembedded/cef/wiki/BranchesAndBuilding.md
  if(${CEF_VERSION_MAJOR} VERSION_GREATER_EQUAL 138)
    set(CMAKE_OSX_DEPLOYMENT_TARGET 12.0)
  elseif(${CEF_VERSION_MAJOR} VERSION_GREATER_EQUAL 117)
    set(CMAKE_OSX_DEPLOYMENT_TARGET 10.15)
  elseif(${CEF_VERSION_MAJOR} VERSION_GREATER_EQUAL 104)
    set(CMAKE_OSX_DEPLOYMENT_TARGET 10.13)
  else()
    set(CMAKE_OSX_DEPLOYMENT_TARGET 10.11)
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

set(CCefView_LIB_TYPE SHARED)

add_subdirectory(thirdparty/json)
add_subdirectory(thirdparty/cefviewcore)

# ## CCefView
# ###############################################################################################
set(PROJECT_NAME CCefView)

file(GLOB CCefView_capi_SRC_FILES
  "${CMAKE_CURRENT_SOURCE_DIR}/capi/*.h"
  "${CMAKE_CURRENT_SOURCE_DIR}/capi/*.cpp"
)
source_group(
  TREE ${CMAKE_CURRENT_SOURCE_DIR}
  FILES ${CCefView_capi_SRC_FILES}
)

file(GLOB CCefView_INCLUDE_FILES
  "${CMAKE_CURRENT_SOURCE_DIR}/include/*.h"
)
source_group(
  TREE ${CMAKE_CURRENT_SOURCE_DIR}/include
  PREFIX include
  FILES ${CCefView_INCLUDE_FILES}
)

file(GLOB CCefView_SRC_FILES
  "${CMAKE_CURRENT_SOURCE_DIR}/source/*.h"
  "${CMAKE_CURRENT_SOURCE_DIR}/source/*.cpp"
)
source_group(
  TREE ${CMAKE_CURRENT_SOURCE_DIR}/source
  PREFIX source
  FILES ${CCefView_SRC_FILES}
)

file(GLOB_RECURSE CCefView_details_SRC_FILES
  "${CMAKE_CURRENT_SOURCE_DIR}/source/details/*.h"
  "${CMAKE_CURRENT_SOURCE_DIR}/source/details/*.cpp"
)
source_group(
  TREE ${CMAKE_CURRENT_SOURCE_DIR}/source/details
  PREFIX source/details
  FILES ${CCefView_details_SRC_FILES}
)

if(USE_SANDBOX AND(OS_WINDOWS OR OS_MACOS))
  add_definitions(-DCEF_USE_SANDBOX)
endif()

if(OS_WINDOWS)
  file(GLOB_RECURSE CCefView_Windows_SRC_FILES
    "${CMAKE_CURRENT_SOURCE_DIR}/source/win/*.h"
    "${CMAKE_CURRENT_SOURCE_DIR}/source/win/*.cpp"
  )
  source_group(
    TREE "${CMAKE_CURRENT_SOURCE_DIR}/source/win"
    PREFIX Source
    FILES ${CCefView_Windows_SRC_FILES}
  )

  add_library(${PROJECT_NAME} SHARED
    ${CCefView_INCLUDE_FILES}
    ${CCefView_SRC_FILES}
    ${CCefView_capi_SRC_FILES}
    ${CCefView_details_SRC_FILES}
    ${CCefView_Windows_SRC_FILES}
  )

  target_compile_definitions(${PROJECT_NAME} PRIVATE
    UNICODE
    _UNICODE
  )

  target_link_options(${PROJECT_NAME}
    PRIVATE
    "/DELAYLOAD:libcef.dll"
  )

  target_link_libraries(${PROJECT_NAME}
    PRIVATE
    d3d11
    d3dcompiler
    dcomp
    delayimp
  )

  add_custom_command(TARGET ${PROJECT_NAME}
    PRE_BUILD

    # copy binary files of CefViewCore
    COMMAND ${CMAKE_COMMAND}
    -E copy_directory
    "$<TARGET_FILE_DIR:${CEFVIEW_WING_NAME}>"
    "$<TARGET_FILE_DIR:${PROJECT_NAME}>/CefView"

    # gnerate a file contains timestamp
    COMMAND ${CMAKE_COMMAND}
    -E echo "/* Auto Build Trigger */" > "${CMAKE_BINARY_DIR}/auto_rebuild.cpp"
  )
endif() # OS_WINDOWS

if(OS_LINUX)
  file(GLOB_RECURSE CCefView_Linux_SRC_FILES
    "${CMAKE_CURRENT_SOURCE_DIR}/source/linux/*.h"
    "${CMAKE_CURRENT_SOURCE_DIR}/source/linux/*.cpp"
  )
  source_group(
    TREE "${CMAKE_CURRENT_SOURCE_DIR}/source/linux"
    PREFIX Source
    FILES ${CCefView_Linux_SRC_FILES}
  )

  add_library(${PROJECT_NAME} SHARED
    ${CCefView_INCLUDE_FILES}
    ${CCefView_SRC_FILES}
    ${CCefView_capi_SRC_FILES}
    ${CCefView_details_SRC_FILES}
    ${CCefView_Linux_SRC_FILES}
  )

  set_target_properties(${PROJECT_NAME}
    PROPERTIES
    INSTALL_RPATH "$ORIGIN"
    BUILD_WITH_INSTALL_RPATH TRUE
  )

  add_custom_command(TARGET ${PROJECT_NAME}
    PRE_BUILD

    # copy binary files of CefViewCore
    COMMAND ${CMAKE_COMMAND}
    -E copy_directory
    "$<TARGET_FILE_DIR:${CEFVIEW_WING_NAME}>"
    "$<TARGET_FILE_DIR:${PROJECT_NAME}>"
  )
endif() # OS_LINUX

if(OS_MACOS)
  file(GLOB_RECURSE CCefView_macOS_SRC_FILES
    "${CMAKE_CURRENT_SOURCE_DIR}/source/mac/*.h"
    "${CMAKE_CURRENT_SOURCE_DIR}/source/mac/*.cpp"
    "${CMAKE_CURRENT_SOURCE_DIR}/source/mac/*.mm"
  )
  source_group(
    TREE "${CMAKE_CURRENT_SOURCE_DIR}/source/mac"
    PREFIX Source
    FILES ${CCefView_macOS_SRC_FILES}
  )
  set(CCefView_INFO_PLIST_FILE "${CMAKE_CURRENT_LIST_DIR}/source/mac/Info.plist")

  add_library(${PROJECT_NAME} SHARED
    ${CCefView_INCLUDE_FILES}
    ${CCefView_SRC_FILES}
    ${CCefView_capi_SRC_FILES}
    ${CCefView_details_SRC_FILES}
    ${CCefView_macOS_SRC_FILES}
    ${CCefView_INFO_PLIST_FILE}
  )

  find_library(METAL_FRAMEWORK Metal)
  find_library(QUARTZCORE_FRAMEWORK QuartzCore)

  target_link_libraries(${PROJECT_NAME}
    PRIVATE
    ${METAL_FRAMEWORK}
    ${QUARTZCORE_FRAMEWORK}
  )

  set_target_properties(${PROJECT_NAME}
    PROPERTIES
    FRAMEWORK TRUE

    # compiler settings
    COMPILE_FLAGS "-fobjc-arc"
    CLANG_ENABLE_OBJC_ARC "YES"

    # xcode settings
    PUBLIC_HEADER "${CCefView_INCLUDE_HEADERS}"
    MACOSX_FRAMEWORK_INFO_PLIST "${CCefView_INFO_PLIST_FILE}"
    XCODE_ATTRIBUTE_PRODUCT_BUNDLE_IDENTIFIER "com.cefview.ccefview"
    XCODE_ATTRIBUTE_CLANG_CXX_LANGUAGE_STANDARD "gnu++11" # -std=gnu++11
    XCODE_ATTRIBUTE_CLANG_LINK_OBJC_RUNTIME "NO" # -fno-objc-link-runtime
    XCODE_ATTRIBUTE_COPY_PHASE_STRIP "NO"
    XCODE_ATTRIBUTE_DEAD_CODE_STRIPPING[variant=Release] "YES" # -Wl,-dead_strip
    XCODE_ATTRIBUTE_GCC_C_LANGUAGE_STANDARD "c99" # -std=c99
    XCODE_ATTRIBUTE_LD_RUNPATH_SEARCH_PATHS "@executable_path/../Frameworks @loader_path/Frameworks"
  )

  add_custom_command(TARGET ${PROJECT_NAME}
    POST_BUILD

    # create plugins directory
    COMMAND mkdir -p
    "$<TARGET_BUNDLE_DIR:${PROJECT_NAME}>/Resources/CefView/"

    # copy the CefViewCore binaries to resource directory
    COMMAND cp -a
    "$<TARGET_BUNDLE_DIR:CefViewWing>/../"
    "$<TARGET_BUNDLE_DIR:${PROJECT_NAME}>/Resources/CefView/"

    # adjust file permission
    COMMAND
    cd "$<TARGET_BUNDLE_DIR:${PROJECT_NAME}>/Resources/CefView/Chromium Embedded Framework.framework/Resources" && chmod +rw *.bin

    # sign the cef framework
    COMMAND codesign
    --force
    --sign -
    "$<TARGET_BUNDLE_DIR:${PROJECT_NAME}>/Resources/CefView/Chromium Embedded Framework.framework"
  )
endif() # OS_MACOS

add_dependencies(${PROJECT_NAME}
  CefViewCore::CefViewCore
)

target_compile_definitions(${PROJECT_NAME}
  PRIVATE NOMINMAX)

target_include_directories(${PROJECT_NAME}
  PRIVATE
  ${CMAKE_CURRENT_SOURCE_DIR}/include
  ${CMAKE_CURRENT_SOURCE_DIR}/source
)

target_link_libraries(${PROJECT_NAME}
  PRIVATE
  nlohmann_json::nlohmann_json
  CefViewCore::CefViewCore
)

# set_target_properties(${PROJECT_NAME}
# PROPERTIES
# ARCHIVE_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/output/lib
# OUTPUT_NAME runtimes/${DOTNET_RID}/native/${OUTPUT_NAME}
# )
