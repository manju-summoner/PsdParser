using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsdParser
{
    public enum BlendMode : int
    {
        PassThrough  = 'p' << 24 | 'a' << 16 | 's' << 8 | 's',
        Normal       = 'n' << 24 | 'o' << 16 | 'r' << 8 | 'm',
        Dissolve     = 'd' << 24 | 'i' << 16 | 's' << 8 | 's',
        Darken       = 'd' << 24 | 'a' << 16 | 'r' << 8 | 'k',
        Multiply     = 'm' << 24 | 'u' << 16 | 'l' << 8 | ' ',
        ColorBurn    = 'i' << 24 | 'd' << 16 | 'i' << 8 | 'v',
        LinearBurn   = 'l' << 24 | 'b' << 16 | 'r' << 8 | 'n',
        DarkerColor  = 'd' << 24 | 'k' << 16 | 'C' << 8 | 'l',
        Lighten      = 'l' << 24 | 'i' << 16 | 't' << 8 | 'e',
        Screen       = 's' << 24 | 'c' << 16 | 'r' << 8 | 'n',
        ColorDodge   = 'd' << 24 | 'i' << 16 | 'v' << 8 | ' ',
        LinearDodge  = 'l' << 24 | 'd' << 16 | 'd' << 8 | 'g',
        LighterColor = 'l' << 24 | 'g' << 16 | 'C' << 8 | 'l',
        Overlay      = 'o' << 24 | 'v' << 16 | 'e' << 8 | 'r',
        SoftLight    = 's' << 24 | 'L' << 16 | 'i' << 8 | 't',
        HardLight    = 'h' << 24 | 'L' << 16 | 'i' << 8 | 't',
        VividLight   = 'v' << 24 | 'L' << 16 | 'i' << 8 | 't',
        LinearLight  = 'l' << 24 | 'L' << 16 | 'i' << 8 | 't',
        PinLight     = 'p' << 24 | 'L' << 16 | 'i' << 8 | 't',
        HardMix      = 'h' << 24 | 'M' << 16 | 'i' << 8 | 'x',
        Difference   = 'd' << 24 | 'i' << 16 | 'f' << 8 | 'f',
        Exclusion    = 's' << 24 | 'm' << 16 | 'u' << 8 | 'd',
        Subtract     = 'f' << 24 | 's' << 16 | 'u' << 8 | 'b',
        Divide       = 'f' << 24 | 'd' << 16 | 'i' << 8 | 'v',
        Hue          = 'h' << 24 | 'u' << 16 | 'e' << 8 | ' ',
        Saturation   = 's' << 24 | 'a' << 16 | 't' << 8 | ' ',
        Color        = 'c' << 24 | 'o' << 16 | 'l' << 8 | 'r',
        Luminosity   = 'l' << 24 | 'u' << 16 | 'm' << 8 | ' ',
    }
}
