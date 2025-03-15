using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private Vector2 position;
    public float checkRadius;

    public List<GameObject> objectsInRadius;

    public Vector2 Position {
        get => position;
        private set => position = value;
    }

    public Node(Vector2 position) {
        this.position = position;
    }
}
