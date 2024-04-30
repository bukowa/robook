using System.ComponentModel;

namespace Robook.Data;

public class Symbol {
    public string Name { get; set;}
    public string Exchange { get; set; }
    
    public Symbol(string name, string exchange) {
        Name = name;
        Exchange = exchange;
    }
    
    public Symbol() {}
}
