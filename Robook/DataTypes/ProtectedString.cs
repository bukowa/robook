using System.ComponentModel;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Robook.Store;


/// <summary>
/// String that uses Data Protection API to encrypt and decrypt data.
/// </summary>
[TypeConverter(typeof(ProtectedStringConverter))]
public class ProtectedString {
    /// <summary>
    ///     Create a new protected string.
    /// </summary>
    public ProtectedString(string data) {
        _value = EncryptData(data);
    }

    /// <summary>
    ///     Constructor for Json deserialization.
    /// </summary>
    [JsonConstructor]
    public ProtectedString() {
        _jsoned = true;    
    }
    
    private bool   _jsoned = false;
    private string _value;
    
    /// <summary>
    ///     Get the decrypted value.
    /// </summary>
    public string Value {
        get => _value;
        set {
            if (_jsoned) {
                _value = value;
                _jsoned = false;
            } else {
                _value = EncryptData(value);
            }
        }
    }
    

    /// <summary>
    ///     Return the encrypted value.
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
        return Value;
    }

    /// <summary>
    ///    Decrypt the value.
    /// </summary>
    /// <returns></returns>
    public string Decrypted() {
        return DecryptData(_value);
    }

    /// <summary>
    ///     Encrypt data using DPAPI.
    /// </summary>
    public static string EncryptData(string data) {
        byte[] plainBytes     = Encoding.UTF8.GetBytes(data);
        byte[] encryptedBytes = ProtectedData.Protect(plainBytes, null, DataProtectionScope.CurrentUser);
        return Convert.ToBase64String(encryptedBytes);
    }

    /// <summary>
    ///     Decrypt data using DPAPI.
    /// </summary>
    public static string DecryptData(string encryptedData) {
        byte[] encryptedBytes = Convert.FromBase64String(encryptedData);
        byte[] plainBytes     = ProtectedData.Unprotect(encryptedBytes, null, DataProtectionScope.CurrentUser);
        return Encoding.UTF8.GetString(plainBytes);
    }
}


public class ProtectedStringConverter : TypeConverter
{

    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
        if (sourceType == typeof(string))
            return true;
            
        return base.CanConvertFrom(context, sourceType);
    }
    
    public override object? ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
    {
        if (value is string)
        {
            return new ProtectedString((string)value);
        }
        return base.ConvertFrom(context, culture, value);
    }
    
}
