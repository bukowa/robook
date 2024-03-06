using System.ComponentModel;
using System.Text.Json;

namespace Robook.Accounts;

/// <summary>
///     Local disk storage for Rithmic accounts.
/// </summary>
public class AccountsStore {
    
    /// <summary>
    /// Collection of accounts.
    /// </summary>
    public BindingList<Account> Accounts = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountsStore"/> class.
    /// </summary>
    /// <param name="path">Path to the accounts store.</param>
    public AccountsStore(string path) {
        _path = path;
    }

    private readonly string _path;
    private object _lock = new();
    private bool   _hasRead;

    /// <summary>
    /// Write the accounts store to disk.
    /// </summary>
    public void Write() {
        lock (_lock) {
            var json = JsonSerializer.Serialize(this.Accounts, new JsonSerializerOptions {
                WriteIndented = true
            });
            File.WriteAllText(_path, json);
        }
    }

    /// <summary>
    /// Read the accounts store from disk.
    /// </summary>
    public void Read() {
        if (_hasRead) return;
        if (!File.Exists(_path)) return;
        var json = File.ReadAllText(_path);
        Accounts = JsonSerializer.Deserialize<BindingList<Account>>(json);
        _hasRead = true;
    }
}