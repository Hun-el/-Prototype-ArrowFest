using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WindowType
{
    Positive, Negative
}
public enum StackType
{ 
    Plus,Times
}
public class Window : MonoBehaviour
{
    LevelSystem levelSystem;
    GameManager gameManager;

    [SerializeField] WindowType typeWindow;
    [SerializeField] StackType typeStack;
    [SerializeField] int value;
    [SerializeField] TextMesh textMesh;
    [SerializeField] MeshRenderer meshRenderer;

    void Start()
    {
        levelSystem = FindObjectOfType<LevelSystem>();
        gameManager = FindObjectOfType<GameManager>();

        setStackType();
        setWindowType();
        setValue();
        SetTextValue();
    }

    void setStackType()
    {
        if(Random.Range(1,101) <= gameManager.windowPlusRate)
        {
            typeStack = StackType.Plus;
        }
        else
        {
            typeStack = StackType.Times;
        }
    }

    void setWindowType()
    {
        int positiveRate = 100 - levelSystem.level * gameManager.positiveRateIncrease;
        if(positiveRate > gameManager.maxPositiveRate)
        {
            positiveRate = gameManager.maxPositiveRate;
        }
        if(positiveRate < gameManager.minPositiveRate)
        {
            positiveRate = gameManager.minPositiveRate;
        }

        if(Random.Range(1,101) > positiveRate)
        {
            typeWindow = WindowType.Negative;
        }
        else
        {
            typeWindow = WindowType.Positive;
        }
    }

    void setValue()
    {
        if(typeStack == StackType.Plus)
        {
            value = Random.Range(1,4 + (levelSystem.level / 2));
        }
        else
        {
            value = Random.Range(1,3 + (levelSystem.level / 20));
        }
    }

    void SetTextValue()
    {
        textMesh.color = typeWindow == WindowType.Positive ? Color.green : Color.red;
        textMesh.text = typeWindow == WindowType.Positive ? typeStack == StackType.Plus ? "+" : "x" : typeStack == StackType.Plus ? "-" : "÷";
        textMesh.text += value.ToString();
    }
    public WindowType TypeWindow { get => typeWindow; }
    public StackType TypeStack { get => typeStack; }
    public int Value { get => value; }
}
