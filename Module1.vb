Imports System.IO
Imports System.Text

Module Module1

    Structure pod6header
        Public magic() As Char
        Public files As UInt32
        Public Version As UInt32
        Public index_offset As UInt32
        Public size_index As UInt32
    End Structure
    Structure pod6entry
        Public path_offset As UInt32
        Public size As UInt32
        Public offset As UInt32
        Public uncompressed As UInt32
        Public compression_level As UInt32
        Public zero As UInt32
    End Structure
    Structure pod6file
        Public head As pod6header
        Public contents As Object
        Public entry As List(Of pod6entry)
        Public filename As List(Of Char())
    End Structure
    Sub Main()
        Dim fs As FileStream = New FileStream(CurDir() + "\W64MODEL.POD", FileMode.Open)
        Dim bread As BinaryReader = New BinaryReader(fs, Text.Encoding.ASCII)
        Dim pfile As pod6file = New pod6file()
        pfile.entry = New List(Of pod6entry)
        pfile.filename = New List(Of Char())
        pfile.head.magic = bread.ReadChars(4)
        pfile.head.files = bread.ReadUInt32()
        pfile.head.Version = bread.ReadUInt32()
        pfile.head.index_offset = bread.ReadUInt32()
        pfile.head.size_index = bread.ReadUInt32()
        bread.BaseStream.Position = pfile.head.index_offset
        For i As Integer = 1 To pfile.head.files
            bread.BaseStream.Position = pfile.head.index_offset + (i * 24)
            Dim pent As pod6entry = New pod6entry()
            pent.path_offset = bread.ReadUInt32()
            pent.size = bread.ReadUInt32()
            pent.offset = bread.ReadUInt32()
            pent.uncompressed = bread.ReadUInt32()
            pent.compression_level = bread.ReadUInt32()
            pent.zero = bread.ReadUInt32()
            Dim pos = bread.BaseStream.Position
            pfile.entry.Add(pent)
            bread.BaseStream.Position = pfile.head.index_offset + (pfile.head.files * 24) + pent.size
            Dim names = bread.ReadChars(256)
            pfile.filename.Add(names)
            Dim value As String = New String(names)
            Console.WriteLine(value.Trim() + vbNewLine)
            bread.BaseStream.Position = pos

        Next

        Dim fsize = pfile.head.index_offset + pfile.head.files * pfile.entry.ToArray().Length + pfile.head.size_index
        Console.WriteLine(fsize.ToString())
        Console.ReadKey()
    End Sub
    'Sub test()
    '    Dim fs As FileStream = New FileStream(CurDir() + "\W64MODEL.POD", FileMode.Open)
    '    Dim bread As BinaryReader = New BinaryReader(fs, Text.Encoding.ASCII)
    '    Dim magic = bread.ReadChars(4)
    '    Dim files = bread.ReadUInt32()
    '    Dim Version = bread.ReadUInt32()
    '    Dim index_offset = bread.ReadUInt32()
    '    Dim size_index = bread.ReadUInt32()
    '    bread.BaseStream.Position = index_offset
    '    For i As Integer = 1 To files
    '        bread.BaseStream.Position = index_offset + (i * 24)
    '        Dim path_offset = bread.ReadUInt32()
    '        Dim size = bread.ReadUInt32()
    '        Dim offset = bread.ReadUInt32()
    '        Dim uncompressed = bread.ReadUInt32()
    '        Dim compression_level = bread.ReadUInt32()
    '        Dim zero = bread.ReadUInt32()
    '        Dim pos = bread.BaseStream.Position
    '        bread.BaseStream.Position = index_offset + (files * 24) + size
    '        Dim names = bread.ReadChars(256)
    '        Console.WriteLine(names)
    '        bread.BaseStream.Position = pos

    '    Next
    '    Console.ReadKey()
    'End Sub
End Module
