# ## CCefView
# ###############################################################################################
project(CCefView)

include_directories(
  ${CMAKE_CURRENT_LIST_DIR}
)

file(GLOB_RECURSE CCefView_INCLUDE_FILES
  "${CMAKE_CURRENT_SOURCE_DIR}/../include/*.h"
)
source_group(
  TREE "${CMAKE_CURRENT_SOURCE_DIR}/../include"
  PREFIX Include
  FILES ${CCefView_INCLUDE_FILES}
)

file(GLOB CCefView_SRC_FILES
  "${CMAKE_CURRENT_SOURCE_DIR}/*.h"
  "${CMAKE_CURRENT_SOURCE_DIR}/*.cpp"
)
source_group(
  TREE ${CMAKE_CURRENT_SOURCE_DIR}
  PREFIX Source
  FILES ${CCefView_SRC_FILES}
)

file(GLOB_RECURSE CCefView_details_SRC_FILES
  "${CMAKE_CURRENT_SOURCE_DIR}/details/*.h"
  "${CMAKE_CURRENT_SOURCE_DIR}/details/*.cpp"
)
source_group(
  TREE ${CMAKE_CURRENT_SOURCE_DIR}
  PREFIX Source
  FILES ${CCefView_details_SRC_FILES}
)

file(GLOB CCefView_capi_SRC_FILES
  "${CMAKE_CURRENT_SOURCE_DIR}/capi/*.h"
  "${CMAKE_CURRENT_SOURCE_DIR}/capi/*.cpp"
)
source_group(
  TREE ${CMAKE_CURRENT_SOURCE_DIR}
  FILES ${CCefView_capi_SRC_FILES}
)

if(OS_WINDOWS)
  file(GLOB_RECURSE CCefView_Windows_SRC_FILES
    "${CMAKE_CURRENT_SOURCE_DIR}/win/*.h"
    "${CMAKE_CURRENT_SOURCE_DIR}/win/*.cpp"
  )
  source_group(
    TREE "${CMAKE_CURRENT_SOURCE_DIR}/win"
    PREFIX Source
    FILES ${CCefView_Windows_SRC_FILES}
  )

  add_library(CCefView ${CCefView_LIB_TYPE}
    ${CCefView_INCLUDE_FILES}
    ${CCefView_SRC_FILES}
    ${CCefView_details_SRC_FILES}
    ${CCefView_Windows_SRC_FILES}
    ${CCefView_capi_SRC_FILES}
  )

  set_target_properties(CCefView PROPERTIES
    FOLDER DNCef
  )

  target_compile_definitions(CCefView PRIVATE
    UNICODE
    _UNICODE
    CCEFVIEW_LIB
  )

  add_custom_command(TARGET CCefView PRE_BUILD

    # copy binary files of CefViewCore
    COMMAND ${CMAKE_COMMAND}
    -E copy_directory
    "$<TARGET_FILE_DIR:CefViewWing>"
    "$<TARGET_FILE_DIR:CCefView>"

    # gnerate a file contains timestamp
    COMMAND ${CMAKE_COMMAND}
    -E echo "/* Auto Build Trigger */" > "${CMAKE_BINARY_DIR}/auto_rebuild.cpp"
  )
endif() # OS_WINDOWS

if(OS_LINUX)
  file(GLOB_RECURSE CCefView_Linux_SRC_FILES
    "${CMAKE_CURRENT_SOURCE_DIR}/linux/*.h"
    "${CMAKE_CURRENT_SOURCE_DIR}/linux/*.cpp"
  )
  source_group(
    TREE "${CMAKE_CURRENT_SOURCE_DIR}/linux"
    PREFIX Source
    FILES ${CCefView_Linux_SRC_FILES}
  )

  add_library(CCefView ${CCefView_LIB_TYPE}
    ${CCefView_INCLUDE_FILES}
    ${CCefView_SRC_FILES}
    ${CCefView_details_SRC_FILES}
    ${CCefView_Linux_SRC_FILES}
    ${CCefView_capi_SRC_FILES}
  )

  target_compile_definitions(CCefView PRIVATE
    CCEFVIEW_LIB
  )

  set_target_properties(CCefView PROPERTIES
    FOLDER DNCef
    INSTALL_RPATH "$ORIGIN"
    BUILD_WITH_INSTALL_RPATH TRUE
  )

  add_custom_command(TARGET CCefView
    PRE_BUILD

    # copy binary files of CefViewCore
    COMMAND ${CMAKE_COMMAND}
    -E copy_directory
    "$<TARGET_FILE_DIR:CefViewWing>"
    "$<TARGET_FILE_DIR:CCefView>"
  )
