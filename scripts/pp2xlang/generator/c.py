#!/usr/bin/env python

import os
import clang.cindex

from pp2xlang.translator import TypeMapper
from pp2xlang.translator import SourceGenerator


def parse_result_type(cursor: clang.cindex.Cursor, mapper):
    params = []
    args = []
    # collect data
    for m_cursor in cursor.get_children():
        match m_cursor.kind:
            case clang.cindex.CursorKind.PARM_DECL:
                param_type = m_cursor.type
                param_name = m_cursor.spelling

                # find the basic type
                for cur in m_cursor.get_children():
                    match cur.kind:
                        case clang.cindex.CursorKind.TYPE_REF:
                            print(f'{cur.spelling}')

                params.append(f'{mapper(param_type.spelling)[0]} {param_name}')
                args.append(param_name)

    return (params, args)


class CGenerator(SourceGenerator):
    def __init__(self, out: str, mapper: TypeMapper) -> None:
        super().__init__()
        self.types = {}

        self.out = out
        self.mapper = mapper

    ##########################################################################
    def translate_type(self, type):
        target_type = self.mapper.map_type(type)
        for k, v in self.types.items():
            target_type = target_type.replace(k, v)
        return target_type

    def get_included_files(self, tu: clang.cindex.TranslationUnit):
        includes = tu.get_includes()
        files = list(
            filter(lambda x: x.depth == 1 and x.include.name.startswith('src/'), includes))
        for f in files:
            print(f'{f.include.name}')
        return files

    def write_destructor(self, cpp_class, thiz):
        declaration = (
            f'  CCEFVIEW_EXPORT void {cpp_class}_Delete({thiz} * thiz);\n'
        )
        self.header_file.write(declaration)
        print(declaration, end='')

        definition = (
            f'void {cpp_class}_Delete({thiz} * thiz) {{\n'
            f'  return delete thiz;\n'
            '}\n\n'
        )
        self.impl_file.write(definition)
        print(definition, end='')

    def parse_param_list(self, cursor: clang.cindex.Cursor):
        params = []
        args = []
        # collect data
        for m_cursor in cursor.get_children():
            match m_cursor.kind:
                case clang.cindex.CursorKind.PARM_DECL:
                    param_type = m_cursor.type
                    param_name = m_cursor.spelling

                    # find the basic type
                    for cur in m_cursor.get_children():
                        match cur.kind:
                            case clang.cindex.CursorKind.TYPE_REF:
                                pass
                                # print(f'param_list: {cur.spelling}')

                    params.append(
                        f'{self.translate_type(param_type.spelling)} {param_name}')

                    if (param_type.get_canonical().kind == clang.cindex.TypeKind.ENUM):
                        args.append(
                            f'({param_type.get_canonical().spelling}){param_name}')
                    else:
                        args.append(param_name)

        return (params, args)

    def return_prefix(self, rt):
        match rt.get_canonical().kind:
            case clang.cindex.TypeKind.VOID:
                return ""
            case clang.cindex.TypeKind.ENUM:
                return f"return ({self.translate_type(rt.get_canonical().spelling)})"
            case _:
                return "return "

    ##########################################################################
    def parse(self, tu: clang.cindex.TranslationUnit):
        """"""
        for cursor in tu.cursor.get_children():
            if (cursor.location.file.name != tu.spelling):
                continue
            else:
                self.parse_cursor(cursor)

    def parse_cursor(self, cursor: clang.cindex.Cursor):
        match cursor.kind:
            case clang.cindex.CursorKind.TYPEDEF_DECL:
                c = cursor.underlying_typedef_type.get_canonical().get_declaration()
                if (c.kind == clang.cindex.CursorKind.ENUM_DECL):
                    print(f'  {cursor.spelling}, {c.kind}')
                    self.types[cursor.spelling] = f'{cursor.spelling.lower()}_enum'
                elif (c.kind == clang.cindex.CursorKind.STRUCT_DECL):
                    print(f'  {c.spelling}, {c.kind}')
                    self.types[c.spelling] = f'{cursor.spelling.lower()}_struct'
                    self.types[cursor.spelling] = f'{cursor.spelling.lower()}_struct'
                else:
                    self.parse_cursor(c)

            case clang.cindex.CursorKind.ENUM_DECL:
                print(f'  {cursor.spelling}, {cursor.kind}')
                self.types[cursor.spelling] = f'{cursor.spelling.lower()}_enum'
            case clang.cindex.CursorKind.STRUCT_DECL:
                print(f'  {cursor.spelling}, {cursor.kind}')
                self.types[cursor.spelling] = f'{cursor.spelling.lower()}_struct'
            case clang.cindex.CursorKind.CLASS_DECL:
                print(f'  {cursor.spelling}, {cursor.kind}')
                self.types[cursor.spelling] = f'{cursor.spelling.lower()}_class'

    def prepare(self, tu: clang.cindex.TranslationUnit):
        """"""
        self.source_name = os.path.splitext(os.path.basename(tu.spelling))[0]
        self.header_file = open(
            f'{os.path.join(self.out, self.source_name)}_c.h', 'wt')
        self.impl_file = open(
            f'{os.path.join(self.out, self.source_name)}_c.cpp', 'wt')

        include_list = ''
        inc_files = self.get_included_files(tu)
        for inc in inc_files:
            inc_name = os.path.splitext(os.path.basename(inc.include.name))[0]
            include_list += f'#include "{inc_name}_c.h"\n'

        # write file header
        header = (
            f'#ifndef {self.source_name}_H_\n'
            f'#define {self.source_name}_H_\n'
            '#pragma once\n'
            '\n'
            '#if defined _WIN32 || defined __CYGWIN__\n'
            '#ifdef __GNUC__\n'
            '#define CCEFVIEW_EXPORT __attribute__((dllexport))\n'
            '#else\n'
            '#define CCEFVIEW_EXPORT __declspec(dllexport)\n'
            '#endif\n'
            '#else\n'
            '#if __GNUC__ >= 4\n'
            '#define CCEFVIEW_EXPORT __attribute__((visibility("default")))\n'
            '#else\n'
            '#define CCEFVIEW_EXPORT\n'
            '#endif\n'
            '#endif\n'
            '\n'
            '#include <stdint.h>\n\n'
            f'{include_list}'
            '\n'
            '#if defined(__cplusplus)\n'
            'extern "C"\n'
            '{\n'
            '#endif\n'
            '\n'
        )
        self.header_file.write(header)

        header = (
            f'#include "{self.source_name}_c.h"\n'
            f'#include "{self.source_name}.h"\n\n'
        )
        self.impl_file.write(header)

    def finalize(self):
        """"""
        # write file footer
        footer = (
            '\n'
            '#if defined(__cplusplus)\n'
            '}\n'
            '#endif\n'
            '\n'
            f"\n#endif // {self.source_name}_H_"
        )
        self.header_file.write(footer)

        self.source_name = ''
        self.header_file.close()
        self.impl_file.close()

    def translate_cursor(self, cursor: clang.cindex.Cursor):
        """
        """
        match cursor.kind:
            case clang.cindex.CursorKind.TYPEDEF_DECL:
                print(
                    f'* Found using clause: {cursor.spelling}, {cursor.kind}')
                c = cursor.underlying_typedef_type.get_canonical().get_declaration()
                if (c.kind == clang.cindex.CursorKind.ENUM_DECL):
                    print(
                        f'* Found enum clause: {cursor.spelling}, {c.kind}')
                    self.translate_enum(cursor)
                elif (c.kind == clang.cindex.CursorKind.STRUCT_DECL):
                    print(f'  {c.spelling}, {c.kind}')
                    self.types[c.spelling] = f'{cursor.spelling.lower()}_struct'
                    print(
                        f'* Found struct clause: {c.spelling}, {c.kind}')
                    self.translate_struct(c, cursor.spelling)
                else:
                    self.parse_cursor(c)
                pass
            case clang.cindex.CursorKind.TYPEDEF_DECL:
                print(
                    f'* Found typedef clause: {cursor.spelling}, {cursor.kind}')
                pass
            case clang.cindex.CursorKind.ENUM_DECL:
                print(
                    f'* Found enum clause: {cursor.spelling}, {cursor.kind}')
                self.translate_enum(cursor)
                pass
            case clang.cindex.CursorKind.STRUCT_DECL:
                print(
                    f'* Found struct clause: {cursor.spelling}, {cursor.kind}')
                self.translate_struct(cursor, cursor.spelling)
                pass
            case clang.cindex.CursorKind.CLASS_DECL:
                print(
                    f'* Found class clause: {cursor.spelling}, {cursor.kind}')
                self.translate_class(cursor)
                pass

    def translate_class(self, cursor: clang.cindex.Cursor):
        """Write a class to the source"""
        cpp_class = cursor.spelling
        thiz = f'{cpp_class.lower()}_class'

        # render typedef statement
        typedef = f'  typedef struct {cpp_class} {thiz};\n'
        print(typedef)
        self.header_file.write(typedef)

        # render class destructor
        self.write_destructor(cpp_class, thiz)

        # render class methods
        constructor_index = 0
        for c_cursor in cursor.get_children():
            if (c_cursor.access_specifier != clang.cindex.AccessSpecifier.PUBLIC):
                continue

            match c_cursor.kind:
                case clang.cindex.CursorKind.CONSTRUCTOR:
                    return_type = c_cursor.result_type.spelling
                    method_name = c_cursor.spelling

                    print(f'Constructor: {method_name}')
                    params, args = self.parse_param_list(c_cursor)
                    print(f'Parameters:')
                    [print(f'  {e}') for e in params]

                    # render class constructor
                    declaration = (
                        f'  CCEFVIEW_EXPORT {thiz} * {method_name}_new{constructor_index}({", ".join(params)});\n'
                    )
                    self.header_file.write(declaration)
                    print(declaration, end='')
                    definition = (
                        f'{thiz} * {cpp_class}_new{constructor_index}({", ".join(params)}) {{\n'
                        f'  return new {method_name}({", ".join(args)});\n'
                        '}\n\n'
                    )
                    self.impl_file.write(definition)
                    print(definition, end='')
                    constructor_index += 1

                case clang.cindex.CursorKind.CXX_METHOD:
                    print(f'******************************************')
                    return_type = c_cursor.result_type
                    method_name = c_cursor.spelling

                    print(f'Method: {method_name}')
                    params, args = self.parse_param_list(c_cursor)
                    print(f'Parameters:')
                    [print(f'  {e}') for e in params]

                    params.insert(0, f'{thiz} * thiz')

                    # render class method
                    print(f'Declaration:')
                    declaration = (
                        f'  CCEFVIEW_EXPORT {self.translate_type(return_type.spelling)} {cpp_class}_{method_name}({", ".join(params)});\n'
                    )
                    self.header_file.write(declaration)
                    print(declaration, end='')

                    print(f'Definition:')
                    definition = (
                        f'{self.translate_type(return_type.spelling)} {cpp_class}_{method_name}({", ".join(params)}) {{\n'
                        f'  {self.return_prefix(return_type)}thiz->{method_name}({", ".join(args)}){self.mapper.map_conversion(return_type.spelling)};\n'
                        '}\n\n'
                    )
                    self.impl_file.write(definition)
                    print(definition, end='')

    def translate_struct(self, cursor: clang.cindex.Cursor, target):
        """Write a struct to the source"""
        self.header_file.write(
            f'  typedef struct {cursor.spelling} {target.lower()}_struct;\n'
        )

    def translate_enum(self, cursor: clang.cindex.Cursor):
        """Write an enum to the source"""
        self.header_file.write(
            # f'  typedef enum {cursor.spelling} {cursor.spelling.lower()}_enum;\n'
            f'  typedef int {cursor.spelling.lower()}_enum;\n'
        )
