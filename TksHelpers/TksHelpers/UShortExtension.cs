namespace TksHelpers
{
    public static class UShortExtension
    {
        public static Bits AsBits(this ushort value)
        {
            var bits = new Bits();
            bits.FillFromUShort(value);
            return bits;
        }
    }
}