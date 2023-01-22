package jlox;

// I make  no claims that this implementation actually makes sense...
public class RpnPrinter implements Expr.Visitor<String> {
    public String print(Expr expr) {
        return evaluate(expr);
    }

    @Override
    public String visitAssignExpr(Expr.Assign expr) {
        String assignation = evaluate(expr.value);
        String assignee = expr.name.lexeme;
        return String.format("%s %s =", assignee, assignation);
    }

    @Override
    public String visitBinaryExpr(Expr.Binary expr) {
        String operator = expr.operator.lexeme;
        String left = evaluate(expr.left);
        String right = evaluate(expr.right);
        return String.format("%s %s %s", left, right, operator);
    }

    @Override
    public String visitCallExpr(Expr.Call expr) {
        var arguments = expr.arguments.stream().map(this::evaluate).toList();
        String called = evaluate(expr.callee);
        var builder = new StringBuilder((arguments.size() + 1) * 3); // making a guess at the length needed.
        for (String argument :
                arguments) {
            builder.append(String.format("%s ", argument));
        }
        builder.append(called);
        return builder.toString();
    }

    @Override
    public String visitGetExpr(Expr.Get expr) {
        String name = expr.name.lexeme;
        String object = evaluate(expr.object);
        return String.format("%s: %s", name, object);
    }

    @Override
    public String visitGroupingExpr(Expr.Grouping expr) {
        return String.format("(%s)", evaluate(expr));
    }

    @Override
    public String visitLiteralExpr(Expr.Literal expr) {
        return expr.value.toString();
    }

    @Override
    public String visitLogicalExpr(Expr.Logical expr) {
        String operator = expr.operator.lexeme;
        String left = evaluate(expr.left);
        String right = evaluate(expr.right);
        return String.format("%s %s %s", left, right, operator);
    }

    @Override
    public String visitSetExpr(Expr.Set expr) {
        String object = evaluate(expr.object);
        String name = expr.name.lexeme;
        String value = evaluate(expr.value);
        return String.format("%s %s <- %s", name, object, value);
    }

    @Override
    public String visitSuperExpr(Expr.Super expr) {
        return String.format("%s %s super", expr.keyword.lexeme, expr.method.lexeme);
    }

    @Override
    public String visitThisExpr(Expr.This expr) {
        return String.format("%s this", expr.keyword.lexeme);
    }

    @Override
    public String visitUnaryExpr(Expr.Unary expr) {
        String operator = expr.operator.lexeme;
        String operand = evaluate(expr.right);
        return String.format("%s %s", operand, operator);
    }

    @Override
    public String visitVariableExpr(Expr.Variable expr) {
        return expr.name.lexeme;
    }

    private String evaluate(Expr expr) {
        return expr.accept(this);
    }
}
