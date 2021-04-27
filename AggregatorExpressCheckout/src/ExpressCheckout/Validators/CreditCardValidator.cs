using System;
using System.Collections;

namespace ExpressCheckout.Validators
{
    public class CreditCardValidator
    {
        /// <summary>
        /// Performs a validation using Luhn's Formula.
        /// </summary>
        public static bool ValidateCardNumber(string cardNumber)
        {
            try
            {
                // Array to contain individual numbers
                System.Collections.ArrayList CheckNumbers = new ArrayList();

                // So, get length of card
                int CardLength = cardNumber.Length;

                // Double the value of alternate digits, starting with the second digit
                // from the right, i.e. back to front.

                // Loop through starting at the end
                for (int i = CardLength - 2; i >= 0; i = i - 2)
                {
                    // Now read the contents at each index, this
                    // can then be stored as an array of integers

                    // Double the number returned
                    CheckNumbers.Add(Int32.Parse(cardNumber[i].ToString()) * 2);
                }

                int CheckSum = 0;	// Will hold the total sum of all checksum digits

                // Second stage, add separate digits of all products
                for (int iCount = 0; iCount <= CheckNumbers.Count - 1; iCount++)
                {
                    int _count = 0;	// will hold the sum of the digits

                    // determine if current number has more than one digit
                    if ((int)CheckNumbers[iCount] > 9)
                    {
                        int _numLength = ((int)CheckNumbers[iCount]).ToString().Length;
                        // add count to each digit
                        for (int x = 0; x < _numLength; x++)
                        {
                            _count = _count + Int32.Parse(((int)CheckNumbers[iCount]).ToString()[x].ToString());
                        }
                    }
                    else
                    {
                        _count = (int)CheckNumbers[iCount];	// single digit, just add it by itself
                    }

                    CheckSum = CheckSum + _count;	// add sum to the total sum
                }

                // Stage 3, add the unaffected digits
                // Add all the digits that we didn't double still starting from the right
                // but this time we'll start from the rightmost number with alternating digits
                int OriginalSum = 0;
                for (int y = CardLength - 1; y >= 0; y = y - 2)
                {
                    OriginalSum = OriginalSum + Int32.Parse(cardNumber[y].ToString());
                }

                // Perform the final calculation, if the sum Mod 10 results in 0 then
                // it's valid, otherwise its false.
                return (((OriginalSum + CheckSum) % 10) == 0);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Method to check Card Number Length based on Card Type
        /// </summary>
        /// <param name="cardNumber">
        /// Credir Card Number
        /// </param>
        /// <returns>
        /// true if Card Length is Valid
        /// </returns>
        public static bool IsValidCardTypeCheckByCardLength(string cardNumber)
        {
            try
            {
                if (cardNumber.Length < 12 || cardNumber.Length > 19)
                {
                    return false;
                }
                // AMEX -- 34 or 37 -- 15 length
                //if ((Regex.IsMatch(cardNumber, "^(34|37)")))
                //{
                //    return true;
                //    //(15 == cardNumber.Length);
                //}

                //// MasterCard -- 51 through 55 -- 16 length
                //else if ((Regex.IsMatch(cardNumber, "^(51|52|53|54|55)")))
                //{
                //    return true;// (16 == cardNumber.Length);
                //}

                //// VISA -- 4 -- 13 and 16 length
                //else if ((Regex.IsMatch(cardNumber, "^(4)")))
                //    return true;//(13 == cardNumber.Length || 16 == cardNumber.Length);

                //// Diners Club -- 300-305, 36 or 38 -- 14 length
                //else if ((Regex.IsMatch(cardNumber, "^(300|301|302|303|304|305|36|38)")))
                //    return true;//(14 == cardNumber.Length);

                //// enRoute -- 2014,2149 -- 15 length
                //else if ((Regex.IsMatch(cardNumber, "^(2014|2149)")))
                //    return true;//(15 == cardNumber.Length);

                //// Discover -- 6011 -- 16 length
                //else if ((Regex.IsMatch(cardNumber, "^(6011)")))
                //    return true;//(16 == cardNumber.Length);

                //// JCB -- 3 -- 16 length
                //else if ((Regex.IsMatch(cardNumber, "^(3)")))
                //    return true;//(16 == cardNumber.Length);

                //// JCB -- 2131, 1800 -- 15 length
                //else if ((Regex.IsMatch(cardNumber, "^(2131|1800)")))
                //    return true;//(15 == cardNumber.Length);
                //else
                //{
                //    // Card type wasn't recognised  return false.
                //    return false;
                //}
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}