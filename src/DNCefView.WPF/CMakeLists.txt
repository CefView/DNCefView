# Setup project.
set(PROJECT_NAME DNCefView.WPF)

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

# Set CLR assembly properties.
set_target_properties(${PROJECT_NAME}
    PROPERTIES
    DOTNET_TARGET_FRAMEWORK "net8.0-windows"
    VS_FRAMEWORK_REFERENCES "Microsoft.WindowsDesktop.App.WPF"
    VS_GLOBAL_ROOTNAMESPACE "${PROJECT_NAME}"
    VS_GLOBAL_Nullable "enable"
    VS_GLOBAL_UseWPF "true"

    VS_GLOBAL_PackageId "${PROJECT_NAME}"
)
