﻿namespace Clowd.Clipboard.Bitmaps.Core;

internal struct G31DFaxCode
{
    public byte bitLength;
    public ushort code;
    public int runLength;

    public G31DFaxCode(byte bl, ushort c, int run)
    {
        bitLength = bl;
        code = c;
        runLength = run;
    }
}

internal class CCITTHuffmanG31D
{
    // 1 = black and 0 = white
    // Run lengths are represented by two types of code words: makeup and terminating
    // The run-length code words are taken from a predefined table of values representing runs of black or white pixels. 
    // This table is part of the T.4 specification and is used to encode and decode all Group 3 data.


    // An encoded pixel run is made up of zero or more makeup code words and a terminating code word
    // Terminating code words represent shorter runs, and makeup codes represent longer runs. There are separate terminating and makeup code words for both black and white runs.


    // Pixel runs with a length of 0 to 63 are encoded using a single terminating code. 
    // Runs of 64 to 2623 pixels are encoded by a single makeup code and a terminating code. 
    // Run lengths greater than 2623 pixels are encoded using one or more makeup codes and a terminating code. 
    // The run length is the sum of the length values represented by each code word.

    // To ensure that the receiver (decompressor) maintains color synchronization, all
    // data lines begin with a white run-length code word set.If the actual scan line
    // begins with a black run, a white run-length of zero is sent (written).


    // m_faxBlackCodes
    // https://github.com/BitMiracle/libtiff.net/blob/4eb982e17da30be0deb5fae6ce794a7a65524c71/LibTiff/Internal/CCITTCodec.cs#L1336 Fax3Decode1D
    // https://github.com/BitMiracle/libtiff.net/blob/4eb982e17da30be0deb5fae6ce794a7a65524c71/LibTiff/Internal/CCITTCodec.cs#L1834 EXPAND1D
    // https://github.com/BitMiracle/libtiff.net/blob/4eb982e17da30be0deb5fae6ce794a7a65524c71/LibTiff/Internal/CCITTCodec_Data.cs tables...
    // https://www.itu.int/itudoc/itu-t/com16/tiff-fx/docs/tiff6.pdf Section10: Modified Huffman Compression
    // http://zig.tgschultz.com/bmp_file_format.txt really good description of Huffman1D in the context of bitmaps

    //internal const short G3CODE_EOL = -1;  /* NB: ACT_EOL - ACT_WRUNT */
    //internal const short G3CODE_INVALID = -2;  /* NB: ACT_INVALID - ACT_WRUNT */
    //internal const short G3CODE_EOF = -3;  /* end of input data */
    //internal const short G3CODE_INCOMP = -4;  /* incomplete run code */

