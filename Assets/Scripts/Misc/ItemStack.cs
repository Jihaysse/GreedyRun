using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemStack : MonoBehaviour
{
    [Tooltip("The parent to attach the GameObjects to")]
    [SerializeField] Transform parent;

    [Tooltip("The offset on the Y axis between stacked objects")]
    [SerializeField] float offsetBetweenObjects = 1f;

    [Tooltip("The GameObject local scale when stacked")]
    [SerializeField] Vector3 objectScale;

    [Tooltip("The GameObject local position when stacked")]
    [SerializeField] Vector3 objectPosition;

    [Tooltip("The GameObject local rotation when stacked")]
    [SerializeField] Vector3 objectRotation;
    [SerializeField] Color blinkColor;

    [Tooltip("The scale punch tween which will scale the new item that will be pushed onto the stack")]
    [SerializeField] float punchScaleForce = .3f;

    [Tooltip("The power with which the stack will balance")]
    [SerializeField] float balancePower = 3f;
    [SerializeField] AudioSource popAudio;
    [SerializeField] AudioSource pushAudio;

    Stack<GameObject> stack = new Stack<GameObject>();
    Vector3 punchScale;

    public int Size { get { return stack.Count; } }

    void Start()
    {
        punchScale = new Vector3(punchScaleForce, punchScaleForce, punchScaleForce);

        BalanceStack();
    }

    public void StackGameObject(GameObject go)
    {
        stack.Push(go);
        pushAudio?.Play();

        BlinkColor(go);
        // place the GameObject onto the stack
        go.transform.SetParent(parent);
        go.transform.localScale = objectScale;
        go.transform.localPosition = new Vector3(objectPosition.x, objectPosition.y + offsetBetweenObjects * stack.Count, objectPosition.z);
        go.transform.localRotation = Quaternion.Euler(objectRotation);

        go.transform.DOPunchScale(punchScale, .4f, 10, 0f);
    }

    public void PopGameObject()
    {
        if (stack.Count < 1)
            return;
        
        GameObject lastObject = stack.Pop();
        popAudio?.Play();
        Destroy(lastObject);
    }

    void BlinkColor(GameObject go)
    {
        MeshRenderer renderer = go.GetComponent<MeshRenderer>();
        if (renderer == null) return;

        renderer.material.DOColor(blinkColor, .1f);
        renderer.material.DOColor(Color.white, 1f);
    }

    void BalanceStack()
    {
        float power = Random.Range(-balancePower, balancePower);
        Vector3 randomRotation = new Vector3(power, 0, power);
        transform.DOLocalRotate(randomRotation, 1f).OnComplete(() => BalanceStack());
    }



}
