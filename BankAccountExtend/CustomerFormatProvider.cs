using System;
using System.Globalization;
using BankAccount;

namespace BankAccountExtend
{
    public class CustomerFormatProvider : IFormatProvider, ICustomFormatter
    {
        private IFormatProvider _parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerFormatProvider"/> class.
        /// </summary>
        public CustomerFormatProvider()
        {
            _parent = CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerFormatProvider"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public CustomerFormatProvider(IFormatProvider parent)
        {
            _parent = parent;
        }

        /// <summary>
        /// Returns an object that provides formatting services for the specified type.
        /// </summary>
        /// <param name="formatType">An object that specifies the type of format object to return.</param>
        /// <returns>
        /// An instance of the object specified by <paramref name="formatType" />, if the <see cref="T:System.IFormatProvider" /> implementation can supply that type of object; otherwise, null.
        /// </returns>
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
            {
                return this;
            }

            return null;
        }

        /// <summary>
        /// Formats the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="argument">The argument.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">argument</exception>
        /// <exception cref="ArgumentException">
        /// format
        /// </exception>
        public string Format(string format, object argument, IFormatProvider formatProvider)
        {
            if (argument is null)
            {
                throw new ArgumentNullException($"{nameof(argument)} shouldn' t be null.");
            }

            if (argument.GetType() != typeof(Customer))
            {
                throw new ArgumentException($"The format of '{format}' is invalid.");
            }

            if (string.IsNullOrEmpty(format))
            {
                format = "G";
            }

            if (formatProvider is null)
            {
                formatProvider = _parent;
            }

            Customer customer = (Customer)argument;
            string s1 = "Customer record: ";
            switch (format.ToUpperInvariant())
            {
                case "RY":
                    {
                        return s1 + customer.Revenue.ToString("C", formatProvider) + " per year";
                    }

                case "RM":
                    {
                        return s1 + (customer.Revenue / 12).ToString("C", formatProvider) + " per month";
                    }

                case "RD":
                    {
                        return s1 + (customer.Revenue / 365).ToString("C", formatProvider) + " per day";
                    }


                default:
                    throw new ArgumentException($"There is no such { nameof(format) } format of representation of {nameof(Customer)}.");
            }
        }
    }
}
