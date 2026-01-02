project(DNCefView.WPF.Demo LANGUAGES CSharp)

# Include CSharpUtilities for specific C# properties and XAML handling
include(CSharpUtilities)

# Define the executable target
add_executable(${PROJECT_NAME} WIN32
    App.xaml
    App.xaml.cs
    AssemblyInfo.cs
    MainWindow.xaml
    MainWindow.xaml.cs
    webres/index.html
    webres/tutorial.html
)

csharp_set_xaml_cs_properties(
    App.xaml
    App.xaml.cs
    AssemblyInfo.cs
    MainWindow.xaml
    MainWindow.xaml.cs
)

set_source_files_properties(App.xaml
    PROPERTIES
    VS_XAML_TYPE "ApplicationDefinition"
)

set_source_files_properties(MainWindow.xaml
    PROPERTIES
    VS_XAML_TYPE "Page"
)

set_source_files_properties(
    webres/index.html
    webres/tutorial.html
    PROPERTIES
    VS_COPY_TO_OUT_DIR PreserveNewest
)

target_link_libraries(${PROJECT_NAME}
    PRIVATE
    DNCefView.WPF
)

set_target_properties(${PROJECT_NAME}
    PROPERTIES
    DOTNET_TARGET_FRAMEWORK "net8.0-windows"
    VS_GLOBAL_ROOTNAMESPACE "DNCefView.WPF.Demo"
    VS_GLOBAL_UseWPF "true"
    VS_GLOBAL_Nullable "enable"
    VS_GLOBAL_OutputType "WinExe"
    VS_FRAMEWORK_REFERENCES "Microsoft.WindowsDesktop.App.WPF"
)