@rem OSR mode is enabled by default, add -DUSE_OSR=OFF to disable the OSR mode
cmake -S . ^
-B .build/windows.arm64 ^
-A ARM64 ^
-DPROJECT_ARCH=arm64 ^
-DCMAKE_INSTALL_PREFIX:PATH="%cd%/out"