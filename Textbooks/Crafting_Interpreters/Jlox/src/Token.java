package jarak.jakar.craftinginterpreters.jlox;


public class Token {
    final jarak.jakar.craftinginterpreters.jlox.TokenType type;
    final String lexeme;
    final Object literal;
    final int line;

    Token(jarak.jakar.craftinginterpreters.jlox.TokenType type, String lexeme, Object literal, int line) {
        this.type = type;
        this.lexeme = lexeme;
        this.literal = literal;
        this.line = line;
    }

    public String toString() {
        return type + " " + lexeme + " " + literal;
    }
}
