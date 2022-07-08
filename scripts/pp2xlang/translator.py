#!/usr/bin/env python
""" Usage: call with <filename> <typename>
"""

import abc
import clang.cindex


class TypeMapper(metaclass=abc.ABCMeta):
    @classmethod
    def __subclasshook__(cls, subclass):
        return (
            hasattr(subclass, 'map_type')
            and callable(subclass.map_type)
            or NotImplemented
        )

    @abc.abstractmethod
    def map_type(self, tu: clang.cindex.TranslationUnit):
        """"""
        pass

    @abc.abstractmethod
    def map_conversion(self, tu: clang.cindex.TranslationUnit):
        """"""
        pass


class SourceGenerator(metaclass=abc.ABCMeta):
    @classmethod
    def __subclasshook__(cls, subclass):
        return (
            hasattr(subclass, 'parse')
            and callable(subclass.parse)
            and hasattr(subclass, 'prepare')
            and callable(subclass.prepare)
            and hasattr(subclass, 'finalize')
            and callable(subclass.finalize)
            and hasattr(subclass, 'translate_cursor')
            and callable(subclass.translate_cursor)
            or NotImplemented
        )

    @abc.abstractmethod
    def parse(self, tu: clang.cindex.TranslationUnit):
        """"""
        pass

    @abc.abstractmethod
    def prepare(self, tu: clang.cindex.TranslationUnit):
        """"""
        raise NotImplementedError

    @abc.abstractmethod
    def finalize(self):
        """"""
        raise NotImplementedError

    @abc.abstractmethod
    def translate_cursor(self, cursor: clang.cindex.Cursor):
        """Write a cursor to the source"""
        raise NotImplementedError


class Translator():
    """
    """

    def __init__(self, args: list):
        """
        """
        self.index = clang.cindex.Index.create()
        self.tus = []
        self.generators = []
        self.args = args

    def add_generator(self, gen):
        self.generators.append(gen)

    def parse(self, source: str, args: list = []):
        print(f'== Parsing Translation Unit: {source} ==')
        merged_args = list(dict.fromkeys(self.args + args))
        tu = self.index.parse(
            source, merged_args, options=clang.cindex.TranslationUnit.PARSE_INCOMPLETE)
        [print(f"  DIAGNOSTIC: {x}") for x in tu.diagnostics]
        [x.parse(tu) for x in self.generators]
        self.tus.append(tu)

    def translate(self):
        """
        """
        for tu in self.tus:
            [x.prepare(tu) for x in self.generators]

            for cursor in tu.cursor.get_children():

                if (cursor.location.file.name == tu.spelling):
                    [x.translate_cursor(cursor) for x in self.generators]

            [x.finalize() for x in self.generators]
