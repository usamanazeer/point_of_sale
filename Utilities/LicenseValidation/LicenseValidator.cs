using System;
using System.Globalization;
using Utilities.EncryptDecryptUtil;

namespace Utilities.LicenseValidation
{
    public class LicenseValidator: ILicenseValidator
    {

        public bool ValidateLicense(string encryptedKey, string systemMacAddress, out bool isValid , out bool isExpired,out bool isInValid)
        {
            isValid = true;
            isExpired = false;
            isInValid = false;
            var currentDate = DateTime.UtcNow.Date;
            var systemMac = systemMacAddress;
            var encryptedLicenseKey = encryptedKey;

            
            var decryptedLicenseKeyComponents = EncryptDecrypt.Decrypt(cipherString: encryptedLicenseKey, useHashing: true).Split(separator: "|");
            var decryptedMac = decryptedLicenseKeyComponents[0];
            var decryptedExpiryDate = decryptedLicenseKeyComponents[1];
            
            var expiryDate = DateTime.ParseExact(s: decryptedExpiryDate, format: "dd-MM-yyyy", CultureInfo.InvariantCulture);
            if (decryptedMac != systemMac || expiryDate.Date < currentDate.Date)
            {
                isValid = false;
                if (decryptedMac != systemMac) isInValid = true;
                if (!(expiryDate.Date >= currentDate.Date)) isExpired = true;
                return decryptedMac == systemMac && expiryDate.Date >= currentDate.Date;
            }
            return decryptedMac == systemMac && expiryDate.Date >= currentDate.Date;
        }
    }

    public interface ILicenseValidator
    {
        bool ValidateLicense(string key, string systemMacAddress, out bool isValid, out bool isExpired, out bool isInvalid);
    }
}
