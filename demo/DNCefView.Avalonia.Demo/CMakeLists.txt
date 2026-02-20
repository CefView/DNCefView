project(DNCefView.Avalonia.Demo LANGUAGES CSharp)

# Include CSharpUtilities for specific C# properties and XAML handling
include(CSharpUtilities)

configure_file(
    "${CMAKE_CURRENT_SOURCE_DIR}/app.manifest"
    "${CMAKE_CURRENT_BINARY_DIR}/app.manifest"
    COPYONLY
)

file(GLOB_RECURSE _HTML_FILES
    webres/*.html
)

# Define the executable target
add_executable(${PROJECT_NAME} WIN32
    app.manifest
    App.axaml
    App.axaml.cs
    MainWindow.axaml
    MainWindow.axaml.cs
    Program.cs
    ${_HTML_FILES}
)

set_source_files_properties(
    "App.axaml"
    "MainWindow.axaml"
    PROPERTIES
    VS_TOOL_OVERRIDE "AvaloniaXAML"
)

set_source_files_properties(
    ${_HTML_FILES}
    PROPERTIES
    VS_COPY_TO_OUT_DIR PreserveNewest
)

target_link_libraries(${PROJECT_NAME}
    PRIVATE
    DNCefView.Avalonia
)

string(JOIN ";" NUGET_PACKAGES
    "Avalonia_*"
    "Avalonia.Desktop_*"
    "Avalonia.Fonts.Inter_*"
    "Avalonia.Labs.Controls_*"
    "Avalonia.Themes.Fluent_*"
)

set_target_properties(${PROJECT_NAME}
    PROPERTIES
    VS_PACKAGE_REFERENCES "${NUGET_PACKAGES}"
    VS_GLOBAL_ROOTNAMESPACE "DNCefView.Avalonia.Demo"
    VS_GLOBAL_Nullable "enable"
    VS_GLOBAL_ApplicationManifest "app.manifest"
    VS_GLOBAL_AvaloniaUseCompiledBindingsByDefault "true"
    VS_GLOBAL_GeneratePackageOnBuild false
)
