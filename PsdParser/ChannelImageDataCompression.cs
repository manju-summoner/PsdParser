namespace PsdParser
{
    public enum ChannelImageDataCompression : short
    {
        RawData = 0,
        RLE=1,
        ZipWithoutPrediction = 2,
        ZipWithPrediction = 3,
    }
}