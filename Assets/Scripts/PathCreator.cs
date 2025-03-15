using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    public static PathCreator instance;
    
    [SerializeField] private float maxMovementAmount;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject pathHolder;
    [SerializeField] private GameObject nodeObj;
    [SerializeField] private TMP_Text text;

    private bool isValidPos = true;
    
    private List<GameObject> nodeObjects = new List<GameObject>();

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy (gameObject);
    }

    private void Update() {
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        transform.position = mouseWorldPosition;

        if (Input.GetMouseButtonDown(1)) {
            RemoveNode();
        }

        if (nodeObjects.Count >= maxMovementAmount) return;

        if (Input.GetMouseButtonDown(0)) {
            if (!isValidPos) return;

            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("Obstacle")) return;

            Vector3 startPos;

            if (nodeObjects.Count > 0) startPos = nodeObjects.Last().transform.position;
            else startPos = GameObject.Find("Player").transform.position; // horrible horrible ew ew ew ew
            
            Vector2 direction = transform.position - startPos;
            float distance = Vector2.Distance(startPos, transform.position);

            RaycastHit2D[] hitList = Physics2D.RaycastAll(startPos, direction, distance);
            foreach (RaycastHit2D objectHit in hitList)
            {
                Debug.Log(objectHit.collider.gameObject);
                if (objectHit.collider != null && objectHit.collider.CompareTag("Obstacle")) return;
            }
            
            GameObject newNode = Instantiate(nodeObj, transform.position, Quaternion.identity, pathHolder.transform);
            nodeObjects.Add(newNode);
        }

        DisplayPaths();
    }
    private void DisplayPaths()
    {
        text.text = "Moves Placed: " + nodeObjects.Count + " Max Moves: " + maxMovementAmount;
    }
    private void RemoveNode() {
        if (nodeObjects.Count == 0) return;

        Destroy(nodeObjects.Last());
        nodeObjects.Remove(nodeObjects.Last());
    }

    public Path GetPath() {
        List<Node> nodeList = new List<Node>();
        nodeObjects.ForEach(o => nodeList.Add(new Node(o.transform.position)));

        return new Path(nodeList);
    }

    public void ResetPath() {
        nodeObjects.ForEach(o => Destroy(o));
        nodeObjects.Clear();
    }
}
