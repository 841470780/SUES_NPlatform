using System.Runtime.InteropServices;

namespace NVHplatform.Interop
{
    public static class AWeightingInterop
    {
        [DllImport("AWeighting.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double ApplyAWeightingAndComputeRMS([In] double[] samples, int length);

        [DllImport("AWeighting.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double ComputeSPLA(double rms);
    }
}
