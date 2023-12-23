namespace PsdParser
{
    public enum AdditionalLayerInformationKey : int
    {
        LayerID = 'l' << 24 | 'y' << 16 | 'i' << 8 | 'd',
        UnicodeLayerName = 'l' << 24 | 'u' << 16 | 'n' << 8 | 'i',
        SectionDividerSetting = 'l' << 24 | 's' << 16 | 'c' << 8 | 't',
        //Undocumented. 'lsdk' works the same as 'lsct'.
        //When the nesting of layer groups exceeds 6, the information is stored in 'lsdk' instead of 'lsct'
        SectionDividerSetting2 = 'l' << 24 | 's' << 16 | 'd' << 8 | 'k',

        //(**PSB**, the following keys have a length count of 8 bytes: LMsk, Lr16, Lr32, Layr, Mt16, Mt32, Mtrn, Alph, FMsk, lnk2, FEid, FXid, PxSD.
        UserMask = 'L' << 24 | 'M' << 16 | 's' << 8 | 'k',
        Lr16 = 'L' << 24 | 'r' << 16 | '1' << 8 | '6',
        Lr32 = 'L' << 24 | 'r' << 16 | '3' << 8 | '2',
        Layr = 'L' << 24 | 'a' << 16 | 'y' << 8 | 'r',
        SavingMergedTransparency16 = 'M' << 24 | 't' << 16 | '1' << 8 | '6',
        SavingMergedTransparency32 = 'M' << 24 | 't' << 16 | '3' << 8 | '2',
        SavingMergedTransparency = 'M' << 24 | 't' << 16 | 'r' << 8 | 'n',
        Alph = 'A' << 24 | 'l' << 16 | 'p' << 8 | 'h',
        FilterMask = 'F' << 24 | 'M' << 16 | 's' << 8 | 'k',
        LinkedLayer2 = 'l' << 24 | 'n' << 16 | 'k' << 8 | '2',
        FilterEffects2 = 'F' << 24 | 'E' << 16 | 'i' << 8 | 'd',
        FilterEffects = 'F' << 24 | 'X' << 16 | 'i' << 8 | 'd',
        PixelSourceData = 'P' << 24 | 'x' << 16 | 'S' << 8 | 'D',
    }
}