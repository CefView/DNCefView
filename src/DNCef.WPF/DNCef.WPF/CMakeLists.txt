# Setup project.
project(DNCef.WPF LANGUAGES CSharp)

# Setup project to use C# utilities.
include(CSharpUtilities)

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
    FOLDER DNCef.WPF
    DOTNET_SDK "Microsoft.NET.Sdk"
    DOTNET_TARGET_FRAMEWORK "net6.0-windows"
    VS_GLOBAL_ROOTNAMESPACE "DNCef"
    VS_GLOBAL_UseWPF "true"
    VS_GLOBAL_Authors "tishion"
    VS_GLOBAL_PackageProjectUrl "https://github.com/CefView/DNCefView"
    VS_GLOBAL_Copyright "Copyright © 2022 TISHION"
    VS_GLOBAL_Description "An encapsulation of CefView for .NET"
    VS_GLOBAL_RepositoryUrl "https://github.com/CefView/DNCefView"
)

add_dependencies(${PROJECT_NAME} DNCef)