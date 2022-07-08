#!/usr/bin/env python

import os

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
            'std::string &': ('char *', '.c_str()'),
            'const std::string &': ('const char *', '.c_str()'),
            'std::string': ('const char *', ''),
            'const std::string': ('const char *', ''),
        }

    def map_type(self, type):
        return self.table.get(type, (type, ''))[0]

    def map_conversion(self, type):
        return self.table.get(type, (type, ''))[1]


class CSharpTypeMapper(TypeMapper):
    def __init__(self) -> None:
        super().__init__()
        self.table = {
            'void *': ('IntPtr', ''),
            'std::string': ('string', ''),
            'int': ('Int32', ''),
            'int16_t': ('Int16', ''),
            'uint16_t': ('UInt16', ''),
            'int32_t': ('Int32', ''),
            'uint32_t': ('UInt32', ''),
            'int64_t': ('Int64', ''),
            'uint64_t': ('UInt64', ''),
        }

    def map_type(self, type):
        return self.table.get(type, (type, ''))[0]

    def map_conversion(self, type):
        return self.table.get(type, (type, ''))[1]


if __name__ == '__main__':
    # configuration
    args = ['-x', 'c++', '-std=c++17',
            '-ICefViewCore/dep/cef_binary_95.7.12+g99c4ac0+chromium-95.0.4638.54_windows64']
    translator = Translator(args)

    cgen = CGenerator('src/CCefView/capi', CTypeMapper())
    translator.add_generator(cgen)

    csharpgen = CSharpGenerator(
        'src/DNCef/AutoGen', 'DNCef', 'CCefView', CSharpTypeMapper())
    translator.add_generator(csharpgen)

    # parse
    translator.parse('src/CCefView/CefConfig.h')
    translator.parse('src/CCefView/CefContext.h')
    translator.parse('src/CCefView/CefQuery.h')
    translator.parse('src/CCefView/CefSetting.h')
    translator.parse('src/CCefView/CefTypes.h')
    translator.parse('src/CCefView/CefBrowser.h')
    translator.parse('src/CCefView/CefBrowserCallback.h')

    # translate
    translator.translate()
