
@rem config cmake
cmake -S . ^
-B .build/windows.x86_64 ^
-A x64 ^
-DPROJECT_ARCH=x86_64 ^
-DCMAKE_INSTALL_PREFIX:PATH="%cd%/out"