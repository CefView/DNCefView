#!/usr/bin/env python

import clang.cindex

if __name__ == '__main__':
    index = clang.cindex.Index.create()
    args = ['-x', 'c++', '-std=c++17', '-Iinclude']
    tu = index.parse('src/CefSetting.h', args)
    [print(f"  DIAGNOSTIC: {x}") for x in tu.diagnostics]

    for cursor in tu.cursor.get_children():
        match cursor.kind:
            case clang.cindex.CursorKind.CLASS_DECL:
                print(f'  {cursor.spelling}, {cursor.kind}')

                for method in cursor.get_children():
                    match method.kind:
                        case clang.cindex.CursorKind.CXX_METHOD:
                            rtype = method.result_type
                            params = []
                            args = []
                            for m_cursor in method.get_children():
                                match m_cursor.kind:
                                    case clang.cindex.CursorKind.PARM_DECL:
                                        param_type = m_cursor.type
                                        param_name = m_cursor.spelling
                                        params.append(
                                            f'{param_type.spelling} {param_name}')
                                        args.append(param_name)

                            field = method.spelling.replace('set', '')
                            field = field[0:1].lower() + field[1:] + '_'

                            statement = ''
                            if (len(args) == 0):
                                statement = f'return pImpl_->{field};\n'
                            else:
                                statement = f'pImpl_->{field} = {args[0]};\n'

                            print(
                                f'{rtype.spelling} {cursor.spelling}::{method.spelling}({",".join(params)}) {"const" if method.type.spelling.endswith("const") else ""} {{\n'
                                f'  {statement}'
                                '}\n'
                            )
