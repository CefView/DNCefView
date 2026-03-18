#!/bin/bash

# config cmake
BUILD_DIR="$(pwd)/.build/macos.x86_64"

echo ============== Config project ==============
cmake -G "Xcode" \
    -S . \
    -B "${BUILD_DIR}" \
    -DPROJECT_ARCH=x86_64 \
    -DUSE_SANDBOX=ON \
    -DCMAKE_INSTALL_PREFIX:PATH="$(pwd)/out" \
    $*