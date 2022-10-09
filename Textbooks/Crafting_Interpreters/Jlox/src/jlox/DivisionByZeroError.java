package jlox;

public class DivisionByZeroError extends RuntimeException {
    public DivisionByZeroError(Expr expr) {
        super("Attempted to divide by zero in expression " + new AstPrinter().print(expr));
    }
}
