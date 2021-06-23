using System;
using System.Text;

namespace BulkShipper.Barcode {
    public class BarcodeGS1128 {
        public string ApplicationIdentifier { get; set; }
        public string ExtensionDigit { get; set; }
        public string GS1CompanyPrefix { get; set; }
        public string SerialNumber { get; set; }
        public string CheckDigit { get; internal set; }
        public string EncodedValue { get; internal set; }
        public string PrintableValue { get; internal set; }

        /// <summary>
        /// Encodes the barcode number as GS1128
        /// </summary>
        public void Encode() {
            int checkDigit = ChechLuhn($"{ExtensionDigit}{GS1CompanyPrefix}{SerialNumber}");
            string fullBarcode = $"{ApplicationIdentifier}{ExtensionDigit}{GS1CompanyPrefix}{SerialNumber}{checkDigit}";
            PrintableValue = $"({ApplicationIdentifier}){ExtensionDigit}{GS1CompanyPrefix}{SerialNumber}{checkDigit}";
            string currentChar;
            int currentValue;
            Char C128CheckDigit = ' ';
            StringBuilder sb = new StringBuilder();
            sb.Append((char)202);

            for (int i = 0; i < fullBarcode.Length; i += 2) {
                currentChar = fullBarcode.Substring(i, 2);
                currentValue = Int32.Parse(currentChar);

                if (currentValue < 95 && currentValue > 0) { sb.Append((char)(currentValue + 32)); }
                if (currentValue > 94) { sb.Append((char)(currentValue + 100)); }
                if (currentValue == 0) { sb.Append((char)194); }
            }

            int weight = WeightedTotal(sb.ToString());

            // Need FID?
            if (weight < 95 && weight > 0) { C128CheckDigit = (char)(weight + 32); }
            if (weight > 94) { C128CheckDigit = (char)(weight + 100); }
            if (weight == 0) { C128CheckDigit = (char)(194); }
            Console.Write(C128CheckDigit);
            sb.Append(C128CheckDigit);
            EncodedValue = $"{(char)205}{sb}{(char)206}";
        }

        /// <summary>
        /// Calculates the check sum of the barcode, using a variation of the Luhn algorithm
        /// </summary>
        /// <param name="purportedCC">The barcode</param>
        /// <returns>The check sum</returns>
        private static int ChechLuhn(string purportedCC) {
            int sum = 0;
            int parity = 0;
            int digit;
            for (int i = 0; i < purportedCC.Length; i++) {
                digit = Int32.Parse(purportedCC[i].ToString());
                if (i % 2 == parity) {
                    digit = digit * 3;
                }
                sum += digit;
            }
            if ((sum % 10) == 0) { return sum % 10; } else { return 10 - (sum % 10); }
        }

        /// <summary>
        /// Calculates the weighted total of the supplied barcode
        /// </summary>
        /// <param name="barcode">The barcode to calculate from</param>
        /// <returns>An integer that is the weighted total</returns>
        private static int WeightedTotal(string barcode) {
            int total = 105;
            int length = barcode.Length;
            int currentCharNumber;
            int currentValue = 0;
            for (int i = 1; i < length + 1; i++) {
                currentCharNumber = System.Convert.ToInt32(barcode[i - 1]);
                if (currentCharNumber < 135) { currentValue = currentCharNumber - 32; }
                if (currentCharNumber > 134) { currentValue = currentCharNumber - 100; }
                if (currentCharNumber == 194) { currentValue = 0; }
                currentValue = currentValue * i;
                total = total + currentValue;
            }
            return total % 103;
        }
    }
}
