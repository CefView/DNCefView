#!/bin/bash

# config cmake
BUILD_DIR="$(pwd)/.build/macos.arm64"

echo ============== Config project ==============
cmake -G "Xcode" \
    -S . \
    -B "${BUILD_DIR}" \
    -DPROJECT_ARCH=arm64 \
    -DUSE_SANDBOX=ON \
    -DCMAKE_INSTALL_PREFIX:PATH="$(pwd)/out" \
    $*