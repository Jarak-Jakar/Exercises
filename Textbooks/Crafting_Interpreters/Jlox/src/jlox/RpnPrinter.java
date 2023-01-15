package jlox;

public class RpnPrinter implements Expr.Visitor<String> {
    public String print(Expr expr) {
        return evaluate(expr);
    }

    @Override
    public String visitAssignExpr(Expr.Assign expr) {
        var assignation = evaluate(expr.value);
        var assignee = expr.name.lexeme;
        return String.format("%s %s =", assignation, assignee);
    }

    @Override
    public String visitBinaryExpr(Expr.Binary expr) {
        var operator = expr.operator.lexeme;
        var left = evaluate(expr.left);
        var right = evaluate(expr.right);
        return String.format("%s %s %s", left, right, operator);
    }

    @Override
    public String visitCallExpr(Expr.Call expr) {
        var arguments = expr.arguments.stream().map(this::evaluate).toList();
        var called = evaluate(expr.callee);
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
        return null;
    }

    @Override
    public String visitGroupingExpr(Expr.Grouping expr) {
        return null;
    }

    @Override
    public String visitLiteralExpr(Expr.Literal expr) {
        return expr.value.toString();
    }

    @Override
    public String visitLogicalExpr(Expr.Logical expr) {
        return null;
    }

    @Override
    public String visitSetExpr(Expr.Set expr) {
        return null;
    }

    @Override
    public String visitSuperExpr(Expr.Super expr) {
        return null;
    }

    @Override
    public String visitThisExpr(Expr.This expr) {
        return null;
    }

    @Override
    public String visitUnaryExpr(Expr.Unary expr) {
        var operator = expr.operator.lexeme;
        var operand = evaluate(expr.right);
        return String.format("%s %s", operand, operator);
    }

    @Override
    public String visitVariableExpr(Expr.Variable expr) {
        return null;
    }

    private String evaluate(Expr expr) {
        return expr.accept(this);
    }
}
