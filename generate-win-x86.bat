
@rem config cmake
cmake -S . ^
-B .build/windows.x86 ^
-A Win32 ^
-DPROJECT_ARCH=x86 ^
-DCMAKE_INSTALL_PREFIX:PATH="%cd%/out"