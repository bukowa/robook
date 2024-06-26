﻿using System.ComponentModel;
using System.Globalization;
using System.Text.Json.Serialization;


namespace Robook.Data;

// [TypeConverter(typeof(SymbolStringConverter))]
public class Symbol {
    
    [JsonIgnore]
    public string DisplayName => $"{Name}:{Exchange}";
    
    public string Id { get; set;} = Guid.NewGuid().ToString();
    
    [JsonIgnore]
    public Symbol Self     => this;
    public string Name     { get; set; }
    public string Exchange { get; set; }

    public Symbol(string name, string exchange) {
        Name     = name;
        Exchange = exchange;
    }

    public Symbol() {
    }
    
    // this is required for the DataGridViewComboBoxColumn to work :O ?
    public override bool Equals(object? obj) {
        if (obj is Symbol symbol) {
            return Name == symbol.Name && Exchange == symbol.Exchange;
        }
        return false;
    }
}

public class SymbolStringConverter : TypeConverter {
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) {
        if (sourceType == typeof(string)) {
            return true;
        }

        return base.CanConvertFrom(context, sourceType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture,
                                        object                 value) {
        if (value is string str) {
            string[] parts = str.Split(':');
            return new Symbol(parts[0], parts[1]);
        }

        return base.ConvertFrom(context, culture, value);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType) {
        if (destinationType == typeof(string)) {
            return true;
        }

        return base.CanConvertTo(context, destinationType);
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value,
                                      Type                    destinationType) {
        if (value is Symbol symbol && destinationType == typeof(string)) {
            return $"{symbol.Name}:{symbol.Exchange}";
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}