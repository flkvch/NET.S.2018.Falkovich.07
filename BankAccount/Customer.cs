using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccount
{
    /// <summary>
    /// Customer
    /// </summary>
    /// <seealso cref="System.IFormattable" />
    public class Customer : IFormattable
    {
        private string firstName;
        private string lastName;
        private string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="Customer"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="contactPhone">The contact phone.</param>
        /// <param name="revenue">The revenue.</param>
        public Customer(string name, string contactPhone, decimal revenue)
        {
            ValidName(name);
            ValidPhone(contactPhone);
            ValidRevenue(revenue);

            firstName = name.Substring(0, name.IndexOf(" "));
            lastName = name.Substring(name.IndexOf(" ") + 1);
            firstName = firstName.Substring(0, 1).ToUpper() + firstName.Substring(1).ToLower();
            lastName = lastName.Substring(0, 1).ToUpper() + lastName.Substring(1).ToLower();
            Name = name;
            ContactPhone = contactPhone;
            Revenue = revenue;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get
            {
                return name;
            }

            private set
            {
                name = firstName + " " + lastName;
            }
        }

        /// <summary>
        /// Gets the contact phone.
        /// </summary>
        /// <value>
        /// The contact phone.
        /// </value>
        public string ContactPhone { get; private set; }

        /// <summary>
        /// Gets the revenue.
        /// </summary>
        /// <value>
        /// The revenue.
        /// </value>
        public decimal Revenue { get; private set; }

        private void ValidRevenue(decimal revenue)
        {
            if (revenue < 0)
            {
                throw new ArgumentException($"{nameof(revenue)} should be bigger than 0");
            }
        }

        private void ValidPhone(string phone)
        {
            for (int i = 0; i < phone.Length; i++) 
            {
                if (phone[i] < 48 || phone[i] > 57)
                {
                    if (phone[i] != 43 && phone[i] != 40 && phone[i] != 41 && phone[i] != 45 && phone[i] != 32) 
                    {
                        throw new ArgumentException($"{nameof(phone)} should have only digits and +-() sign.");
                    }
                }
            }
        }

        private void ValidName(string name)
        {
            if (name.IndexOf(" ") == name.LastIndexOf(" ") && name.IndexOf(" ") != -1)
            {
                name = name.ToLower();
                for (int i = 0; i < name.Length; i++)
                {
                    if (name[i] < 97 || name[i] > 122)
                    {
                        if (name[i] != 32)
                        {
                            throw new ArgumentException("Name should have only English letters.", nameof(name));
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentException("Name should be consisted of the first name and the last name, devided by space.", nameof(name));
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"Customer record: {Name}, {ContactPhone}, {Revenue.ToString("C")}";
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        /// <exception cref="ArgumentException">format</exception>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format)) 
            {
                format = "G";
            }

            if (formatProvider == null)
            {
                formatProvider = CultureInfo.CurrentCulture;
            }

            string s1 = "Customer record: ";
            string d = ", ";
            switch (format.ToUpperInvariant())
            {
                case "G":
                    return ToString();
                case "P":
                    return s1 + ContactPhone;
                case "N":
                    return s1 + Name;
                case "R":
                    return s1 + Revenue.ToString("C", formatProvider);
                case "NP":
                    return s1 + Name + d + ContactPhone;
                case "NR":
                    return s1 + Name + d + Revenue.ToString("C", formatProvider);
                case "PR":
                    return s1 + ContactPhone + d + Revenue.ToString("C", formatProvider);
                case "RR":
                    return s1 + Revenue.ToString("F");  
                default:
                throw new ArgumentException($"There is no such { nameof(format) } format of representation of {nameof(Customer)}.");                   
            }  
        }

        /// <summary>
        /// Gets the format.
        /// </summary>
        /// <param name="formatType">Type of the format.</param>
        /// <returns></returns>
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
            {
                return this;
            }

            return null;
        }
    }
}
