using UnityEngine;
using Zenject;

public class Fields : MonoBehaviour
{
    public Field[,] _fields2D { get; private set; }

    [Inject]
    void Construct()
    {
        _fields2D = new Field[8, 8];

        int figureCount = transform.childCount - 1;

        for (int i = 0; i < figureCount; i++)
        {
            int x = i % 8;
            int y = i / 8;

            _fields2D[x, y] = transform.GetChild(i + 1).GetComponent<Field>();
        }
    }
}
