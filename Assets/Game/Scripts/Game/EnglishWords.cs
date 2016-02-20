using UnityEngine;

public class EnglishWords : MonoBehaviour 
{
	static private EnglishWords instance;

	private const string wordListPath = "Xml/english-words";

	[SerializeField]
	private string[] words_;

	static public string[] words
	{
		get { return instance.words_; }
	}

	void Awake()
	{
		instance = this;
	}

	void Start() 
	{
		var file = Resources.Load(wordListPath) as TextAsset;
		words_ = file.text.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);

		for (int i = 0; i < words.Length; ++i) {
			words_[i] = words_[i].ToUpper();
		}
	}
}
