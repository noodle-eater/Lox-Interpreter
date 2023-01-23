﻿namespace Lox_Interpreter;

public class Scanner
{
    private readonly string _source;
    private readonly List<Token> _tokens = new List<Token>();
    
    private int _start = 0;
    private int _current = 0;
    private int _line = 1;

    private static readonly Dictionary<string, TokenType> _keywords = new Dictionary<string, TokenType>()
    {
        {"and", TokenType.And},
        {"class", TokenType.Class},
        {"else", TokenType.Else},
        {"false", TokenType.False},
        {"for", TokenType.For},
        {"fun", TokenType.Fun},
        {"if", TokenType.If},
        {"nil", TokenType.Nil},
        {"or", TokenType.Or},
        {"print", TokenType.Print},
        {"return", TokenType.Return},
        {"super", TokenType.Super},
        {"this", TokenType.This},
        {"true", TokenType.True},
        {"var", TokenType.Var},
        {"while", TokenType.While},
    };

    public Scanner(string source)
    {
        _source = source;
    }

    public List<Token> ScanTokens()
    {
        while (!IsAtEnd())
        {
            _start = _current;
            ScanToken();
        }
        
        _tokens.Add(new Token(TokenType.Eof, "", null, _line));
        return _tokens;
    }
    
    private void ScanToken()
    {
        char c = Advance();
        switch (c)
        {
            case '(': AddToken(TokenType.LeftParen); break;
            case ')': AddToken(TokenType.RightParen); break;
            case '{': AddToken(TokenType.LeftBrace); break;
            case '}': AddToken(TokenType.RightBrace); break;
            case ',': AddToken(TokenType.Comma); break;
            case '.': AddToken(TokenType.Dot); break;
            case '-': AddToken(TokenType.Minus); break;
            case '+': AddToken(TokenType.Plus); break;
            case ';': AddToken(TokenType.Semicolon); break;
            case '*': AddToken(TokenType.Star); break;
            case '!':
                AddToken(Match('=') ? TokenType.BangEqual : TokenType.Bang);
                break;
            case '=':
                AddToken(Match('=') ? TokenType.EqualEqual : TokenType.Equal);
                break;
            case '<':
                AddToken(Match('=') ? TokenType.LessEqual : TokenType.Less);
                break;
            case '>':
                AddToken(Match('=') ? TokenType.GreaterEqual : TokenType.Greater);
                break;
            case '/':
                if (Match('/'))
                {
                    // A comment goes until the end of the line.
                    while (Peek() != '\n' && !IsAtEnd()) Advance();
                }
                else
                {
                    AddToken(TokenType.Slash);
                }
                break;
            case ' ':
            case '\r':
            case '\t':
                // Ignore whitespace.
                break;
            
            case '\n':
                _line++;
                break;
            
            case '"': String();
                break;
            
            default:
                if (IsDigit(c))
                {
                    Number();
                } else if (IsAlpha(c))
                {
                    Identifier();
                }
                else
                {
                    Lox.Error(_line, "Unexpected character,");
                }
                break;
        }
    }

    private char Advance() => _source[_current++];
    private bool IsAtEnd() => _current >= _source.Length;
    private bool IsDigit(char c) => c is >= '0' and <= '9';
    private bool IsAlphaNumeric(char c) => IsAlpha(c) || IsDigit(c);
    private void AddToken(TokenType tokenType) => AddToken(tokenType, null);

    private void AddToken(TokenType tokenType, object literal)
    {
        string text = _source.Substring(_start, _current);
        _tokens.Add(new Token(tokenType, text, literal, _line));
    }

    private bool Match(char expected)
    {
        if (IsAtEnd()) return false;
        if (_source[_current] != expected) return false;

        _current++;
        return true;
    }

    private char Peek()
    {
        if (IsAtEnd()) return '\0';
        return _source[_current];
    }

    private char PeekNext()
    {
        if (_current + 1 >= _source.Length) return '\0';
        return _source[_current + 1];
    }
    
    private void String()
    {
        while (Peek() != '"' && !IsAtEnd())
        {
            if (Peek() == '\n') _line++;
            Advance();
        }

        if (IsAtEnd())
        {
            Lox.Error(_line, "Unterminated string.");
            return;
        }
        
        // Closing ".
        Advance();

        string value = _source.Substring(_start + 1, _current - 1);
        AddToken(TokenType.String, value);
    }

    private void Number()
    {
        while (IsDigit(Peek())) Advance();
        
        // Look for a fractional part.
        if (Peek() == '.' && IsDigit(PeekNext()))
        {
            // Consume the .
            Advance();

            while (IsDigit(Peek())) Advance();
        }
        
        AddToken(TokenType.Number, double.Parse(_source.Substring(_start, _current)));
    }

    private void Identifier()
    {
        while (IsAlphaNumeric(Peek())) Advance();

        string text = _source.Substring(_start, _current);
        TokenType type = TokenType.None;
        if (!_keywords.ContainsKey(text))
        {
            type = TokenType.Identifier;
        }
        
        AddToken(type);
    }

    private bool IsAlpha(char c)
    {
        return (c is >= 'a' and <= 'z') ||
               (c is >= 'A' and <= 'Z') ||
               c == '_';
    }
}