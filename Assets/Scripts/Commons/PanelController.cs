using DG.Tweening;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    public delegate void postCloseFunction();
    public postCloseFunction closeFunction;
    public postCloseFunction openFunction;

    public void showPanel(float delay = 0f)
    {
        gameObject.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        gameObject.SetActive(true);
		transform.DOScale(1f, 0.2f).SetDelay(delay);
		GetComponent<CanvasGroup>().DOFade(1f,0.2f).SetDelay(delay).OnComplete(() => { openFunction?.Invoke(); openFunction = null; });
    }

    public void closePanel(float delay = 0f)
    {
        transform.DOScale(0.9f, 0.2f).SetDelay(delay);
		GetComponent<CanvasGroup>().DOFade(0f, 0.2f).SetDelay(delay).OnComplete(() => { closeFunction?.Invoke(); closeFunction = null; gameObject.SetActive(false); });
    }

}
