//> Classes lox-instance
package jlox;

import java.util.HashMap;
import java.util.Map;

class LoxInstance {
    //> lox-instance-fields
    private final Map<String, Object> fields = new HashMap<>();
    private final LoxClass klass;
//< lox-instance-fields

    LoxInstance(LoxClass klass) {
        this.klass = klass;
    }

    //> lox-instance-get-property
    Object get(Token name) {
        if (fields.containsKey(name.lexeme)) {
            return fields.get(name.lexeme);
        }

//> lox-instance-get-method
        LoxFunction method = klass.findMethod(name.lexeme);
/* Classes lox-instance-get-method < Classes lox-instance-bind-method
    if (method != null) return method;
*/
//> lox-instance-bind-method
        if (method != null) {
            return method.bind(this);
        }
//< lox-instance-bind-method

//< lox-instance-get-method
        throw new RuntimeError(name, // [hidden]
                "Undefined property '" + name.lexeme + "'.");
    }

    //< lox-instance-get-property
//> lox-instance-set-property
    void set(Token name, Object value) {
        fields.put(name.lexeme, value);
    }

    //< lox-instance-set-property
    @Override
    public String toString() {
        return klass.name + " instance";
    }
}

