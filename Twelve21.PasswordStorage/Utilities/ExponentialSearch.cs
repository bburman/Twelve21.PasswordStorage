using System;

namespace Twelve21.PasswordStorage.Utilities
{
    public class ExponentialSearch
    {
        #region Fields
        private int _lowerBounds;
        private int _upperBounds;
        private Func<int, ExponentialSearchComparison> _compareTo;
        #endregion

        #region Construction
        public ExponentialSearch(Func<int, ExponentialSearchComparison> compareTo)
            : this(1, int.MaxValue, compareTo)
        { }

        public ExponentialSearch(int lowerBounds, int upperBounds, Func<int, ExponentialSearchComparison> compareTo)
        {
            if (compareTo == null)
                throw new ArgumentNullException(nameof(compareTo));
            if (lowerBounds > upperBounds)
                throw new ArgumentOutOfRangeException(nameof(lowerBounds));

            _lowerBounds = lowerBounds;
            _upperBounds = upperBounds;
            _compareTo = compareTo;
        }
        #endregion

        #region Methods
        public int Search()
        {
            int maximum = _lowerBounds;
            while (maximum < _upperBounds && _compareTo(maximum) != ExponentialSearchComparison.ToHigh)
                maximum *= 2;

            return BinarySearch(maximum / 2, Math.Min(maximum, _upperBounds));
        }

        private int BinarySearch(int minimum, int maximum)
        {
            if (maximum >= minimum && minimum >= _lowerBounds)
            {
                int mid = minimum + (maximum - minimum) / 2;

                switch (_compareTo(mid))
                {
                    case ExponentialSearchComparison.ToLow:
                        return BinarySearch(mid + 1, maximum);
                    case ExponentialSearchComparison.Equal:
                        return mid;
                    case ExponentialSearchComparison.ToHigh:
                        return BinarySearch(minimum, mid - 1);
                }
            }

            return -1;
        }
        #endregion
    }
}
