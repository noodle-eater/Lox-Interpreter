namespace Lox_Interpreter;

public abstract class Expr
{
    public class Binary : Expr
    {

        private readonly Expr left;
        private readonly Token op;
        private readonly Expr right;

        public Binary(Expr left, Token op, Expr right)
        {
            this.left = left;
            this.op = op;
            this.right = right;
        }
    }
    public class Grouping : Expr
    {

        private readonly Expr expression;

        public Grouping(Expr expression)
        {
            this.expression = expression;
        }
    }
    public class Literal : Expr
    {

        private readonly Object value;

        public Literal(Object value)
        {
            this.value = value;
        }
    }
    public class Unary : Expr
    {

        private readonly Token op;
        private readonly Expr right;

        public Unary(Token op, Expr right)
        {
            this.op = op;
            this.right = right;
        }
    }
}
