using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;


//alias PartController as PC
using PC = PartController;

public class ShipController : MonoBehaviour
{
    public const int maxWidth = 10, maxHeight = 7;

    //TODO init Dotween in main loading or whatever


    public List<PartController> Parts = new();
    public Dictionary<Vector2Int, PartController> PartMap = new();


    public PartController PartAtPosition(Vector2Int position)
    {
        return PartMap.ContainsKey(position) ? PartMap[position] : null;
    }

    public PartController GetMainPart()
    {
        const string mainPartName = "Main";
        return PartMap.Values.FirstOrDefault(part => part.PartSO._name == mainPartName);
    }

    public bool HasMainPart()
    {
        return GetMainPart() != null;
    }

    public bool InBounds(Vector2Int position)
    {
        return position.x >= 0 && position.x < maxWidth && position.y >= 0 && position.y < maxHeight;
    }

    private void PlacePart(PartSO part, Vector2Int position)
    {
        PartController partC = Instantiate(PartsCollectionManager.Instance.PartControllerPrefab, transform);
        partC.PartSO = part;
        partC.transform.localPosition = new Vector3(position.x, -position.y, 0);
        partC.shipPosition = position;
        Parts.Add(partC);
        for (int i = 0; i < PC.maxWidth; i++)
            for (int j = 0; j < PC.maxHeight; j++)
                if (part.shape[j * PC.maxWidth + i])
                    PartMap.Add(position + new Vector2Int(i, j), partC);
    }

    public void RemovePart(PartController part)
    {
        Parts.Remove(part);
        //TODO do it faster through enumeate
        for (int x=0; x < maxWidth; x++)
        {
            for (int y = 0; y < maxHeight; y++)
            {
                Vector2Int position = new Vector2Int(x, y);
                if (PartAtPosition(position) == part)
                {
                    PartMap.Remove(position);
                }
            }
        }

        Destroy(part.gameObject);
    }

    public bool IsConnectedToMainPart(PartController part)
    {
        if (!HasMainPart())
            return false;
        //BFS
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(part.shipPosition);
        Vector2Int mainPartPosition = GetMainPart().shipPosition;
        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            if (visited.Contains(current))
                continue;
            if (PartAtPosition(current) == null)
                continue;
            visited.Add(current);
            if (current == mainPartPosition)
                return true;
            foreach (Vector2Int direction in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {
                Vector2Int next = current + direction;
                if (InBounds(next))
                    queue.Enqueue(next);
            }
        }
        return false;
    }

    public bool IsWholeShipConnectedToMainPart()
    {
        if (!HasMainPart())
            return false;
        foreach (PartController part in Parts)
        {
            if (!IsConnectedToMainPart(part))
                return false;
        }
        return true;
    }


    #region Serialization

    public string Serialize()
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (PartController part in Parts)
        {
            stringBuilder.Append(part.shipPosition.x);
            stringBuilder.Append(',');
            stringBuilder.Append(part.shipPosition.y);
            stringBuilder.Append(',');
            stringBuilder.Append(part.PartSO._name);
            stringBuilder.Append(';');
        }
        return stringBuilder.ToString();
    }

    public void Deserialize(string data)
    {
        foreach (PartController part in Parts)
        {
            Destroy(part.gameObject);
        }
        Parts.Clear();
        PartMap.Clear();
        string[] parts = data.Split(';');
        foreach (string part in parts)
        {
            if (part == "")
                continue;
            string[] partData = part.Split(',');
            int x = int.Parse(partData[0]);
            int y = int.Parse(partData[1]);
            char partID = partData[2][0];
            PartSO partSO = PartsCollectionManager.Instance.GetPart(partID);
            PlacePart(partSO, new Vector2Int(x, y));
        }
    }

    #endregion





    #region Gizmos
    private void OnDrawGizmos()
    {
        for (int x = 0; x < maxWidth; x++)
        {
            for (int y = 0; y < maxHeight; y++)
            {
                Gizmos.color = PartAtPosition(new Vector2Int(x, y)) ? Color.green : Color.red;
                Vector3 cubePos = new Vector3(x + 0.5f, -y - 0.5f, 0);
                Vector3 cubeSize = Vector3.one * 0.99f;
                cubePos = Vector3.Scale(cubePos, transform.lossyScale) + transform.position;
                cubeSize = Vector3.Scale(cubeSize, transform.lossyScale);
                Gizmos.DrawWireCube(cubePos, cubeSize);
            }
        }
    }
    #endregion
}
