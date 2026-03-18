
@rem config cmake
cmake -S . ^
-B .build/windows.arm64 ^
-A ARM64 ^
-DPROJECT_ARCH=arm64 ^
-DCMAKE_INSTALL_PREFIX:PATH="%cd%/out" ^
%*