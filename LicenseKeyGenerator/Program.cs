using System;
using System.IO;
using Utilities.EncryptDecryptUtil;

namespace LicenseKeyGenerator
{
    internal class Program
    {
        private static void Main()
        {
            try
            {
                Console.Write(value: "Enter Mac Address: ");
                var macAddressInput = Console.ReadLine();
                if (macAddressInput != null)
                {
                    macAddressInput = macAddressInput.Replace(oldValue: "-",
                                                              newValue: "");

                    Console.Write(value: "Enter License Expiry Date{format:dd-mm-yyy}: ");
                    var expiryDateInput = Console.ReadLine();
                    if (expiryDateInput != null)
                    {
                        var expiryDateComponents = expiryDateInput.Split(separator: "-");
                        var expiryDate = new DateTime(year: Convert.ToInt32(value: expiryDateComponents[2]),
                                                      month: Convert.ToInt32(value: expiryDateComponents[1]),
                                                      day: Convert.ToInt32(value: expiryDateComponents[0]));

                        var toEncrypt =
                            $"{macAddressInput.Replace(oldValue: "-", newValue: "")}|{expiryDate.Date:dd-MM-yyyy}";
                        var encryptedValue = EncryptDecrypt.Encrypt(toEncrypt: toEncrypt,
                                                                    useHashing: true);
                        File.WriteAllLines(path: "License.txt",
                                           contents: new[]
                                                     {
                                                         $"License Key: {encryptedValue}",
                                                         $"Valid for Mac Address: {macAddressInput}",
                                                         $"Valid till date: {expiryDate.Date:dd-MMM-yyyy}"
                                                     });

                        Console.Clear();
                        Console.WriteLine(value: "License Key Generated Successfully...");

                        Console.WriteLine(value: $"License Key: {encryptedValue}");
                        Console.WriteLine(value: $"License is valid for Mac Address: {macAddressInput}");
                        Console.WriteLine(value:
                                          $"License is valid till date: {expiryDate.Date:dd-MMM-yyyy}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(value: "Failed to generate license key...");
                Console.WriteLine(value: $"Error: {e.Message}");
                //throw;
            }

            Console.ReadKey();
        }
    }
}