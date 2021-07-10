# Чистый код 🥴:pinched_fingers:🤙🏿

## 1. Обязательно нужно использовать префикс "_" в названии `private` или `protected` переменных, а также использовать префикс "s_" в названии `private static` или `protected static` переменных.
```cs
[SerializeField] private float _variable;
[SerializeField] protected float _variable;
private float _variable;
protected float _variable;
```
## 2. Ключевое слово `var` необходимо использовать только в случае явного указания значения переменной или использования ключевого слова `new()`.
```cs
var a = "Hello, motherfucker!";
var a = new ArrayList();
```
## 3. Если внутренности блока `if` занимают больше двух строк и если есть возможность инвертировать его, то необходимо сделать это.
### 3.1 Один уровень вложенности:
*Неправильно:*
```cs
void Method(bool condition, object coolObject, object normalObject, object sadObject)
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
void Method(bool condition, object coolObject, object normalObject, object sadObject)
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
void Method(bool condition, object coolObject, object normalObject)
{
    if (condition)
    {
        coolObject.Method();
        normalObject.Method();
    }
}
```
### 3.2 Два и больше уровня вложенности:
*Неправильно:*
```cs
void Method(bool condition1, bool condition2, bool condition3, object coolObject)
{
    if (condition1)
    {
        if (condition2)
        {
            if (condition3)
            {
                coolObject.Method();
            }
        }
    }
}
```
*Правильно:*
```cs
void Method(bool condition1, bool condition2, bool condition3, object coolObject)
{
    if (!condition1 || !condition2 || !condition3)
    {
        return;
    }
}
```
## 4. Необходимо оставлять пустую строку в конце каждого скрипта.
## 5. При хэширование наследников `YieldInstruction` (`WaitForSeconds`, `WaitForFixedUpdate` и так далее) название переменной должно соответствовать названию класса.
```cs
IEnumerator Moving(State state)
{
    var waitForFixedUpdate = new WaitForFixedUpdate();
    while (true)
    {
        yield return waitForFixedUpdate;
        state.Move();
    }
}
```
## 6. Если внутри оператора `if-else` одна строка кода, то её необходимо записать с новой строки в фигурных скобках, а не в одной строке с `if-else`.
*Неправильно:*
```cs
if (condition) coolObject.Method();
```
*Правильно:*
```cs
if (condition)
{
    coolObject.Method();
}
```
## 7(4). В конце каждого скрипта должна быть пустая строка.
## 8. В случае, когда от типа ничего не наследуется, этот тип следует обозначить, как `sealed`.
*Неправильно:*
```cs
public class Work { }
```
*Правильно:*
```cs
public sealed class Work { }
```
## 9. В случае, когда поле инициализируется только в конструкторе, его следует обозначить как `readonly`.
*Неправильно:*
```cs
public class Work
{
    private float _amount;
    
    public Work(float amount) => _amount = amount
    
    public void ShowWork() => Debug.Log(_amount);
}
```
*Правильно:*
```cs 
class Work
{
    private readonly float _amount;
    
    public Work(float amount) => _amount = amount
    
    public void ShowWork() => Debug.Log(_amount);
}
```
## 10. В случае, когда класс/структура/интерфейс/поле/свойство/метод нуждается в дополнительной обработке, к нему следует приписать атрибут [NeedsRefactor] с соответствующими параметрами.
*Неправильно:*
```cs
// todo: Needs implementation
void BadMethod() { }
```
*Правильно:*
```cs
[NeedsRefactor(Needs.Implementation)]
void BadMethod() { }
```
## 11. В случае создания пустого массива следует использовать `Array.Empty<T>()`.
*Неправильно:*
```cs
var array = new object[];
```
*Правильно:*
```cs
var array = Array.Empty<object>();
```
## 12(4). Одна пустая строка должна быть в конце каждого файла.
