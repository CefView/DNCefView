# Setup project.
project(DNCefView LANGUAGES CSharp)

file(GLOB_RECURSE _SRC_FILES
    "${CMAKE_CURRENT_SOURCE_DIR}/*.cs"
)

# Add shared library project.
add_library(${PROJECT_NAME} SHARED
    ${_SRC_FILES}
)

# Set CLR assembly properties.
set_target_properties(${PROJECT_NAME}
    PROPERTIES
    VS_GLOBAL_ROOTNAMESPACE "DNCefView"
)

target_link_libraries(${PROJECT_NAME}
    PRIVATE
    CCefView
)

configure_file(
    "${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.targets"
    "${CMAKE_CURRENT_BINARY_DIR}/Directory.Build.targets"
    COPYONLY
)