namespace PsdParser
{
    public enum ChannelId : short
    {
        RealUserSuppliedLayerMask = -3,
        UserSuppliedLayerMask = -2,
        TransparencyMask = -1,

        R = 0,
        G = 1,
        B = 2,

        C = R,
        M = G,
        Y = B,
        K = 3,
    }
}