using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[Serializable]
public class TreeConnectDetails
{
    public TreeConnectHandler childNode;
    public NodeDirectionType direction;

    [Range(100f, 350f)] public float length;
    [Range(-50f, 50f)] public float rotation;
}

public class TreeConnectHandler : MonoBehaviour
{
    private RectTransform rect => GetComponent<RectTransform>();
    [SerializeField] private TreeConnectDetails[] connectDetails;
    [SerializeField] private TreeConnection[] connections;

    private Image connectionImage;
    private Color originalColor;

    private void Awake()
    {
        if (connectionImage != null)
        {
            originalColor = connectionImage.color;
        }
    }

    private void OnValidate()
    {
        if (connectDetails.Length <= 0)
        {
            return;
        }

        if (connectDetails.Length != connections.Length)
        {
            Debug.Log("Amount of connectDetails should be same as amount of connection. - " + gameObject.name);
            return;
        }

        UpdateConnections();
    }

    public void UpdateConnections()
    {
        for (int i = 0; i < connectDetails.Length; i++)
        {
            var detail = connectDetails[i];
            var connection = connections[i];
            Vector2 targetPosition = connection.GetConnectionPoint(rect);
            Image connectionImage = connection.GetConnectionImage();

            connection.DirectConnection(detail.direction, detail.length, detail.rotation);

            if (detail.childNode == null)
            {
                continue;
            }

            detail.childNode.SetPosition(targetPosition);
            detail.childNode.SetConnectionImage(connectionImage);
            detail.childNode.transform.SetAsLastSibling();
        }
    }

    public void UpdateAllConnections()
    {
        UpdateConnections();

        foreach (var node in connectDetails)
        {
            if (node.childNode == null)
            {
                continue;
            }
            node.childNode.UpdateConnections();
        }
    }

    public void UnlockConnectionImage(bool isUnlocked)
    {
        if (connectionImage == null)
        {
            return;
        }

        connectionImage.color = isUnlocked ? Color.white : originalColor;
    }

    public void SetConnectionImage(Image image)
    {
        connectionImage = image;
    }

    public void SetPosition(Vector2 position)
    {
        rect.anchoredPosition = position;
    }
}