    private static readonly short[] faxWhiteCodes =
    {
            8, 0x35, 0,  /* 0011 0101 */
            6, 0x7, 1,  /* 0001 11 */
            4, 0x7, 2,  /* 0111 */
            4, 0x8, 3,  /* 1000 */
            4, 0xB, 4,  /* 1011 */
            4, 0xC, 5,  /* 1100 */
            4, 0xE, 6,  /* 1110 */
            4, 0xF, 7,  /* 1111 */
            5, 0x13, 8,  /* 1001 1 */
            5, 0x14, 9,  /* 1010 0 */
            5, 0x7, 10,  /* 0011 1 */
            5, 0x8, 11,  /* 0100 0 */
            6, 0x8, 12,  /* 0010 00 */
            6, 0x3, 13,  /* 0000 11 */
            6, 0x34, 14,  /* 1101 00 */
            6, 0x35, 15,  /* 1101 01 */
            6, 0x2A, 16,  /* 1010 10 */
            6, 0x2B, 17,  /* 1010 11 */
            7, 0x27, 18,  /* 0100 111 */
            7, 0xC, 19,  /* 0001 100 */
            7, 0x8, 20,  /* 0001 000 */
            7, 0x17, 21,  /* 0010 111 */
            7, 0x3, 22,  /* 0000 011 */
            7, 0x4, 23,  /* 0000 100 */
            7, 0x28, 24,  /* 0101 000 */
            7, 0x2B, 25,  /* 0101 011 */
            7, 0x13, 26,  /* 0010 011 */
            7, 0x24, 27,  /* 0100 100 */
            7, 0x18, 28,  /* 0011 000 */
            8, 0x2, 29,  /* 0000 0010 */
            8, 0x3, 30,  /* 0000 0011 */
            8, 0x1A, 31,  /* 0001 1010 */
            8, 0x1B, 32,  /* 0001 1011 */
            8, 0x12, 33,  /* 0001 0010 */
            8, 0x13, 34,  /* 0001 0011 */
            8, 0x14, 35,  /* 0001 0100 */
            8, 0x15, 36,  /* 0001 0101 */
            8, 0x16, 37,  /* 0001 0110 */
            8, 0x17, 38,  /* 0001 0111 */
            8, 0x28, 39,  /* 0010 1000 */
            8, 0x29, 40,  /* 0010 1001 */
            8, 0x2A, 41,  /* 0010 1010 */
            8, 0x2B, 42,  /* 0010 1011 */
            8, 0x2C, 43,  /* 0010 1100 */
            8, 0x2D, 44,  /* 0010 1101 */
            8, 0x4, 45,  /* 0000 0100 */
            8, 0x5, 46,  /* 0000 0101 */
            8, 0xA, 47,  /* 0000 1010 */
            8, 0xB, 48,  /* 0000 1011 */
            8, 0x52, 49,  /* 0101 0010 */
            8, 0x53, 50,  /* 0101 0011 */
            8, 0x54, 51,  /* 0101 0100 */
            8, 0x55, 52,  /* 0101 0101 */
            8, 0x24, 53,  /* 0010 0100 */
            8, 0x25, 54,  /* 0010 0101 */
            8, 0x58, 55,  /* 0101 1000 */
            8, 0x59, 56,  /* 0101 1001 */
            8, 0x5A, 57,  /* 0101 1010 */
            8, 0x5B, 58,  /* 0101 1011 */
            8, 0x4A, 59,  /* 0100 1010 */
            8, 0x4B, 60,  /* 0100 1011 */
            8, 0x32, 61,  /* 0011 0010 */
            8, 0x33, 62,  /* 0011 0011 */
            8, 0x34, 63,  /* 0011 0100 */

            // end of terminating codes
            // begin makeup codes

            5, 0x1B, 64,  /* 1101 1 */
            5, 0x12, 128,  /* 1001 0 */
            6, 0x17, 192,  /* 0101 11 */
            7, 0x37, 256,  /* 0110 111 */
            8, 0x36, 320,  /* 0011 0110 */
            8, 0x37, 384,  /* 0011 0111 */
            8, 0x64, 448,  /* 0110 0100 */
            8, 0x65, 512,  /* 0110 0101 */
            8, 0x68, 576,  /* 0110 1000 */
            8, 0x67, 640,  /* 0110 0111 */
            9, 0xCC, 704,  /* 0110 0110 0 */
            9, 0xCD, 768,  /* 0110 0110 1 */
            9, 0xD2, 832,  /* 0110 1001 0 */
            9, 0xD3, 896,  /* 0110 1001 1 */
            9, 0xD4, 960,  /* 0110 1010 0 */
            9, 0xD5, 1024,  /* 0110 1010 1 */
            9, 0xD6, 1088,  /* 0110 1011 0 */
            9, 0xD7, 1152,  /* 0110 1011 1 */
            9, 0xD8, 1216,  /* 0110 1100 0 */
            9, 0xD9, 1280,  /* 0110 1100 1 */
            9, 0xDA, 1344,  /* 0110 1101 0 */
            9, 0xDB, 1408,  /* 0110 1101 1 */
            9, 0x98, 1472,  /* 0100 1100 0 */
            9, 0x99, 1536,  /* 0100 1100 1 */
            9, 0x9A, 1600,  /* 0100 1101 0 */
            6, 0x18, 1664,  /* 0110 00 */
            9, 0x9B, 1728,  /* 0100 1101 1 */

            // begin SHARED makeup codes

            11, 0x8, 1792,  /* 0000 0001 000 */
            11, 0xC, 1856,  /* 0000 0001 100 */
            11, 0xD, 1920,  /* 0000 0001 101 */
            12, 0x12, 1984,  /* 0000 0001 0010 */
            12, 0x13, 2048,  /* 0000 0001 0011 */
            12, 0x14, 2112,  /* 0000 0001 0100 */
            12, 0x15, 2176,  /* 0000 0001 0101 */
            12, 0x16, 2240,  /* 0000 0001 0110 */
            12, 0x17, 2304,  /* 0000 0001 0111 */
            12, 0x1C, 2368,  /* 0000 0001 1100 */
            12, 0x1D, 2432,  /* 0000 0001 1101 */
            12, 0x1E, 2496,  /* 0000 0001 1110 */
            12, 0x1F, 2560,  /* 0000 0001 1111 */

            12, 0b_0000_0000_0001, -1, // EOL

            //12, 0x1, G3CODE_EOL,  /* 0000 0000 0001 */
            //9, 0x1, G3CODE_INVALID,  /* 0000 0000 1 */
            //10, 0x1, G3CODE_INVALID,  /* 0000 0000 01 */
            //11, 0x1, G3CODE_INVALID,  /* 0000 0000 001 */
            //12, 0x0, G3CODE_INVALID,  /* 0000 0000 0000 */
        };

