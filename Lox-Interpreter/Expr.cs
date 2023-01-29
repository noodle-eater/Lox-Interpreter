namespace Lox_Interpreter;

public abstract class Expr
{
    public class Binary : Expr
    {
        private readonly Expr _left;
        private readonly Token _operator;
        private readonly Expr _right;

        public Binary(Expr left, Token op, Expr right)
        {
            _left = left;
            _operator = op;
            _right = right;
        }
    }
}