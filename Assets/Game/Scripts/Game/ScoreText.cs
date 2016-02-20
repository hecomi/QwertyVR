using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    private Text text_;

    void Start()
    {
        text_ = GetComponent<Text>();
    }

	void Update()
    {
        text_.text = Score.GetScore().ToString();
	}
}
