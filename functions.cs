using UnityEngine;
using System.Collections;

public static class Functions{

	public static int getMaxFloatIndex(float[] array){
        float max = 0;
        int maxIndex = 0;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] > max)
            {
                maxIndex = i;
            }
        }
        return maxIndex;
    }

    public static Vector2 getDirectionFrom(Vector2 pointA, Vector2 pointB)
    {
        return (pointB - pointA).normalized;
    }

    public static Vector2 degreesToVector2(float degrees)
    {
        return new Vector2(Mathf.Cos(degrees), Mathf.Sin(degrees));
    }

    public static void ErrorMessage(string Message)
    {
        Debug.Log(Message);
    }
}
