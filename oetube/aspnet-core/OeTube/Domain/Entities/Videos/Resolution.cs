using Volo.Abp.Domain.Values;

namespace OeTube.Domain.Entities.Videos
{
    public class Resolution : ValueObject
    {
        public static readonly Resolution Zero = new(0, 0);
        private const char DefaultSeparator = 'x';

        public static bool TryParse(string input, out Resolution resolution, char separator = DefaultSeparator)
        {
            string[] parts = input.Split(separator);
            resolution = Zero;
            if (parts.Length == 2 && int.TryParse(parts[0], out int width)
                && int.TryParse(parts[1], out int height))
            {
                resolution = new Resolution(width, height);
                return true;
            }
            else return false;
        }

        public static Resolution Parse(string input, char separator = DefaultSeparator)
        {
            if (!TryParse(input, out Resolution resolution, separator))
            {
                throw new ArgumentException(input);
            }
            return resolution;
        }

        public static readonly Resolution SD = new(720, 480);
        public static readonly Resolution HD = new(1280, 720);
        public static readonly Resolution FHD = new(1920, 1080);

        public static bool operator ==(Resolution left, Resolution right)
        {
            return left.Width == right.Width && left.Height == right.Height;
        }

        public static bool operator !=(Resolution left, Resolution right)
        {
            return !(left == right);
        }

        public static double operator /(Resolution left, Resolution right)
        {
            double leftArea = left.Width * left.Height;
            double rightArea = right.Width * right.Height;
            return leftArea / rightArea;
        }

        public Resolution(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; }
        public int Height { get; }

        public override string ToString()
        {
            return ToString(DefaultSeparator);
        }

        public string ToString(char separator)
        {
            return $"{Width}{separator}{Height}";
        }

        public override bool Equals(object? obj)
        {
            return obj is Resolution resolution &&
                   Width == resolution.Width &&
                   Height == resolution.Height;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Width, Height);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Width;
            yield return Height;
        }
    }
}