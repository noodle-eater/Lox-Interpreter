namespace Lox_Interpreter;

public abstract class Expr
{
    public interface Visitor<T>
    {
        T VisitBinaryExpr(Binary expr);
        T VisitGroupingExpr(Grouping expr);
        T VisitLiteralExpr(Literal expr);
        T VisitUnaryExpr(Unary expr);
    }

    public abstract T Accept<T>(Visitor<T> visitor);

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

        public override T Accept<T>(Visitor<T> visitor)
        {
            return visitor.VisitBinaryExpr(this);
        }
    }
    public class Grouping : Expr
    {

        private readonly Expr expression;

        public Grouping(Expr expression)
        {
            this.expression = expression;
        }

        public override T Accept<T>(Visitor<T> visitor)
        {
            return visitor.VisitGroupingExpr(this);
        }
    }
    public class Literal : Expr
    {

        private readonly Object value;

        public Literal(Object value)
        {
            this.value = value;
        }

        public override T Accept<T>(Visitor<T> visitor)
        {
            return visitor.VisitLiteralExpr(this);
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

        public override T Accept<T>(Visitor<T> visitor)
        {
            return visitor.VisitUnaryExpr(this);
        }
    }
}
