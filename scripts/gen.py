#!/usr/bin/env python

import os
import sys

from pp2xlang.translator import Translator
from pp2xlang.translator import TypeMapper
from pp2xlang.generator.c import CGenerator
from pp2xlang.generator.csharp import CSharpGenerator

import clang.cindex
import clang.native

clang.cindex.Config.set_library_path(os.path.dirname(clang.native.__file__))


class CTypeMapper(TypeMapper):
    def __init__(self) -> None:
        super().__init__()
        self.table = {
            "std::string &": ("char *", ".c_str()"),
            "const std::string &": ("const char *", ".c_str()"),
            "std::string": ("const char *", ""),
            "const std::string": ("const char *", ""),
        }

    def map_type(self, type):
        return self.table.get(type, (type, ""))[0]

    def map_conversion(self, type):
        return self.table.get(type, (type, ""))[1]


class CSharpTypeMapper(TypeMapper):
    def __init__(self) -> None:
        super().__init__()
        self.table = {
            "void *": ("IntPtr", ""),
            "char *": ("string", ""),
            "unsigned char *": ("byte[]", ""),
            "unsigned long long": ("ulong", ""),
            "std::string": ("string", ""),
            "unsigned int": ("uint", ""),
            "int16_t": ("Int16", ""),
            "uint16_t": ("UInt16", ""),
            "int32_t": ("Int32", ""),
            "uint32_t": ("UInt32", ""),
            "int64_t": ("Int64", ""),
            "uint64_t": ("UInt64", ""),
            "char *": ("string", ""),
        }

    def map_type(self, type):
        return self.table.get(type, (type, ""))[0]

    def map_conversion(self, type):
        return self.table.get(type, (type, ""))[1]


if __name__ == "__main__":
    import argparse
    from pathlib import Path
    parser = argparse.ArgumentParser(description="Generate candidates without overwriting hardened interop snapshots")
    parser.add_argument("cef_include_path")
    parser.add_argument("--output-root", default=".generated-bindings")
    options = parser.parse_args()
    cef_include_path = options.cef_include_path
    output = Path(options.output_root).resolve()
    source = Path("src").resolve()
    if output == source or source in output.parents:
        parser.error("Generate outside src; review and merge candidates with the maintained lifetime/ABI contracts")
    output.mkdir(parents=True, exist_ok=True)

    # configuration
    args = [
        "-x",
        "c++",
        "-std=c++17",
        f"-I{cef_include_path}",
    ]
    translator = Translator(args)

    cgen = CGenerator(str(output / "capi"), CTypeMapper())
    translator.add_generator(cgen)

    csharpgen = CSharpGenerator(
        str(output / "AutoGen"), "DNCefView", "CCefView", CSharpTypeMapper()
    )
    translator.add_generator(csharpgen)

    # parse
    translator.parse("src/CCefView/include/CefVersion.h")
    translator.parse("src/CCefView/include/CefGlobal.h")
    translator.parse("src/CCefView/include/CefTypes.h")
    translator.parse("src/CCefView/include/CefConfig.h")
    translator.parse("src/CCefView/include/CefContext.h")
    translator.parse("src/CCefView/include/CefQuery.h")
    translator.parse("src/CCefView/include/CefSetting.h")
    translator.parse("src/CCefView/include/CefBrowser.h")
    translator.parse("src/CCefView/include/CefBrowserCallback.h")

    # translate
    translator.translate()
