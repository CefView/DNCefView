include(FetchContent)

if(NOT CEFVIEW_CORE_VERSION)
    set(DEFAULT_CEFVIEW_CORE_VERSION "main")
    set(CEFVIEW_CORE_VERSION ${DEFAULT_CEFVIEW_CORE_VERSION})
    message(STATUS "CEFVIEW_CORE_VERSION not set, use default value:${CEFVIEW_CORE_VERSION}")
endif()

# fetch CefViewCore project
FetchContent_Declare(
    CefViewCore
    GIT_REPOSITORY https://github.com/CefView/CefViewCore.git
    GIT_TAG ${CEFVIEW_CORE_VERSION}
)

# set folder
set(CMAKE_FOLDER ${CMAKE_FOLDER}/CefViewCore)

# set CEF SDK version
set(CEF_SDK_VERSION "126.2.18+g3647d39+chromium-126.0.6478.183")

# populate CefViewCore project
FetchContent_MakeAvailable(CefViewCore)

configure_file(
    "${CefViewCore_SOURCE_DIR}/include/CefVersion.h"
    "${CMAKE_SOURCE_DIR}/src/CCefView/include/CefVersion.h"
    COPYONLY
)

if((DEFINED UPDATE_BINDING_SOURCE) AND UPDATE_BINDING_SOURCE)
    message(STATUS "Generating C and C# binding source files")
    execute_process(
        COMMAND python scripts/gen.py "${CefViewCore_CEF_INCLUDE_DIR}/.."
        WORKING_DIRECTORY ${CMAKE_SOURCE_DIR}
        RESULT_VARIABLE res
        OUTPUT_VARIABLE out
        OUTPUT_FILE ${CMAKE_BINARY_DIR}/code-gen.log
        ERROR_FILE ${CMAKE_BINARY_DIR}/code-gen.log
    )
else()
    message(STATUS "Skipping generating C and C# binding source files")
endif()
