using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class CircleGridd : MonoBehaviour
{
    [SerializeField] private int elementsInGridCount;
    [SerializeField] private int maxElementsCount;
    [SerializeField] private float radius;
    private float elementsDistance;
    private Vector3 circleCenter;
    private float circleLenght;
    private List<Vector3> pointArray = new List<Vector3>();
    private bool isInit;

    private void Start()
    {
        circleCenter = this.transform.position;
        circleLenght = 2 * (float)Math.PI * radius;
        elementsDistance = circleLenght / maxElementsCount;
        float angle = ((360 * (float)Math.PI) / 180) / maxElementsCount;
        AddPoints(angle);
    }

    private async void AddPoints(float angle)
    {
        await Task.Run(() =>
        {
            for (int i = 1; i <= maxElementsCount; i++)
            {
                pointArray.Add(new Vector3(circleCenter.x + radius * (float)Math.Cos(angle * i), circleCenter.y + radius * (float)Math.Sin(angle * i), 0));
            }
        });
        isInit = true;
    }

    public void AddElement(GameObject element)
    {
        if (elementsInGridCount < maxElementsCount && isInit)
        {
            GameObject d = Instantiate(element, this.transform.position, Quaternion.identity);
            d.transform.position = pointArray[elementsInGridCount];
            elementsInGridCount += 1;
        }
    }
}
