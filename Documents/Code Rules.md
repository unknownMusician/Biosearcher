### 1. Обязательно нужно использовать префикс "_" в названии `private` или `protected` переменных.
```cs
[SerializeField] private float _variable;
[SerializeField] protected float _variable;
private float _variable;
protected float _variable;
```
### 2. Ключевое слово `var` нужно использовать только в случае явного указания значения переменной или использования ключевого слова `new()`.
```cs
var a = "Hello, motherfucker!";
var a = new ArrayList();
```