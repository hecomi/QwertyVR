using UnityEngine;
using UnityEngine.UI;

public class ScoreBar : MonoBehaviour
{
    private Image image_;

	void Start()
    {
        image_ = GetComponent<Image>();
	}
	
	void Update()
    {
        image_.fillAmount = Score.GetRatio();
	}
}
