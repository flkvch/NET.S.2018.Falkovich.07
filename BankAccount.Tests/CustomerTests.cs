using NUnit.Framework;
using System;
using System.Globalization;
using BankAccountExtend;

namespace BankAccount.Tests
{
    class CustomerTests
    {
        [TestCase(ExpectedResult = "Customer record: Ksenia Falkovich, +375 (29) 777-7777, 11 234,00 ₽")]
        public string ToString_Override()
        {
            return new Customer("KsEnIa falkovich", "+375 (29) 777-7777", 11234M).ToString();
        }

        [TestCase("G", ExpectedResult = "Customer record: Ksenia Falkovich, +375 (29) 777-7777, 11 234,01 ₽")]
        [TestCase("P", ExpectedResult = "Customer record: +375 (29) 777-7777")]
        [TestCase("N", ExpectedResult = "Customer record: Ksenia Falkovich")]
        [TestCase("R", ExpectedResult = "Customer record: 11 234,01 ₽")]
        [TestCase("NP", ExpectedResult = "Customer record: Ksenia Falkovich, +375 (29) 777-7777")]
        [TestCase("NR", ExpectedResult = "Customer record: Ksenia Falkovich, 11 234,01 ₽")]
        [TestCase("PR", ExpectedResult = "Customer record: +375 (29) 777-7777, 11 234,01 ₽")]
        [TestCase("RR", ExpectedResult = "Customer record: 11234,01")]
        public string ToString_Formatted(string format)
        {
            return new Customer("Ksenia fALkovich", "+375 (29) 777-7777", 11234.0089M).ToString($"{format}");
        }

        [TestCase("P", "en-US", ExpectedResult = "Customer record: +375 (29) 777-7777")]
        [TestCase("N", "en-US", ExpectedResult = "Customer record: Ksenia Falkovich")]
        [TestCase("R", "en-US", ExpectedResult = "Customer record: $11,234.01")]
        [TestCase("R", "fr-FR", ExpectedResult = "Customer record: 11 234,01 €")]
        [TestCase("NP", "en-US", ExpectedResult = "Customer record: Ksenia Falkovich, +375 (29) 777-7777")]
        [TestCase("NR", "en-US", ExpectedResult = "Customer record: Ksenia Falkovich, $11,234.01")]
        [TestCase("PR", "en-US", ExpectedResult = "Customer record: +375 (29) 777-7777, $11,234.01")]
        [TestCase("RR", "en-US", ExpectedResult = "Customer record: 11234,01")]
        [TestCase("", "en-US", ExpectedResult = "Customer record: Ksenia Falkovich, +375 (29) 777-7777, 11 234,01 ₽")]
        [TestCase(null, "en-US", ExpectedResult = "Customer record: Ksenia Falkovich, +375 (29) 777-7777, 11 234,01 ₽")]
        public string ToString_Formatted_Culture(string format, string provider)
        {
            return new Customer("Ksenia FalkoVich", "+375 (29) 777-7777", 11234.0089M).ToString($"{format}", new CultureInfo(provider));
        }

        [TestCase("RY", "en-US")]
        [TestCase("NER", "ru-RU")]
        public void ToString_Formatted_Exceptions(string format, string provider) =>
        Assert.Throws<ArgumentException>(() =>
       new Customer("Ksenia FalkoVich", "+375 (29) 777-7777", 11234.0089M).ToString($"{format}", new CultureInfo(provider)));

        [TestCase("RM", "en-US", ExpectedResult = "Customer record: $936.17 per month")]
        [TestCase("RY", "en-US", ExpectedResult = "Customer record: $11,234.01 per year")]
        [TestCase("RD", "fr-FR", ExpectedResult = "Customer record: 30,78 € per day")]
        public string ToString_Formatted_CustomerFormatProvider(string format, string provider)
        {
            CultureInfo ci = new CultureInfo(provider);
            CustomerFormatProvider formatprovider = new CustomerFormatProvider();
            Customer c1 = new Customer("Ksenia FalkoVich", "+375 (29) 777-7777", 11234.0089M);
            return formatprovider.Format(format, c1, ci);
        }

        [TestCase("RU", "en-US")]
        [TestCase("NER", "ru-RU")]
        public void ToString_FormattedCustom_Exceptions(string format, string provider) =>
        Assert.Throws<ArgumentException>(() => 
        new CustomerFormatProvider().Format(format, new Customer("Ksenia FalkoVich", "+375 (29) 777-7777", 11234.0089M), new CultureInfo(provider)));
    }
}