endif() # OS_LINUX

if(OS_MACOS)
  file(GLOB_RECURSE CCefView_INCLUDE_HEADERS
    "${CMAKE_CURRENT_SOURCE_DIR}/../include/*.h"
  )
  source_group(
    TREE "${CMAKE_CURRENT_SOURCE_DIR}/../include"
    PREFIX Include
    FILES ${CCefView_INCLUDE_HEADERS}
  )

  file(GLOB_RECURSE CCefView_macOS_SRC_FILES
    "${CMAKE_CURRENT_SOURCE_DIR}/mac/*.h"
    "${CMAKE_CURRENT_SOURCE_DIR}/mac/*.cpp"
    "${CMAKE_CURRENT_SOURCE_DIR}/mac/*.mm"
  )
  source_group(
    TREE "${CMAKE_CURRENT_SOURCE_DIR}/mac"
    PREFIX Source
    FILES ${CCefView_macOS_SRC_FILES}
  )
  set(CCefView_INFO_PLIST_FILE "${CMAKE_CURRENT_LIST_DIR}/mac/Info.plist")

  add_library(CCefView ${CCefView_LIB_TYPE}
    ${CCefView_INCLUDE_FILES}
    ${CCefView_SRC_FILES}
    ${CCefView_details_SRC_FILES}
    ${CCefView_macOS_SRC_FILES}
    ${CCefView_INFO_PLIST_FILE}
    ${CCefView_capi_SRC_FILES}
  )

  set_target_properties(CCefView PROPERTIES
    FOLDER DNCef
    FRAMEWORK TRUE

    # compiler settings
    COMPILE_FLAGS "-fobjc-arc"
    CLANG_ENABLE_OBJC_ARC "YES"

    # xcode settings
    PUBLIC_HEADER "${CCefView_INCLUDE_HEADERS}"
    MACOSX_FRAMEWORK_INFO_PLIST "${CCefView_INFO_PLIST_FILE}"
    XCODE_ATTRIBUTE_PRODUCT_BUNDLE_IDENTIFIER "com.cefview.CCefView"
    XCODE_ATTRIBUTE_CLANG_CXX_LANGUAGE_STANDARD "gnu++11" # -std=gnu++11
    XCODE_ATTRIBUTE_CLANG_LINK_OBJC_RUNTIME "NO" # -fno-objc-link-runtime
    XCODE_ATTRIBUTE_COPY_PHASE_STRIP "NO"
    XCODE_ATTRIBUTE_DEAD_CODE_STRIPPING[variant=Release] "YES" # -Wl,-dead_strip
    XCODE_ATTRIBUTE_GCC_C_LANGUAGE_STANDARD "c99" # -std=c99
    XCODE_ATTRIBUTE_LD_RUNPATH_SEARCH_PATHS "@executable_path/../Frameworks @loader_path/Frameworks"
  )

  add_custom_command(TARGET CCefView
    POST_BUILD

    # create plugins directory
    COMMAND mkdir -p
    "$<TARGET_BUNDLE_DIR:CCefView>/PlugIns/"

    # copy the CefViewCore binaries to resource directory
    COMMAND cp -a
    "$<TARGET_BUNDLE_DIR:CefViewWing>/../"
    "$<TARGET_BUNDLE_DIR:CCefView>/PlugIns/"

    # sign the cef framework
    COMMAND codesign
    --force
    --sign -
    "$<TARGET_BUNDLE_DIR:CCefView>/PlugIns/Chromium Embedded Framework.framework"
  )
endif() # OS_MACOS

set_property(TARGET CCefView
  PROPERTY RUNTIME_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/output"
)

if(USE_OSR)
  target_compile_definitions(CCefView PRIVATE
    -DCEF_USE_OSR
  )
endif()

target_include_directories(CCefView PRIVATE
  "${CMAKE_CURRENT_SOURCE_DIR}/../include"
  ${CefViewCore_EXPORT_INCLUDE_PATH}
)

add_dependencies(CCefView
  CefViewCore
)

target_link_libraries(CCefView PRIVATE
  CefViewCore
)

# install target files
install(FILES "$<TARGET_FILE:CCefView>" DESTINATION "$<CONFIG>/runtimes/${DOTNET_RID_OS}-${DOTNET_RID_ARCH}/native")
