# Setup project.
set(PROJECT_NAME DNCefView.Avalonia)

file(GLOB_RECURSE _SRC_FILES
    "${CMAKE_CURRENT_SOURCE_DIR}/*.cs"
)

# Add shared library project.
add_library(${PROJECT_NAME} SHARED
    ${_SRC_FILES}
)

target_link_libraries(${PROJECT_NAME}
    PRIVATE
    DNCefView
)

string(JOIN ";" NUGET_PACKAGES
    "Avalonia_11.0.6"
)

# Set CLR assembly properties.
set_target_properties(${PROJECT_NAME}
    PROPERTIES
    VS_GLOBAL_ROOTNAMESPACE "${PROJECT_NAME}"

    VS_PACKAGE_REFERENCES "${NUGET_PACKAGES}"
    VS_GLOBAL_PackageId "${PROJECT_NAME}"
)
