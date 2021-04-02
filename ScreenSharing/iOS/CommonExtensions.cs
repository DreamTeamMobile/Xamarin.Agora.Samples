using CoreGraphics;

namespace DT.Samples.Agora.ScreenSharing.iOS
{
    public static class CommonExtensions
    {
        public static CGSize FixedSize(this CGSize size, CGSize other)
        {
            if (other.Width > other.Height)
            {
                return FixedLandscapeSize(size);
            }
            else
            {
                return FixedPortraitSize(size);
            }
        }

        public static CGSize FixedLandscapeSize(this CGSize size)
        {
            if (size.Width < size.Height)
            {
                return new CGSize(size.Height, size.Width);
            }
            else
            {
                return size;
            }
        }

        public static CGSize FixedPortraitSize(this CGSize size)
        {
            if (size.Width > size.Height)
            {
                return new CGSize(size.Height, size.Width);
            }
            else
            {
                return size;
            }
        }
    }
}
