namespace PsdParser
{
    public enum ImageResourceBlockId : short
    {
        ///<summary>
        ///0x03E8 1000 (Obsolete--Photoshop 2.0 only ) Contains five 2-byte values: number of channels, rows, columns, depth, and mode
        ///</summary>
        ChannelsRowsColumnsDepthAndMode = 0x03E8,
        ///<summary>
        ///0x03E9 1001 Macintosh print manager print info record
        ///</summary>
        MacintoshPrintManagerPrintInfoRecord = 0x03E9,
        ///<summary>
        ///0x03EA 1002 Macintosh page format information. No longer read by Photoshop. (Obsolete)
        ///</summary>
        MacintoshPageFormatInformation = 0x03EA,
        ///<summary>
        ///0x03EB 1003 (Obsolete--Photoshop 2.0 only ) Indexed color table
        ///</summary>
        IndexedColorTable = 0x03EB,
        ///<summary>
        ///0x03ED 1005 ResolutionInfo structure. See Appendix A in Photoshop API Guide.pdf.
        ///</summary>
        ResolutionInfo = 0x03ED,
        ///<summary>
        ///0x03EE 1006 Names of the alpha channels as a series of Pascal strings.
        ///</summary>
        AlphaChannelNames = 0x03EE,
        ///<summary>
        ///0x03EF 1007 (Obsolete) See ID 1077DisplayInfo structure. See Appendix A in Photoshop API Guide.pdf.
        ///</summary>
        DisplayInfo = 0x03EF,
        ///<summary>
        ///0x03F0 1008 The caption as a Pascal string.
        ///</summary>
        Caption = 0x03F0,
        ///<summary>
        ///0x03F1 1009 Border information. Contains a fixed number (2 bytes real, 2 bytes fraction) for the border width, and 2 bytes for border units (1 = inches, 2 = cm, 3 = points, 4 = picas, 5 = columns).
        ///</summary>
        BorderInformation = 0x03F1,
        ///<summary>
        ///0x03F2 1010 Background color. See See Color structure.
        ///</summary>
        BackgroundColor = 0x03F2,
        ///<summary>
        ///0x03F3 1011 Print flags. A series of one-byte boolean values (see Page Setup dialog): labels, crop marks, color bars, registration marks, negative, flip, interpolate, caption, print flags.
        ///</summary>
        PrintFlags = 0x03F3,
        ///<summary>
        ///0x03F4 1012 Grayscale and multichannel halftoning information
        ///</summary>
        GrayscaleAndMultichannelHalftoningInformation = 0x03F4,
        ///<summary>
        ///0x03F5 1013 Color halftoning information
        ///</summary>
        ColorHalftoningInformation = 0x03F5,
        ///<summary>
        ///0x03F6 1014 Duotone halftoning information
        ///</summary>
        DuotoneHalftoningInformation = 0x03F6,
        ///<summary>
        ///0x03F7 1015 Grayscale and multichannel transfer function
        ///</summary>
        GrayscaleAndMultichannelTransferFunction = 0x03F7,
        ///<summary>
        ///0x03F8 1016 Color transfer functions
        ///</summary>
        ColorTransferFunctions = 0x03F8,
        ///<summary>
        ///0x03F9 1017 Duotone transfer functions
        ///</summary>
        DuotoneTransferFunctions = 0x03F9,
        ///<summary>
        ///0x03FA 1018 Duotone image information
        ///</summary>
        DuotoneImageInformation = 0x03FA,
        ///<summary>
        ///0x03FB 1019 Two bytes for the effective black and white values for the dot range
        ///</summary>
        EffectiveBlackAndWhiteValuesForTheDotRange = 0x03FB,
        ///<summary>
        ///0x03FC 1020 (Obsolete)
        ///</summary>
        Obsolete = 0x03FC,
        ///<summary>
        ///0x03FD 1021 EPS options
        ///</summary>
        EpsOptions = 0x03FD,
        ///<summary>
        ///0x03FE 1022 Quick Mask information. 2 bytes containing Quick Mask channel ID; 1- byte boolean indicating whether the mask was initially empty.
        ///</summary>
        QuickMaskInformation = 0x03FE,
        ///<summary>
        ///0x03FF 1023 (Obsolete)
        ///</summary>
        Obsolete2 = 0x03FF,
        ///<summary>
        ///0x0400 1024 Layer state information. 2 bytes containing the index of target layer (0 = bottom layer).
        ///</summary>
        LayerStateInformation = 0x0400,
        ///<summary>
        ///0x0401 1025 Working path (not saved). See See Path resource format.
        ///</summary>
        WorkingPath = 0x0401,
        ///<summary>
        ///0x0402 1026 Layers group information. 2 bytes per layer containing a group ID for the dragging groups. Layers in a group have the same group ID.
        ///</summary>
        LayersGroupInformation = 0x0402,
        ///<summary>
        ///0x0403 1027 (Obsolete)
        ///</summary>
        Obsolete3 = 0x0403,
        ///<summary>
        ///0x0404 1028 IPTC-NAA record. Contains the File Info... information. See the documentation in the IPTC folder of the Documentation folder.
        ///</summary>
        IptcNaaRecord = 0x0404,
        ///<summary>
        ///0x0405 1029 Image mode for raw format files
        ///</summary>
        ImageModeForRawFormatFiles = 0x0405,
        ///<summary>
        ///0x0406 1030 JPEG quality. Private.
        ///</summary>
        JpegQuality = 0x0406,
        ///<summary>
        ///0x0408 1032 (Photoshop 4.0) Grid and guides information. See See Grid and guides resource format.
        ///</summary>
        GridAndGuidesInformation = 0x0408,
        ///<summary>
        ///0x0409 1033 (Photoshop 4.0) Thumbnail resource for Photoshop 4.0 only. See See Thumbnail resource format.
        ///</summary>
        ThumbnailResource = 0x0409,
        ///<summary>
        ///0x040A 1034 (Photoshop 4.0) Copyright flag. Boolean indicating whether image is copyrighted. Can be set via Property suite or by user in File Info...
        ///</summary>
        CopyrightFlag = 0x040A,
        ///<summary>
        ///0x040B 1035 (Photoshop 4.0) URL. Handle of a text string with uniform resource locator. Can be set via Property suite or by user in File Info...
        ///</summary>
        Url = 0x040B,
        ///<summary>
        ///0x040C 1036 (Photoshop 5.0) Thumbnail resource (supersedes resource 1033). See See Thumbnail resource format.
        ///</summary>
        ThumbnailResource2 = 0x040C,
        ///<summary>
        ///0x040D 1037 (Photoshop 5.0) Global Angle. 4 bytes that contain an integer between 0 and 359, which is the global lighting angle for effects layer. If not present, assumed to be 30.
        ///</summary>
        GlobalAngle = 0x040D,
        ///<summary>
        ///0x040E 1038 (Obsolete) See ID 1073 below. (Photoshop 5.0) Color samplers resource. See See Color samplers resource format.
        ///</summary>
        ColorSamplersResource = 0x040E,
        ///<summary>
        ///0x040F 1039 (Photoshop 5.0) ICC Profile. The raw bytes of an ICC (International Color Consortium) format profile. See ICC1v42_2006-05.pdf in the Documentation folder and icProfileHeader.h in Sample Code\Common\Includes .
        ///</summary>
        IccProfile = 0x040F,
        ///<summary>
        ///0x0410 1040 (Photoshop 5.0) Watermark. One byte.
        ///</summary>
        Watermark = 0x0410,
        ///<summary>
        ///0x0411 1041 (Photoshop 5.0) ICC Untagged Profile. 1 byte that disables any assumed profile handling when opening the file. 1 = intentionally untagged.
        ///</summary>
        IccUntaggedProfile = 0x0411,
        ///<summary>
        ///0x0412 1042 (Photoshop 5.0) Effects visible. 1-byte global flag to show/hide all the effects layer. Only present when they are hidden.
        ///</summary>
        EffectsVisible = 0x0412,
        ///<summary>
        ///0x0413 1043 (Photoshop 5.0) Spot Halftone. 4 bytes for version, 4 bytes for length, and the variable length data.
        ///</summary>
        SpotHalftone = 0x0413,
        ///<summary>
        ///0x0414 1044 (Photoshop 5.0) Document-specific IDs seed number. 4 bytes: Base value, starting at which layer IDs will be generated (or a greater value if existing IDs already exceed it). Its purpose is to avoid the case where we add layers, flatten, save, open, and then add more layers that end up with the same IDs as the first set.
        ///</summary>
        DocumentSpecificIdsSeedNumber = 0x0414,
        ///<summary>
        ///0x0415 1045 (Photoshop 5.0) Unicode Alpha Names. Unicode string
        ///</summary>
        UnicodeAlphaNames = 0x0415,
        ///<summary>
        ///0x0416 1046 (Photoshop 6.0) Indexed Color Table Count. 2 bytes for the number of colors in table that are actually defined
        ///</summary>
        IndexedColorTableCount = 0x0416,
        ///<summary>
        ///0x0417 1047 (Photoshop 6.0) Transparency Index. 2 bytes for the index of transparent color, if any.
        ///</summary>
        TransparencyIndex = 0x0417,
        ///<summary>
        ///0x0419 1049 (Photoshop 6.0) Global Altitude. 4 byte entry for altitude
        ///</summary>
        GlobalAltitude = 0x0419,
        ///<summary>
        ///0x041A 1050 (Photoshop 6.0) Slices. See See Slices resource format.
        ///</summary>
        Slices = 0x041A,
        ///<summary>
        ///0x041B 1051 (Photoshop 6.0) Workflow URL. Unicode string
        ///</summary>
        WorkflowUrl = 0x041B,
        ///<summary>
        ///0x041C 1052 (Photoshop 6.0) Jump To XPEP. 2 bytes major version, 2 bytes minor version, 4 bytes count. Following is repeated for count: 4 bytes block size, 4 bytes key, if key = 'jtDd' , then next is a Boolean for the dirty flag; otherwise it's a 4 byte entry for the mod date.
        ///</summary>
        JumpToXpep = 0x041C,
        ///<summary>
        ///0x041D 1053 (Photoshop 6.0) Alpha Identifiers. 4 bytes of length, followed by 4 bytes each for every alpha identifier.
        ///</summary>
        AlphaIdentifiers = 0x041D,
        ///<summary>
        ///0x041E 1054 (Photoshop 6.0) URL List. 4 byte count of URLs, followed by 4 byte long, 4 byte ID, and Unicode string for each count.
        ///</summary>
        UrlList = 0x041E,
        ///<summary>
        ///0x0421 1057 (Photoshop 6.0) Version Info. 4 bytes version, 1 byte hasRealMergedData , Unicode string: writer name, Unicode string: reader name, 4 bytes file version.
        ///</summary>
        VersionInfo = 0x0421,
        ///<summary>
        ///0x0422 1058 (Photoshop 7.0) EXIF data 1. See http://www.kodak.com/global/plugins/acrobat/en/service/digCam/exifStandard2.pdf
        ///</summary>
        ExifData1 = 0x0422,
        ///<summary>
        ///0x0423 1059 (Photoshop 7.0) EXIF data 3. See http://www.kodak.com/global/plugins/acrobat/en/service/digCam/exifStandard2.pdf
        ///</summary>
        ExifData3 = 0x0423,
        ///<summary>
        ///0x0424 1060 (Photoshop 7.0) XMP metadata. File info as XML description. See http://www.adobe.com/devnet/xmp/
        ///</summary>
        XmpMetadata = 0x0424,
        ///<summary>
        ///0x0425 1061 (Photoshop 7.0) Caption digest. 16 bytes: RSA Data Security, MD5 message-digest algorithm
        ///</summary>
        CaptionDigest = 0x0425,
        ///<summary>
        ///0x0426 1062 (Photoshop 7.0) Print scale. 2 bytes style (0 = centered, 1 = size to fit, 2 = user defined). 4 bytes x location (floating point). 4 bytes y location (floating point). 4 bytes scale (floating point)
        ///</summary>
        PrintScale = 0x0426,
        ///<summary>
        ///0x0428 1064 (Photoshop CS) Pixel Aspect Ratio. 4 bytes (version = 1 or 2), 8 bytes double, x / y of a pixel. Version 2, attempting to correct values for NTSC and PAL, previously off by a factor of approx. 5%.
        ///</summary>
        PixelAspectRatio = 0x0428,
        ///<summary>
        ///0x0429 1065 (Photoshop CS) Layer Comps. 4 bytes (descriptor version = 16), Descriptor (see See Descriptor structure)
        ///</summary>
        LayerComps = 0x0429,
        ///<summary>
        ///0x042A 1066 (Photoshop CS) Alternate Duotone Colors. 2 bytes (version = 1), 2 bytes count, following is repeated for each count: [ Color: 2 bytes for space followed by 4 * 2 byte color component ], following this is another 2 byte count, usually 256, followed by Lab colors one byte each for L, a, b. This resource is not read or used by Photoshop.
        ///</summary>
        AlternateDuotoneColors = 0x042A,
        ///<summary>
        ///0x042B 1067 (Photoshop CS)Alternate Spot Colors. 2 bytes (version = 1), 2 bytes channel count, following is repeated for each count: 4 bytes channel ID, Color: 2 bytes for space followed by 4 * 2 byte color component. This resource is not read or used by Photoshop.
        ///</summary>
        AlternateSpotColors = 0x042B,
        ///<summary>
        ///0x042D 1069 (Photoshop CS2) Layer Selection ID(s). 2 bytes count, following is repeated for each count: 4 bytes layer ID
        ///</summary>
        LayerSelectionIds = 0x042D,
        ///<summary>
        ///0x042E 1070 (Photoshop CS2) HDR Toning information
        ///</summary>
        HdrToningInformation = 0x042E,
        ///<summary>
        ///0x042F 1071 (Photoshop CS2) Print info
        ///</summary>
        PrintInfo = 0x042F,
        ///<summary>
        ///0x0430 1072 (Photoshop CS2) Layer Group(s) Enabled ID. 1 byte for each layer in the document, repeated by length of the resource. NOTE: Layer groups have start and end markers
        ///</summary>
        LayerGroupsEnabledId = 0x0430,
        ///<summary>
        ///0x0431 1073 (Photoshop CS3) Color samplers resource. Also see ID 1038 for old format. See See Color samplers resource format.
        ///</summary>
        ColorSamplersResource2 = 0x0431,
        ///<summary>
        ///0x0432 1074 (Photoshop CS3) Measurement Scale. 4 bytes (descriptor version = 16), Descriptor (see See Descriptor structure)
        ///</summary>
        MeasurementScale = 0x0432,
        ///<summary>
        ///0x0433 1075 (Photoshop CS3) Timeline Information. 4 bytes (descriptor version = 16), Descriptor (see See Descriptor structure)
        ///</summary>
        TimelineInformation = 0x0433,
        ///<summary>
        ///0x0434 1076 (Photoshop CS3) Sheet Disclosure. 4 bytes (descriptor version = 16), Descriptor (see See Descriptor structure)
        ///</summary>
        SheetDisclosure = 0x0434,
        ///<summary>
        ///0x0435 1077 (Photoshop CS3) DisplayInfo structure to support floating point clors. Also see ID 1007. See Appendix A in Photoshop API Guide.pdf .
        ///</summary>
        DisplayInfo2 = 0x0435,
        ///<summary>
        ///0x0436 1078 (Photoshop CS3) Onion Skins. 4 bytes (descriptor version = 16), Descriptor (see See Descriptor structure)
        ///</summary>
        OnionSkins = 0x0436,
        ///<summary>
        ///0x0438 1080 (Photoshop CS4) Count Information. 4 bytes (descriptor version = 16), Descriptor (see See Descriptor structure) Information about the count in the document. See the Count Tool.
        ///</summary>
        CountInformation = 0x0438,
        ///<summary>
        ///0x043A 1082 (Photoshop CS5) Print Information. 4 bytes (descriptor version = 16), Descriptor (see See Descriptor structure) Information about the current print settings in the document. The color management options.
        ///</summary>
        PrintInformation = 0x043A,
        ///<summary>
        ///0x043B 1083 (Photoshop CS5) Print Style. 4 bytes (descriptor version = 16), Descriptor (see See Descriptor structure) Information about the current print style in the document. The printing marks, labels, ornaments, etc.
        ///</summary>
        PrintStyle = 0x043B,
        ///<summary>
        ///0x043C 1084 (Photoshop CS5) Macintosh NSPrintInfo. Variable OS specific info for Macintosh. NSPrintInfo. It is recommened that you do not interpret or use this data.
        ///</summary>
        MacintoshNsPrintInfo = 0x043C,
        ///<summary>
        ///0x043D 1085 (Photoshop CS5) Windows DEVMODE. Variable OS specific info for Windows. DEVMODE. It is recommened that you do not interpret or use this data.
        ///</summary>
        WindowsDevMode = 0x043D,
        ///<summary>
        ///0x043E 1086 (Photoshop CS6) Auto Save File Path. Unicode string. It is recommened that you do not interpret or use this data.
        ///</summary>
        AutoSaveFilePath = 0x043E,
        ///<summary>
        ///0x043F 1087 (Photoshop CS6) Auto Save Format. Unicode string. It is recommened that you do not interpret or use this data.
        ///</summary>
        AutoSaveFormat = 0x043F,
        ///<summary>
        ///0x0440 1088 (Photoshop CC) Path Selection State. 4 bytes (descriptor version = 16), Descriptor (see See Descriptor structure) Information about the current path selection state.
        ///</summary>
        PathSelectionState = 0x0440,
        ///<summary>
        ///0x07D0-0x0BB6 2000-2997 Path Information (saved paths). See See Path resource format.
        ///</summary>
        PathInformation = 0x07D0,
        ///<summary>
        ///0x0BB7 2999 Name of clipping path. See See Path resource format.
        ///</summary>
        NameOfClippingPath = 0x0BB7,
        ///<summary>
        ///0x0BB8 3000 (Photoshop CC) Origin Path Info. 4 bytes (descriptor version = 16), Descriptor (see See Descriptor structure) Information about the origin path data.
        ///</summary>
        OriginPathInfo = 0x0BB8,
        ///<summary>
        ///0x0FA0-0x1387 4000-4999 Plug-In resource(s). Resources added by a plug-in. See the plug-in API found in the SDK documentation
        ///</summary>
        PluginResources = 0x0FA0,
        ///<summary>
        ///0x1B58 7000 Image Ready variables. XML representation of variables definition
        ///</summary>
        ImageReadyVariables = 0x1B58,
        ///<summary>
        ///0x1B59 7001 Image Ready data sets
        ///</summary>
        ImageReadyDataSets = 0x1B59,
        ///<summary>
        ///0x1B5A 7002 Image Ready default selected state
        ///</summary>
        ImageReadyDefaultSelectedState = 0x1B5A,
        ///<summary>
        ///0x1B5B 7003 Image Ready 7 rollover expanded state
        ///</summary>
        ImageReady7RolloverExpandedState = 0x1B5B,
        ///<summary>
        ///0x1B5C 7004 Image Ready rollover expanded state
        ///</summary>
        ImageReadyRolloverExpandedState = 0x1B5C,
        ///<summary>
        ///0x1B5D 7005 Image Ready save layer settings
        ///</summary>
        ImageReadySaveLayerSettings = 0x1B5D,
        ///<summary>
        ///0x1B5E 7006 Image Ready version
        ///</summary>
        ImageReadyVersion = 0x1B5E,
        ///<summary>
        ///0x1F40 8000 (Photoshop CS3) Lightroom workflow, if present the document is in the middle of a Lightroom workflow.
        ///</summary>
        LightroomWorkflow = 0x1F40,
        ///<summary>
        ///0x2710 10000 Print flags information. 2 bytes version ( = 1), 1 byte center crop marks, 1 byte ( = 0), 4 bytes bleed width value, 2 bytes bleed width scale.
        ///</summary>
        PrintFlagsInformation = 0x2710

    }
}