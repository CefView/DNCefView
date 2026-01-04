#!/usr/bin/env python

import os
import clang.cindex

from pp2xlang.translator import TypeMapper
from pp2xlang.translator import SourceGenerator

# TYPE_PREFIX = "DN"
TYPE_PREFIX = ""


class Parameter:
    def __init__(self, type, basic, spelling) -> None:
        self.type = type
        self.basic = basic
        self.spelling = spelling


class CSharpGenerator(SourceGenerator):
    def __init__(self, out: str, namespace: str, dll: str, mapper: TypeMapper) -> None:
        super().__init__()
        self.types = {}
        self.out = out
        self.namespace = namespace
        self.dll = dll
        self.mapper = mapper

    #################################################################################
    def translate_type(self, t, marshal: bool = False):
        type = t.spelling
        is_ref = False

        match t.get_canonical().kind:
            case clang.cindex.TypeKind.LVALUEREFERENCE | clang.cindex.TypeKind.POINTER:
                pointee = t.get_pointee()
                if not pointee.kind in [
                    clang.cindex.TypeKind.VOID, 
                    clang.cindex.TypeKind.SCHAR, 
                    clang.cindex.TypeKind.CHAR_S, 
                    clang.cindex.TypeKind.UCHAR,
                    clang.cindex.TypeKind.CHAR_U,
                ]:
                    is_ref = not pointee.is_const_qualified()
                    type = pointee.spelling

        type = type.replace("const", "").strip()

        target_type = self.mapper.map_type(type)
        for k, v in self.types.items():
            if k in target_type:
                return v

        if marshal and target_type == "string":
            return "IntPtr"

        return f"{'ref ' if is_ref else ''}{target_type}"

    def parse_param_list(self, cursor: clang.cindex.Cursor):
        params = []
        args = []
        
        # collect data
        for c in cursor.get_children():
            match c.kind:
                case clang.cindex.CursorKind.PARM_DECL:
                    is_ref = False
                    param_type = c.type
                    basic_type = c.type
                    param_name = c.spelling

                    match param_type.kind:
                        case clang.cindex.TypeKind.LVALUEREFERENCE | clang.cindex.TypeKind.POINTER:
                            for cc in c.get_children():
                                match cc.kind:
                                    case clang.cindex.CursorKind.TYPE_REF:
                                        basic_type = cc.type
                        case _:
                            pass

                    params.append(
                        Parameter(
                            param_type.spelling,
                            basic_type.spelling,
                            f"{self.translate_type(param_type)} {param_name}"
                        )
                    )
                    args.append(param_name)

        return (params, args)

    def marshal_params(self, params, args, out_params, out_args):
        for i in range(0, len(params)):
            p = params[i]
            a = args[i]

            if p.basic.startswith("CCef") and p.basic in self.types.keys():
                out_params.append(p.spelling.replace(self.types[p.basic], "IntPtr"))
                out_args.append(f"{a}.NativeObject")
            elif p.basic.startswith("std::string"):
                out_params.append(f"[MarshalAs(UnmanagedType.LPUTF8Str)] " + p.spelling)
                out_args.append(a)
            elif "[]" in p.spelling:
                out_params.append(f"[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = {len(out_params) + 1})] " + p.spelling)
                out_args.append(a)
            else:
                out_params.append(p.spelling)
                out_args.append(a)

    def return_prefix(self, rt):
        match rt.get_canonical().kind:
            case clang.cindex.TypeKind.VOID:
                return ""
            # case clang.cindex.TypeKind.ENUM:
            #     return f"return ({self.translate_type(rt.get_canonical())})"
            case _:
                return "return "

    def convert_arg_name(self, name):
        parts = name.split("_")
        # Capitalize all parts except the first, then join them together
        return "".join(word.capitalize() for word in parts[0:])

    def extract_funcptr_name(self, name):
        return name.replace("pfn", "")

    #################################################################################
    def parse(self, tu: clang.cindex.TranslationUnit):
        """"""
        for cursor in tu.cursor.get_children():
            if cursor.location.file.name != tu.spelling:
                continue
            else:
                self.parse_cursor(cursor)

    def parse_cursor(self, cursor: clang.cindex.Cursor):
        """"""
        print(f"  parsing {cursor.kind}:{cursor.spelling} ")
        match cursor.kind:
            case clang.cindex.CursorKind.TYPEDEF_DECL:
                self.types[cursor.underlying_typedef_type.get_canonical().spelling] = (
                    f"{TYPE_PREFIX}{cursor.spelling}"
                )
            case clang.cindex.CursorKind.STRUCT_DECL:
                self.types[cursor.spelling] = f"{TYPE_PREFIX}{cursor.spelling}"
            case clang.cindex.CursorKind.ENUM_DECL:
                self.types[cursor.spelling] = f"{TYPE_PREFIX}{cursor.spelling}"
            case clang.cindex.CursorKind.CLASS_DECL:
                self.types[cursor.spelling] = (
                    f'{TYPE_PREFIX}{cursor.spelling[1:] if cursor.spelling[0] == "C" else cursor.spelling[0:]}'
                )

    def prepare(self, tu: clang.cindex.TranslationUnit):
        """"""
        self.source_name = os.path.splitext(os.path.basename(tu.spelling))[0]
        self.source_file = open(
            f"{os.path.join(self.out, self.source_name)}+AutoGen.cs", "wt"
        )
        # self.user_file = open(
        #     f'{os.path.join(self.out, self.source_name)}.cs', 'wt')

        header = (
            f"#pragma warning disable CS8603\n"
            f"using System;\n"
            f"using System.Runtime.InteropServices;\n"
            f"\n"
            f"namespace {self.namespace}\n"
            f"{{\n"
        )
        self.source_file.write(header)
        # self.user_file.write(header)

    def finalize(self):
        """"""
        footer = "}"

        self.source_file.write(footer)
        self.source_file.close()

        # self.user_file.write(footer)
        # self.user_file.close()

        self.source_name = ""

    def translate_cursor(self, cursor: clang.cindex.Cursor):
        """ """
        match cursor.kind:
            case clang.cindex.CursorKind.TYPEDEF_DECL:
                print(f"* Found using clause: {cursor.spelling}, {cursor.kind}")
                c = cursor.underlying_typedef_type.get_canonical().get_declaration()
                c._spelling = cursor.spelling
                c._displayname = cursor.displayname
                self.translate_cursor(c)
                pass
            case clang.cindex.CursorKind.TYPEDEF_DECL:
                print(f"* Found typedef clause: {cursor.spelling}, {cursor.kind}")
                pass
            case clang.cindex.CursorKind.ENUM_DECL:
                print(f"* Found enum clause: {cursor.spelling}, {cursor.kind}")
                self.translate_enum(cursor)
                pass
            case clang.cindex.CursorKind.STRUCT_DECL:
                print(f"* Found struct clause: {cursor.spelling}, {cursor.kind}")
                self.translate_struct(cursor)
                pass
            case clang.cindex.CursorKind.CLASS_DECL:
                print(f"* Found class clause: {cursor.spelling}, {cursor.kind}")
                self.translate_class(cursor)
                pass

    def translate_class(self, cursor: clang.cindex.Cursor):
        """Write a class to the source"""
        cpp_class = cursor.spelling
        sharp_class = (
            f'{TYPE_PREFIX}{cpp_class[1:] if cpp_class[0] == "C" else cpp_class[0:]}'
        )

        class_header = (
            f'    // Source: {cpp_class} \n'
            f"    public partial class {sharp_class} : IDisposable\n"
            f"    {{\n"
            f"        private IntPtr _native;\n"
            f"        public IntPtr NativeObject\n"
            f"        {{\n"
            f"            get {{ return _native; }}\n"
            f"        }}\n"
            f"\n"
            f"        public void Dispose()\n"
            f"        {{\n"
            f"            Dispose(true);\n"
            f"            GC.SuppressFinalize(this);\n"
            f"        }}\n"
            f"\n"
            f'        [DllImport("{self.dll}")]\n'
            f"        private static extern void {cpp_class}_Delete(IntPtr p);\n"
            f"        protected virtual void Dispose(bool disposing)\n"
            f"        {{\n"
            f"            if (disposing)\n"
            f"            {{\n"
            f"                // TODO: cleanup the managed resources\n"
            f"            }}\n"
            f"\n"
            f"            // cleanup unmanaged resources\n"
            f"            if (_native != IntPtr.Zero)\n"
            f"            {{\n"
            f"                {cpp_class}_Delete(_native);\n"
            f"                _native = IntPtr.Zero;\n"
            f"            }}\n"
            f"        }}\n"
            f"\n"
        )

        self.source_file.write(class_header)

        # render class methods
        constructor_index = 0
        for c_cursor in cursor.get_children():
            if c_cursor.access_specifier != clang.cindex.AccessSpecifier.PUBLIC:
                continue

            match c_cursor.kind:
                case clang.cindex.CursorKind.CONSTRUCTOR:
                    print(f"******************************************")
                    cpp_constructor_name = c_cursor.spelling
                    print(f"Constructor: {cpp_constructor_name}")

                    params, args = self.parse_param_list(c_cursor)
                    print(f"  Parameters:")
                    [print(f"    {e}") for e in params]

                    marshal_params = []
                    self.marshal_params(params, args, marshal_params, [])

                    # render class constructor
                    constructor = (
                        f"        // Source: {c_cursor.displayname}\n"
                        f'        [DllImport("{self.dll}")]\n'
                        f'        private static extern IntPtr {cpp_class}_new{constructor_index}({", ".join(marshal_params)});\n'
                        f"\n"
                    )
                    self.source_file.write(constructor)
                    print(constructor, end="")
                    constructor_index += 1

                case clang.cindex.CursorKind.CXX_METHOD:
                    print(f"******************************************")
                    cpp_method_name = c_cursor.spelling
                    print(f"Method: {cpp_method_name}")
                    cpp_return_type = c_cursor.result_type

                    csharp_method_name = (
                        c_cursor.spelling[:1].upper() + c_cursor.spelling[1:]
                    )

                    params, args = self.parse_param_list(c_cursor)
                    print(f"  Parameters:")
                    [print(f"    {e}") for e in params]
                    
                    marshal_params = [f"IntPtr thiz"]
                    marshal_args = ["_native"]
                    self.marshal_params(params, args, marshal_params, marshal_args)

                    # render class method
                    print(f"Definition:")
                    method = (
                        f"        // Source: {cpp_return_type.spelling} {c_cursor.displayname}\n"
                        f'        [DllImport("{self.dll}")]\n'
                        f'        private static extern {self.translate_type(cpp_return_type, True)} {cpp_class}_{cpp_method_name}({", ".join(marshal_params)});\n'
                        f'        public {self.translate_type(cpp_return_type)} {csharp_method_name}({", ".join([p.spelling for p in params])})\n'
                        f"        {{\n"
                    )
                    if self.translate_type(cpp_return_type) == "string":
                        method += f'            return Marshal.PtrToStringUTF8({cpp_class}_{cpp_method_name}({", ".join(marshal_args)}));\n'
                    else:
                        method += f'            {self.return_prefix(cpp_return_type)}{cpp_class}_{cpp_method_name}({", ".join(marshal_args)});\n'

                    method += f"        }}\n\n"

                    self.source_file.write(method)
                    print(method, end="")

        self.source_file.write(f"    }}\n")

    def translate_struct(self, cursor: clang.cindex.Cursor):
        """Write struct to the source"""
        print(f"--------- Translating struct: {cursor.spelling} ---------")
        cpp_struct = cursor.spelling
        sharp_struct = f"{TYPE_PREFIX}{cpp_struct}"

        struct_header = (
            f'    // Source: {cpp_struct} \n'
            f"    [StructLayout(LayoutKind.Sequential)]\n"
            f"    public partial struct {sharp_struct}\n"
            f"    {{\n"
        )

        self.source_file.write(struct_header)

        for f_cursor in cursor.get_children():
            if f_cursor.access_specifier != clang.cindex.AccessSpecifier.PUBLIC:
                continue

            match f_cursor.kind:
                case clang.cindex.CursorKind.FIELD_DECL:
                    print(f"==== struct member: {f_cursor.spelling}")

                    # write the field declaration
                    match f_cursor.type.get_canonical().kind:
                        case clang.cindex.TypeKind.POINTER:
                            pointee = f_cursor.type.get_canonical().get_pointee()
                            if pointee.kind in [
                                clang.cindex.TypeKind.FUNCTIONPROTO,
                                clang.cindex.TypeKind.FUNCTIONNOPROTO,
                            ]:
                                params, args = self.parse_param_list(f_cursor)
                                print(f"  Parameters:")
                                [print(f"    {e}") for e in params]

                                marshal_params = []
                                self.marshal_params(params, args, marshal_params, [])

                                self.source_file.write(
                                    f"        // Source: {f_cursor.type.get_canonical().spelling.replace('(*)', f_cursor.displayname)}\n"
                                    f"        [UnmanagedFunctionPointer(CallingConvention.StdCall)]\n"
                                    f"        public delegate {self.translate_type(pointee.get_result())} {self.extract_funcptr_name(f_cursor.spelling)}Callback({", ".join(marshal_params)});\n"
                                    f"        public {self.extract_funcptr_name(f_cursor.spelling)}Callback {self.extract_funcptr_name(f_cursor.spelling)}Cb;\n"
                                )
                            else:
                                self.source_file.write(
                                    f"        // Source: {f_cursor.type.spelling} {f_cursor.displayname}\n"
                                    f"        public {self.translate_type(f_cursor.type.get_canonical())} {self.convert_arg_name(f_cursor.spelling)};\n"
                                )
                        case _:
                            self.source_file.write(
                                f"        // Source: {f_cursor.type.spelling} {f_cursor.displayname}\n"
                                f"        public {self.translate_type(f_cursor.type.get_canonical())} {self.convert_arg_name(f_cursor.spelling)};\n"
                            )
                            pass

                    self.source_file.write(f"\n")

        self.source_file.write(f"    }}\n\n")

    def translate_enum(self, cursor: clang.cindex.Cursor):
        """Write an enum to the source"""
        enum = f"    public enum {TYPE_PREFIX}{cursor.spelling}\n" f"    {{\n"

        for c_cursor in cursor.get_children():
            print(f"{c_cursor.spelling}")
            enum += f"        {c_cursor.spelling} = {c_cursor.enum_value},\n"

        enum += "    };\n\n"

        self.source_file.write(enum)
