using System;
using System.ComponentModel;

namespace VernamCipherTool.Enums
{
    public enum Operation
    {
        [Description("Encoded:")]
        Encode,

        [Description("Decoded:")]
        Decode
    }
}
