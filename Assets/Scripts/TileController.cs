using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public delegate void TileInteractions(TileController active, TileController target);
public class TileController : MonoBehaviour
{
    public event TileInteractions OnTileSlied;

    private Vector2 _anchoredPosition = Vector2.zero;
    private Vector2 _originalPosition = Vector2.zero;

#if UNITY_IPHONE || UNITY_EDITOR
    [SerializeField]
    private Sprite _iosPart;
    private Image _picture;
#endif
    [SerializeField]
    private int _originalIndex;
    public int actualIndex;

    public Canvas _canvas = null;
    
    public TileController Above = null;
    public TileController Right = null;
    public TileController Beneath = null;
    public TileController Left = null;

    public Vector2 AnchoredPosition { get { _anchoredPosition = transform.position; return _anchoredPosition;} set { transform.position = value; } }
    public int OriginalIndex { get { return _originalIndex; } private set { } }

    public bool IsEmpty { get { return _originalIndex == 4; } }
    public bool IsWellPlaced { get {
            return _originalIndex == actualIndex; } }

    public void BeginDrag(BaseEventData data)
    {
        _anchoredPosition = transform.position;
        _originalPosition = transform.position;
    }
    public void DragHandler(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;
        Vector2 position;

        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)_canvas.transform, pointerData.position, _canvas.worldCamera, out position);
        Vector2 localMousePosition = _canvas.transform.TransformPoint(position);

        if (Right?.IsEmpty ?? false) { transform.position = new Vector2(localMousePosition.x, transform.position.y); }
        if (Left?.IsEmpty ?? false) { transform.position = new Vector2(localMousePosition.x, transform.position.y); }


        if (Right?.IsEmpty ?? false)
        {

            float minmaxingX = Mathf.Min(Mathf.Max(localMousePosition.x, _anchoredPosition.x), Right.AnchoredPosition.x);
            transform.position = new Vector2(minmaxingX, transform.position.y);
        }
        if (Left?.IsEmpty ?? false)
        {

            float minmaxingX = Mathf.Max(Mathf.Min(localMousePosition.x, _anchoredPosition.x), Left.AnchoredPosition.x);
            transform.position = new Vector2(minmaxingX, transform.position.y);
        }
        if (Above?.IsEmpty ?? false)
        {

            float minmaxingY = Mathf.Min(Mathf.Max(localMousePosition.y, _anchoredPosition.y), Above.AnchoredPosition.y);
            transform.position = new Vector2(transform.position.x, minmaxingY);
        }
        if (Beneath?.IsEmpty ?? false)
        {

            float minmaxingY = Mathf.Max(Mathf.Min(localMousePosition.y, _anchoredPosition.y), Beneath.AnchoredPosition.y);
            transform.position = new Vector2(transform.position.x, minmaxingY);
        }
    }

    public void OnEndDrag(BaseEventData eventData)
    {
        if (Beneath?.IsEmpty ?? false)
        {
            if (((Beneath.AnchoredPosition.y - _anchoredPosition.y) / 2) > (Beneath.AnchoredPosition.y - transform.position.y))
                transform.position = _anchoredPosition;
            else
            {
                transform.position = Beneath.AnchoredPosition;
                Beneath.transform.position = _originalPosition;
                OnTileSlied(this, Beneath);
            }
        }
        else if (Above?.IsEmpty ?? false)
        {
            if (((_anchoredPosition.y - Above.AnchoredPosition.y) / 2) > (transform.position.y - Above.AnchoredPosition.y))
                transform.position = _anchoredPosition;
            else
            {
                transform.position = Above.AnchoredPosition;
                Above.transform.position = _originalPosition;
                OnTileSlied(this, Above);
            }
        }

        if (Left?.IsEmpty ?? false)
        {
            if (((Left.AnchoredPosition.x - _anchoredPosition.x) / 2) > (Left.AnchoredPosition.x - transform.position.x))
                transform.position = _anchoredPosition;
            else
            {
                transform.position = Left.AnchoredPosition;
                Left.transform.position = _originalPosition;
                OnTileSlied(this, Left);
            }
        }
        else if (Right?.IsEmpty ?? false)
        {
            if (((_anchoredPosition.x - Right.AnchoredPosition.x) / 2) > (transform.position.x - Right.AnchoredPosition.x))
                transform.position = _anchoredPosition;
            else
            {
                transform.position = Right.AnchoredPosition;
                Right.transform.position = _originalPosition;
                OnTileSlied(this, Right);
            }
        }

    }

    private 

    // Start is called before the first frame update
    void Awake()
    {
#if UNITY_IPHONE || UNITY_EDITOR
        _picture.sprite = _iosPart;
#endif
    actualIndex = _originalIndex;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
