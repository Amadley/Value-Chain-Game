using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TileTooltip : MonoBehaviour
{
    public static TileTooltip Instance { get; private set; }

    public float TextPadding = 5f;
    [SerializeField]
    private RectTransform canvasRectTransform;

    private RectTransform backgroundRectTransform;
    private TextMeshProUGUI textMeshPro;
    private RectTransform rectTransform;

    private void Awake()
    {
        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
        textMeshPro = transform.Find("TileTooltipText").GetComponent<TextMeshProUGUI>();
        rectTransform = transform.GetComponent<RectTransform>();

        Instance = this;
        HideTooltip();
    } 

    private void Update()
    {
        AnchorTooltipToMouse();


    }
     
    private void SetText(string tooltipText)
    {
        textMeshPro.SetText(tooltipText);
        textMeshPro.ForceMeshUpdate();

        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(TextPadding, TextPadding);

        backgroundRectTransform.sizeDelta = textSize + paddingSize;
    }

    private void AnchorTooltipToMouse()
    {
        Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;

        if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width)
        {
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        }

        if (anchoredPosition.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height)
        {
            anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;
        }

        rectTransform.anchoredPosition = anchoredPosition;
    }

    public void ShowTooltip(Tile tile)
    {
        gameObject.SetActive(true);
        SetText(GetTileDataText(tile));
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    public static void ShowTooltipStatic(Tile tile)
    {
        Instance.ShowTooltip(tile);
    }

    public static void HideTooltipStatic()
    {
        Instance.HideTooltip();
    }

    private string GetTileDataText(Tile tile)
    {
        string tileDataText = null;

        if (tile == null)
        {
            tileDataText = "No Tile Data Found - TileTooltip.Instance.GetTileDataText returns null.";
        }
        else
        {
            tileDataText = "Chunk: " + tile.ParentChunk.name + "\nLocal Tile: " + tile.PositionInChunk;
        }

        return tileDataText;
    }
}
