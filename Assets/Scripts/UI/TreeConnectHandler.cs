using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class TreeConnectDetails
{
    public TreeConnectHandler childNode;
    public NodeDirectionType direction;

    [Range(100f, 350f)]
    public float length;
}

public class TreeConnectHandler : MonoBehaviour
{
    private RectTransform rect => GetComponent<RectTransform>();
    [SerializeField] private TreeConnectDetails[] connectDetails;
    [SerializeField] private TreeConnection[] connections;

    private void OnValidate()
    {
        if (connectDetails.Length != connections.Length)
        {
            Debug.Log("Amount of connectDetails should be same as amount of connection. - " + gameObject.name);
        }

        UpdateConnections();
    }

    private void UpdateConnections()
    {
        for (int i = 0; i < connectDetails.Length; i++)
        {
            var detail = connectDetails[i];
            var connection = connections[i];
            Vector2 targetPosition = connection.GetConnectionPoint(rect);

            connection.DirectConnection(detail.direction, detail.length);
            detail.childNode?.SetPosition(targetPosition);
        }
    }

    public void SetPosition(Vector2 position)
    {
        rect.anchoredPosition = position;
    }
}