    private static readonly short[] faxBlackCodes =
    {
            10, 0x37, 0,  /* 0000 1101 11 */
            3, 0x2, 1,  /* 010 */
            2, 0x3, 2,  /* 11 */
            2, 0x2, 3,  /* 10 */
            3, 0x3, 4,  /* 011 */
            4, 0x3, 5,  /* 0011 */
            4, 0x2, 6,  /* 0010 */
            5, 0x3, 7,  /* 0001 1 */
            6, 0x5, 8,  /* 0001 01 */
            6, 0x4, 9,  /* 0001 00 */
            7, 0x4, 10,  /* 0000 100 */
            7, 0x5, 11,  /* 0000 101 */
            7, 0x7, 12,  /* 0000 111 */
            8, 0x4, 13,  /* 0000 0100 */
            8, 0x7, 14,  /* 0000 0111 */
            9, 0x18, 15,  /* 0000 1100 0 */
            10, 0x17, 16,  /* 0000 0101 11 */
            10, 0x18, 17,  /* 0000 0110 00 */
            10, 0x8, 18,  /* 0000 0010 00 */
            11, 0x67, 19,  /* 0000 1100 111 */
            11, 0x68, 20,  /* 0000 1101 000 */
            11, 0x6C, 21,  /* 0000 1101 100 */
            11, 0x37, 22,  /* 0000 0110 111 */
            11, 0x28, 23,  /* 0000 0101 000 */
            11, 0x17, 24,  /* 0000 0010 111 */
            11, 0x18, 25,  /* 0000 0011 000 */
            12, 0xCA, 26,  /* 0000 1100 1010 */
            12, 0xCB, 27,  /* 0000 1100 1011 */
            12, 0xCC, 28,  /* 0000 1100 1100 */
            12, 0xCD, 29,  /* 0000 1100 1101 */
            12, 0x68, 30,  /* 0000 0110 1000 */
            12, 0x69, 31,  /* 0000 0110 1001 */
            12, 0x6A, 32,  /* 0000 0110 1010 */
            12, 0x6B, 33,  /* 0000 0110 1011 */
            12, 0xD2, 34,  /* 0000 1101 0010 */
            12, 0xD3, 35,  /* 0000 1101 0011 */
            12, 0xD4, 36,  /* 0000 1101 0100 */
            12, 0xD5, 37,  /* 0000 1101 0101 */
            12, 0xD6, 38,  /* 0000 1101 0110 */
            12, 0xD7, 39,  /* 0000 1101 0111 */
            12, 0x6C, 40,  /* 0000 0110 1100 */
            12, 0x6D, 41,  /* 0000 0110 1101 */
            12, 0xDA, 42,  /* 0000 1101 1010 */
            12, 0xDB, 43,  /* 0000 1101 1011 */
            12, 0x54, 44,  /* 0000 0101 0100 */
            12, 0x55, 45,  /* 0000 0101 0101 */
            12, 0x56, 46,  /* 0000 0101 0110 */
            12, 0x57, 47,  /* 0000 0101 0111 */
            12, 0x64, 48,  /* 0000 0110 0100 */
            12, 0x65, 49,  /* 0000 0110 0101 */
            12, 0x52, 50,  /* 0000 0101 0010 */
            12, 0x53, 51,  /* 0000 0101 0011 */
            12, 0x24, 52,  /* 0000 0010 0100 */
            12, 0x37, 53,  /* 0000 0011 0111 */
            12, 0x38, 54,  /* 0000 0011 1000 */
            12, 0x27, 55,  /* 0000 0010 0111 */
            12, 0x28, 56,  /* 0000 0010 1000 */
            12, 0x58, 57,  /* 0000 0101 1000 */
            12, 0x59, 58,  /* 0000 0101 1001 */
            12, 0x2B, 59,  /* 0000 0010 1011 */
            12, 0x2C, 60,  /* 0000 0010 1100 */
            12, 0x5A, 61,  /* 0000 0101 1010 */
            12, 0x66, 62,  /* 0000 0110 0110 */
            12, 0x67, 63,  /* 0000 0110 0111 */

            // end of terminating codes
            // begin makeup codes

            10, 0xF, 64,     /* 0000 0011 11 */
            12, 0xC8, 128,   /* 0000 1100 1000 */
            12, 0xC9, 192,   /* 0000 1100 1001 */
            12, 0x5B, 256,   /* 0000 0101 1011 */
            12, 0x33, 320,   /* 0000 0011 0011 */
            12, 0x34, 384,   /* 0000 0011 0100 */
            12, 0x35, 448,   /* 0000 0011 0101 */
            13, 0x6C, 512,   /* 0000 0011 0110 0 */
            13, 0x6D, 576,   /* 0000 0011 0110 1 */
            13, 0x4A, 640,   /* 0000 0010 0101 0 */
            13, 0x4B, 704,   /* 0000 0010 0101 1 */
            13, 0x4C, 768,   /* 0000 0010 0110 0 */
            13, 0x4D, 832,   /* 0000 0010 0110 1 */
            13, 0x72, 896,   /* 0000 0011 1001 0 */
            13, 0x73, 960,   /* 0000 0011 1001 1 */
            13, 0x74, 1024,  /* 0000 0011 1010 0 */
            13, 0x75, 1088,  /* 0000 0011 1010 1 */
            13, 0x76, 1152,  /* 0000 0011 1011 0 */
            13, 0x77, 1216,  /* 0000 0011 1011 1 */
            13, 0x52, 1280,  /* 0000 0010 1001 0 */
            13, 0x53, 1344,  /* 0000 0010 1001 1 */
            13, 0x54, 1408,  /* 0000 0010 1010 0 */
            13, 0x55, 1472,  /* 0000 0010 1010 1 */
            13, 0x5A, 1536,  /* 0000 0010 1101 0 */
            13, 0x5B, 1600,  /* 0000 0010 1101 1 */
            13, 0x64, 1664,  /* 0000 0011 0010 0 */
            13, 0x65, 1728,  /* 0000 0011 0010 1 */

            // begin SHARED makeup codes

            11, 0x8, 1792,   /* 0000 0001 000 */
            11, 0xC, 1856,   /* 0000 0001 100 */
            11, 0xD, 1920,   /* 0000 0001 101 */
            12, 0x12, 1984,  /* 0000 0001 0010 */
            12, 0x13, 2048,  /* 0000 0001 0011 */
            12, 0x14, 2112,  /* 0000 0001 0100 */
            12, 0x15, 2176,  /* 0000 0001 0101 */
            12, 0x16, 2240,  /* 0000 0001 0110 */
            12, 0x17, 2304,  /* 0000 0001 0111 */
            12, 0x1C, 2368,  /* 0000 0001 1100 */
            12, 0x1D, 2432,  /* 0000 0001 1101 */
            12, 0x1E, 2496,  /* 0000 0001 1110 */
            12, 0x1F, 2560,  /* 0000 0001 1111 */

            12, 0b_000000000001, -1, // EOL

            //12, 0x1, G3CODE_EOL,  /* 0000 0000 0001 */
            //9, 0x1, G3CODE_INVALID,  /* 0000 0000 1 */
            //10, 0x1, G3CODE_INVALID,  /* 0000 0000 01 */
            //11, 0x1, G3CODE_INVALID,  /* 0000 0000 001 */
            //12, 0x0, G3CODE_INVALID,  /* 0000 0000 0000 */
        };

    private static IEnumerable<G31DFaxCode> FromArray(short[] arr)
    {
        for (int i = 0; i < arr.Length; i += 3)
        {
            yield return new G31DFaxCode((byte)arr[i], (ushort)arr[i + 1], arr[i + 2]);
        }
    }

    public static readonly G31DFaxCode[] WhiteCodes = FromArray(faxWhiteCodes).OrderBy(c => c.bitLength).ToArray();
    public static readonly G31DFaxCode[] BlackCodes = FromArray(faxBlackCodes).OrderBy(c => c.bitLength).ToArray();
}
