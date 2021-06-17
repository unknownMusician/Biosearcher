### 1. Обязательно нужно использовать префикс "_" в названии `private` или `protected` переменных.
```cs
[SerializeField] private float _variable;
[SerializeField] protected float _variable;
private float _variable;
protected float _variable;
```
### 2. Ключевое слово `var` необходимо использовать только в случае явного указания значения переменной или использования ключевого слова `new()`.
```cs
var a = "Hello, motherfucker!";
var a = new ArrayList();
```
### 3. Если внутренности блока `if` занимают больше двух строк, его необходимо инвертировать.
*Неправильно:*
```cs
private void Method()
{
    if (condition)
    {
        coolObject.Method();
        normalObject.Method();
        sadObject.Method();
    }
}
```
*Правильно:*
```cs
private void Method()
{
    if (!condition)
    {
        return;
    }

    coolObject.Method();
    normalObject.Method();
    sadObject.Method();
}
```
```cs
private void Method()
{
    if (condition)
    {
        coolObject.Method();
        normalObject.Method();
    }
}
```
### 4. Необходимо оставлять пустую строку в конце скрипта.
### 5. При хэширование наследников `YieldInstruction` (`WaitForSeconds`, `WaitForFixedUpdate` и так далее) название переменной должно соответствовать названию класса.
```cs
protected IEnumerator Moving()
{
    var waitForFixedUpdate = new WaitForFixedUpdate();
    while (true)
    {
        yield return waitForFixedUpdate;
        _state.Move();
    }
}
```
### 6. Если внутри оператора `if-else` одна строка кода, то её нужно записать с новой строки в фигурных скобках, а не в одной строке с `if-else`.
*Неправильно:*
```cs
private void Method()
{
    if (condition) coolObject.Method();
}
```
*Правильно:*
```cs
private void Method()
{
    if (condition)
    {
        coolObject.Method();
    }
}
```
### 7. В конце каждого скрипта должна быть пустая строка.