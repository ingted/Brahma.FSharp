﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brahman.Substrings;

namespace Brahman.Runner.CS
{

    class HDReader:MatcherCS.IHDReader
    {
        public MatcherCS.ReadingResult Read(byte[] buf)
        {
            // fill buffer
            buf[0] = 1;
            return new MatcherCS.ReadingResult(buf, /*If buffer do not contains new data for processing then 'true' else 'false'*/ false);
        }
    }

    public class Runner
    {

        static byte[][] first = 
        {
         new byte[] { 0x00 , 0x00, 0x00, 0x14, 0x66, 0x74, 0x79, 0x70, 0x69, 0x73, 0x6F, 0x6D },//MP4
         new byte[] { 0x00, 0x00, 0x00, 0x18, 0x66, 0x74, 0x79, 0x70, 0x33, 0x67, 0x70, 0x35},//MP4
         new byte[] { 0x00, 0x00, 0x00, 0x1C, 0x66, 0x74, 0x79, 0x70, 0x4D, 0x53, 0x4E, 0x56, 0x01, 0x29, 0x00, 0x46, 0x4D, 0x53, 0x4E, 0x56, 0x6D, 0x70, 0x34, 0x32},//MP4
         new byte[] { 0x1A, 0x45, 0xDF, 0xA3, 0x93, 0x42, 0x82, 0x88, 0x6D, 0x61, 0x74, 0x72, 0x6F, 0x73, 0x6B, 0x61},//MKV
         new byte[] { 0x25, 0x50, 0x44, 0x46},//PDF
         new byte[] { 0x00, 0x00, 0x01, 0xBA},//MPG/VOB
         new byte[] { 0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11, 0xA6, 0xD9, 0x00, 0xAA, 0x00, 0x62, 0xCE, 0x6C},//WMA/WMV
         new byte[] { 0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C},//7z
         new byte[] { 0x38, 0x42, 0x50, 0x53},//PSD
         new byte[] { 0x42, 0x4D}//BMP
       };

        static byte[][] templates =
        { 
        new byte[] { 0x43, 0x57, 0x53 },//SWF
        new byte[] { 0x46, 0x57, 0x53 },//SWF
        new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 },//GIF
        new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 },//GIF
        new byte[] { 0x49, 0x20, 0x49 },//TIFF
        new byte[] { 0x49, 0x44, 0x33 },//MP3
        new byte[] { 0x49, 0x49, 0x2A, 0x00 },//TIFF
        new byte[] { 0x4A, 0x41, 0x52, 0x43, 0x53, 0x00 },//JAR
        new byte[] { 0x4F, 0x67, 0x67, 0x53, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },//OGG
        new byte[] { 0x50, 0x4B, 0x03, 0x04 },//ZIP
        new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x01, 0x00, 0x63, 0x00, 0x00, 0x00, 0x00, 0x00 },//ZIP
        new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x06, 0x00 },//DOCX
        new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x08, 0x00, 0x08, 0x00 },//JAR
        new byte[] { 0x50, 0x4B, 0x05, 0x06 },//ZIP
        new byte[] { 0x50, 0x4B, 0x07, 0x08 },//ZIP
        new byte[] { 0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x00 },//RAR
        new byte[] { 0x66, 0x4C, 0x61, 0x43, 0x00, 0x00, 0x00, 0x22 },//FLAC
        new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A },//PNG
        new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 },//DOC
        new byte[] { 0xE3, 0x10, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00 }//INFO
    };

        static byte[][] additional = 
        { 
        new byte[] { 0x00, 0x00, 0x00, 0x0C, 0x6A, 0x50, 0x20, 0x20, 0x0D, 0x0A },//JP2
        new byte[] { 0x00, 0x00, 0x01, 0x00 },//ICO
        new byte[] { 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x02, 0x00, 0x01 },//MDF
        new byte[] { 0x01, 0x00, 0x09, 0x00, 0x00, 0x03 },//WMF
        new byte[] { 0x09, 0x08, 0x10, 0x00, 0x00, 0x06, 0x05, 0x00 },//XLS
        new byte[] { 0x0F, 0x00, 0xE8, 0x03 },//PPT
        new byte[] { 0x1F, 0x8B, 0x08 },//GZ/TGZ
        new byte[] { 0x3C, 0x21, 0x64, 0x6F, 0x63, 0x74, 0x79, 0x70 },//HTML
        new byte[] { 0x3C, 0x3F, 0x78, 0x6D, 0x6C, 0x20, 0x76, 0x65, 0x72, 0x73, 0x69, 0x6F, 0x6E, 0x3D },//MANIFEST
        new byte[] { 0x3C, 0x3F, 0x78, 0x6D, 0x6C, 0x20, 0x76, 0x65, 0x72, 0x73, 0x69, 0x6F, 0x6E, 0x3D, 0x22, 0x31, 0x2E, 0x30, 0x22, 0x3F, 0x3E },//XUL
        new byte[] { 0x3F, 0x5F, 0x03, 0x00 },//HLP
        new byte[] { 0x43, 0x6C, 0x69, 0x65, 0x6E, 0x74, 0x20, 0x55, 0x72, 0x6C, 0x43, 0x61, 0x63, 0x68, 0x65, 0x20, 0x4D, 0x4D, 0x46, 0x20, 0x56, 0x65, 0x72, 0x20 },//DAT
        new byte[] { 0x49, 0x53, 0x63, 0x28 },//CAB
        new byte[] { 0x49, 0x54, 0x53, 0x46 },//CHM
        new byte[] { 0x4D, 0x4D, 0x00, 0x2A },//TIFF
        new byte[] { 0x4D, 0x4D, 0x00, 0x2B },//TIFF
        new byte[] { 0x4D, 0x54, 0x68, 0x64 },//MIDI
        new byte[] { 0x4D, 0x5A },//EXE/DLL
        new byte[] { 0x4D, 0x5A, 0x90, 0x00, 0x03, 0x00, 0x00, 0x00 },//API
        new byte[] { 0x4D, 0x69, 0x63, 0x72, 0x6F, 0x73, 0x6F, 0x66, 0x74, 0x20, 0x43, 0x2F, 0x43, 0x2B, 0x2B, 0x20 }//PDB
    };


        static void Main()
        {
            var matcher = new MatcherCS.MatcherCS();            
            var r = matcher.RabinKarp( new HDReader(), templates);
            System.Console.WriteLine("Ordered templates: " + r.Templates);
            System.Console.WriteLine("Chunk size: " + r.ChunkSize);
            foreach (var i in r.Data)
            {
                System.Console.WriteLine(i.PatternId + "; " + i.ChunkNum + "; " + i.Offset);
            }
        }
    }
}
